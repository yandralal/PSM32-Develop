

using Psm32.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Psm32.Models;

public class TransferMessage 
{
    private static readonly char SYNC_CHAR = 'Q';
    private static readonly int MAX_RETRIES = 3;
    private static byte _currentMessageId = 1;
    private readonly Object _currentMessageIdLock = new();
    private byte _messageId;
    private byte _retires;
    private DateTime _sentTime;

    public CommandMessage CommandMessage { get; }
    public char Sync { get; }
    public DateTime SentTime => _sentTime;
    public int Retries => _retires;

    public byte MessageId => _messageId;

    public TransferMessage (CommandMessage commandMessage)
    {
        Sync = SYNC_CHAR;
        CommandMessage = commandMessage;
        SetMessageId();
        _sentTime = DateTime.Now;
        _retires = 1;
    }

    private void SetMessageId()
    {
        lock (_currentMessageIdLock)
        {
            _messageId = _currentMessageId;

            _currentMessageId += 1;
            if (_currentMessageId > 254)
                _currentMessageId = 1;
        }
    }

    public bool IsAck()
    {
        return CommandMessage.IsAck();
    }

    public bool IsNack()
    {
        return CommandMessage.IsNack();
    }

    public bool CanResend()
    {
        return _retires < MAX_RETRIES;
    }

    public void PrepareToResend()
    {
        SetMessageId();
        _retires++;
        _sentTime = DateTime.Now;
    }

    public static void ResetMessageId()
    {
        _currentMessageId = 1;
    }

    public List<byte> Serialize()
    {
        var message = new List<byte> { Convert.ToByte(Sync), MessageId };
        message.AddRange(CommandMessage.Serialize());
        message.Add(GetCRC8(message));

        return message;
    }

    public static TransferMessage Parse(List<byte> message)
    {
        var bytesArr = message.ToArray();

        int idx = 0;
        //TODO: finish and test this
        if (bytesArr[idx] != SYNC_CHAR)
        {
            throw new TransferMessageException($"Parse command failed, invalid or missing sync byte");
        }

        var messageId = bytesArr[idx++];
        var commandMessage = CommandMessage.Parse(bytesArr[2..]);

        var parsedMessage = new TransferMessage(commandMessage)
        {
            _messageId = messageId
        };

        return parsedMessage;
    }

    public static bool IsSyncChar(char c)
    {
        return c == SYNC_CHAR;
    }

    public static byte? GetDataLength(byte[] message)
    {
        if (message.Length <= 2)
        {
            return null;
        }
        return CommandMessage.GetDataLength(message[2..]);
    }

    public static int GetMetadataLength()
    {
        return 2 //Sync + MessageId
            + CommandMessage.GetMetadataLength();
    }

    private static byte GetCRC8(List<byte> message)
    {
        byte crc = 0x00;

        foreach (byte b in message)
        {
            byte tempB = b;
            for (byte i = 8; i > 0; i--)
            {
                var sum = (crc ^ tempB) & 0x01;

                crc >>= 1;

                if (sum != 0)
                    crc ^= 0x8C;

                tempB >>= 1;
            }
        }

        return (crc);
    }
}
