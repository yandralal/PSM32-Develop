using Psm32.Models;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace Psm32.Services;


public interface IQSUMessageSender
{
    TransferMessage QsuReset(int unitId);
    TransferMessage QsuEnum(int unitId);
    TransferMessage ChannelGoIdle(int unitId, char? channelLetter);
    TransferMessage ChannelGoRun(int unitId, char? channelLetter);
    TransferMessage ChannelGoOff(int unitId, char? channelLetter);
    TransferMessage ChannelSetRamp(int unitId, char? channelLetter, int rampTargetPercent, int rampPeriod);
    TransferMessage ChannelSetPulse(int unitId, char? channelLetter, int ampPos, int ampNeg, int pwPos, int pwNeg, int frequency);

    TransferMessage? ResendMessage(TransferMessage message);
    bool MessageSent(byte MessageId);
    public List<TransferMessage> GetTimedOutMessages();
}


public class QSUMessageSender : IQSUMessageSender
{
    public static int WAIT_TIMEOUT_SEC { get; set; }

    private static ConcurrentDictionary<byte, TransferMessage> _sentMessagesDict = new();
    private readonly IComPortCommunicationService _comPortCommunicationService;
    private readonly ILogger _logger;

    public QSUMessageSender(IComPortCommunicationService comPortService, ILogger logger)
    {
        WAIT_TIMEOUT_SEC = 3;
        _comPortCommunicationService = comPortService;
        _logger = logger;
    }

    public static void ResetSentMessagesDict()
    {
        _sentMessagesDict.Clear();
    }

    public static TransferMessage? FindSentMessage(byte messageId)
    {
        _sentMessagesDict.TryRemove(messageId, out var sentMessage);
        return sentMessage;
    }

    public TransferMessage QsuReset(int unitId)
    {
        var command = CommandBuilder.QsuReset_Command(unitId);
        return SendMessage(command);
    }

    public TransferMessage QsuEnum(int unitId)
    {
        var commad = CommandBuilder.QsuEnum_Command(unitId);
        return SendMessage(commad);
    }

    public TransferMessage ChannelGoIdle(int unitId, char? channelLetter)
    {
        var commad = CommandBuilder.ChannelGoIdle_Command(unitId, channelLetter);
        return SendMessage(commad);
    }

    public TransferMessage ChannelGoRun(int unitId, char? channelLetter)
    {
        var commad = CommandBuilder.ChannelGoRun_Command(unitId, channelLetter);
        return SendMessage(commad);
    }

    public TransferMessage ChannelGoOff(int unitId, char? channelLetter)
    {
        var commad = CommandBuilder.ChannelGoOff_Command(unitId, channelLetter);
        return SendMessage(commad);
    }

    public TransferMessage ChannelSetRamp(int unitId, char? channelLetter, int rampTargetPercent, int rampPeriod)
    {
        var commad = CommandBuilder.ChannelSetRamp_Command(unitId, channelLetter, rampTargetPercent, rampPeriod);
        return SendMessage(commad);
    }

    public TransferMessage ChannelSetPulse(int unitId, char? channelLetter, int ampPos, int ampNeg, int pwPos, int pwNeg, int frequency)
    {
        var commad = CommandBuilder.ChannelSetPulse_Command(unitId, channelLetter, ampPos, ampNeg, pwPos, pwNeg, frequency);
        return SendMessage(commad);
    }

    private TransferMessage SendMessage(CommandMessage message)
    {
        var transferMessage = new TransferMessage(message);
        SendMessage(transferMessage);

        return transferMessage;
    }

    public TransferMessage? ResendMessage(TransferMessage message)
    {
        if (!message.CanResend())
        {
            //TODO: exposing too much info?
            _logger.Error($"Max send attepmts reached for message ID `{message.MessageId}` and command `{message.CommandMessage}`");
            return null;
        }

        message.PrepareToResend();
        SendMessage(message);
        return message;
    }

    public List<TransferMessage> GetTimedOutMessages()
    {
        var messages = new List<TransferMessage>();
        foreach (var message in _sentMessagesDict)
        {
           if ((DateTime.Now - message.Value.SentTime).TotalSeconds > WAIT_TIMEOUT_SEC)
            {
                _sentMessagesDict.TryRemove(message.Value.MessageId, out var sentMessage);
                if (sentMessage != null)
                {
                    messages.Add(sentMessage);
                }
            }
        }
        return messages;
    }

    private void SendMessage(TransferMessage transferMessage)
    {
        //TODO: add try catch
        _logger.Information($"Sending message with ID: `{transferMessage.MessageId}`");
        _comPortCommunicationService.EnqueueWriteMessage(transferMessage);

        //!!!!! This will fail if a message with such ID already exists in the Dict and
        //we haven't cleared it. TODO: figure this out~~~~
        if (!_sentMessagesDict.TryAdd(transferMessage.MessageId, transferMessage))
        {
            throw new Exception($"Message with ID `{transferMessage.MessageId}` is already in sent queue");
        }

    }

    public bool MessageSent(byte MessageId)
    {
        return _sentMessagesDict.ContainsKey(MessageId);
    }
}
