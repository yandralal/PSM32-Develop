using Psm32.Models;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace Psm32.Services;

public interface IComPortCommunicationService
{
    public void Start(Action<List<byte>> onCommandRecieved);
    public void Stop();
    public void EnqueueWriteMessage(TransferMessage message);

}
public class ComPortCommunicationService : IComPortCommunicationService
{
    private readonly IComPortWrapper _comPortWrapper;
    private readonly Thread _handlerThread;

    private Action<List<byte>>? _onCommandRecieved;
    private bool _keepRunning;
    private ConcurrentQueue<TransferMessage> _writeQueue;
    private ILogger _logger;

    public ComPortCommunicationService(
        IComPortWrapper comPortWrapper,
        ILogger logger)
    {
        _comPortWrapper = comPortWrapper;
        _logger = logger;
        _handlerThread = new Thread(HandleSerialPort);
        _writeQueue = new ConcurrentQueue<TransferMessage>();
        _keepRunning = false;
        _onCommandRecieved = null;
    }

    public void Start(Action<List<byte>> onCommandRecieved)
    {
        _onCommandRecieved = onCommandRecieved;
        try
        {
            _comPortWrapper.Open();
        } catch (Exception)
        {
            throw new Exception("Could not Open ComPort");
        }
        _keepRunning = true;
        _handlerThread.Start();
    }

    public void Stop()
    {
        _keepRunning = false;
        _comPortWrapper.Close();
    }

    public void EnqueueWriteMessage(TransferMessage message)
    {
        _writeQueue.Enqueue(message);
    }

    public static string[] GetAvailablePorts()
    {
        return SerialPort.GetPortNames();
    }

    private void HandleSerialPort()
    {
        List<byte> recievedData = new();
        int failedCount = 0;

        while (_keepRunning)
        {
            if (failedCount >= 200)
            {
                //stop if too many errors occured
                _keepRunning = false;
                _logger.Error("Too many errors occured while reading from serial port. Stopping the service.");
                continue;
            }
            try
            {
                failedCount = 0;
                if (_writeQueue.TryDequeue(out var message))
                {
                    _comPortWrapper.Write(message.Serialize());
                }

                //TODO: handle concurrency?
                var data = _comPortWrapper.Read();
                if (data == null)
                {
                    continue;
                }
                recievedData.AddRange(data);
                var extractedMessage = ExtractMessageFromStream(recievedData);
                if (extractedMessage is not null)
                {
                    recievedData.RemoveRange(0, extractedMessage.Count);

                    _onCommandRecieved?.Invoke(extractedMessage);
                }
                //TODO: handle reading message

            }
            catch (InvalidOperationException)
            {
                failedCount++;
                //TODO: The specified port is not open,  handle
            }
            catch (TimeoutException)
            {
                failedCount++;
                //TODO: timeout occured, handle
            }
            catch (ArgumentNullException)
            {
                failedCount++;
                //TODO: message is null, handle 
            }
        }
    }

    private static List<byte>? ExtractMessageFromStream(List<byte> recievedData)
    {
        recievedData = DiscardJunkCharacters(recievedData);

        var dataLength = TransferMessage.GetDataLength(recievedData.ToArray());

        if (dataLength is not null)
        {
            if (dataLength < 0)
            {
                //TODO: Means _recievedData[4] is not numeric => it is an ivalid message. Figure out how to handle this.
                throw new Exception("_recievedData[4]  must contain a number to indicate message lenght. TODO: figure out how to handle this.");
            }

            var messageLen = TransferMessage.GetMetadataLength() + dataLength.Value;
            if (recievedData.Count >= messageLen)
            {
                var message = recievedData.GetRange(0, messageLen);//TODO: test this based on the proper structure of the message
                return message;
            }
        }
        return null;
    }

    private static List<byte> DiscardJunkCharacters(List<byte> message)
    {
        //If message doesn't start with Q, it is junk and needs to be discarded
        int nonSyncCount = 0;
        foreach (byte b in message)
        {
            if (TransferMessage.IsSyncChar(Convert.ToChar(b)))
            {
                break;
            }
            nonSyncCount++;
        }

        return message.GetRange(nonSyncCount, message.Count - nonSyncCount);
    }
}
