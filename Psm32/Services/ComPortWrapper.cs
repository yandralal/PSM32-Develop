using Psm32.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Psm32.Services;

public interface IComPortWrapper
{
    void Open();
    void Close();
    void Write(List<byte> message);
    List<byte> Read();
}
public class ComPortWrapper : IComPortWrapper, IDisposable
{
   

    private readonly SerialPort _serialPort;

    public ComPortWrapper(ComPortConfiguration comPortConfiguration)
    {
        _serialPort = new SerialPort(
            comPortConfiguration.PortName, 
            comPortConfiguration.BoudRate,
            comPortConfiguration.ParityValue,
            comPortConfiguration.ByteSize,
            comPortConfiguration.StopBitsValue
        );
        _serialPort.ReadTimeout = comPortConfiguration.ReadTimeOut;
        _serialPort.WriteTimeout = comPortConfiguration.WriteTimeOut;
    }

    public void Open()
    {
        if (_serialPort.IsOpen)
        {
            _serialPort.Close();
        }
        _serialPort.Open();
    }

    public void Close()
    {
        _serialPort.Close();
    }

    public void Dispose()
    {
        if (_serialPort.IsOpen)
        {
            _serialPort.Close();
        }
        _serialPort.Dispose();
    }

    public void Write(List<byte> message)
    {
        _serialPort.Write(message.ToArray(), 0, message.Count);
    }

    public List<byte> Read()
    {
        try
        {
            var message = new List<byte>();
            byte[] buffer = new byte[1024];

            while (_serialPort.Read(buffer, 0, 1024) > 0)
            {
                var byteList = new List<byte>(buffer);
                message.AddRange(byteList);
            }

            return message;
        } 
        catch (Exception)
        {
            return new List<byte>();
        }
    }
}
