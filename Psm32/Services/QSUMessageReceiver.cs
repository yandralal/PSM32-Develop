using Psm32.Models;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace Psm32.Services;


public interface IQSUMessageReceiver
{
    void ReceiveMessage(List<byte> message);
    void Start();
}


public class QSUMessageReceiver : IQSUMessageReceiver, IDisposable
{
    private const double RECIEVE_MESSAGE_TIMER_INTERVAL = 1000;
    private static ConcurrentQueue<TransferMessage> _receivedMessageQueue = new();
    private readonly IComPortCommunicationService _comPortCommunicationService;
    private readonly IQSUMessageSender _qsuMessageSender;
    private readonly ILogger _logger;
    private Timer _timer;

    public QSUMessageReceiver(
        IComPortCommunicationService comPortService, 
        IQSUMessageSender qsuMessageSender, 
        ILogger logger)
    {
        _comPortCommunicationService = comPortService;
        _qsuMessageSender = qsuMessageSender;
        _logger = logger;
        _timer = new Timer(interval: RECIEVE_MESSAGE_TIMER_INTERVAL);
        _timer.Elapsed += (sender, e) => OnTimer();
    }

    public void Start()
    {
        _timer.Start();
        _comPortCommunicationService.Start(ReceiveMessage);
    }

    public static void ResetReceivedMessagesQueue()
    {
        _receivedMessageQueue.Clear();
    }

    public void ReceiveMessage(List<byte> message)
    {
        var command = TransferMessage.Parse(message);

        _receivedMessageQueue.Enqueue(command);
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }

    private void OnTimer()
    {
        if (_timer == null)
            return;

        if (_receivedMessageQueue.TryDequeue(out var recievedMessage))
        {

            var sentMessage = QSUMessageSender.FindSentMessage(recievedMessage.MessageId);
            if (sentMessage == null)
            {
                throw new Exception($"Sent Message for ID `{recievedMessage.MessageId}` not found "); //TODO: what to do here?
            }

            if (recievedMessage.IsAck())
            {
                //TODO: what are we doing here? nothing?
            }
            else if (recievedMessage.IsNack())
            {
                _qsuMessageSender.ResendMessage(sentMessage);
            }
            else
            {
                //TODO: Data Message
            }

        } //TODO: finish this

        var timedOutMessages = _qsuMessageSender.GetTimedOutMessages();
        foreach (var message in timedOutMessages)
        {
            _qsuMessageSender.ResendMessage(message);
        }
    }
}
