using Psm32.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psm32.Models;

public class CommandMessage
{
	private static readonly List<char> ALLOWED_COMMANDS = new() { 'A', 'C', 'E', 'F', 'G', 'H', 'I', 'K', 'N', 'P', 'R', 'S', 'T', 'U', 'V', 'X', 'Z', '*' };
	private static readonly char ACK_CMD = 'K';
	private static readonly char NACK_CMD = 'N';
	private static readonly int LENGTH_BYTE_IDX = 2;

	public byte QsuId { get; }
	public byte ChannelId { get; }
	public char Command { get; }
	public List<byte> Data { get; } 
	public CommandMessage(byte qsuId, byte channelId, char command, List<byte>? data = null)
	{
		QsuId = qsuId;
		ChannelId = channelId;
		Command = command;
		Data = new List<byte>();
		if (data != null)
		{
			Data.AddRange(data);
		}
		

		Validate();
	}

	private void Validate()
	{
		if (QsuId < 0 || QsuId > QSUnit.MAX_UNIT_ID)
		{
			throw new CommandMessageException($"Invalid QSU ID, given `{QsuId}`, must be between 0 and {QSUnit.MAX_UNIT_ID}");
		}

		if (ChannelId < 0 || ChannelId > Muscle.MAX_CHANNEL_ID)
		{
			throw new CommandMessageException($"Invalid Channel ID, given `{ChannelId}`, must be between 0 and {Muscle.MAX_CHANNEL_ID}");
		}

		if (!ALLOWED_COMMANDS.Contains(Command))
		{
			throw new CommandMessageException($"Command `{Command}` is not a valid command");
		}
	}

	public override bool Equals(object? obj)
	{
		CommandMessage? message = obj as CommandMessage;
		// If parameter is null return false:
		if (message == null)
		{
			return false;
		}

		// Return true if the fields match:
		return (QsuId == message.QsuId)
			&& (ChannelId == message.ChannelId)
			&& (Command == message.Command)
			&& DataEqual(message.Data);
	}

	public override int GetHashCode()
	{
		return QsuId.GetHashCode()
			^ ChannelId.GetHashCode()
			^ Command.GetHashCode()
			^ DataHash();
	}

	public bool IsAck()
	{
		return Command == ACK_CMD;
	}
	public bool IsNack()
	{
		return Command == NACK_CMD;
	}

	public List<byte> Serialize()
	{
		var length = (byte)(Data == null ? 0: Data.Count);
		var message = new List<byte> { CreateAddress(QsuId, ChannelId), Convert.ToByte(Command), length};
		if (Data != null)
		{
			message.AddRange(Data);
		}
		
		return message;
	}

	private bool DataEqual(List<byte> data)
	{
		if (Data.Count != data.Count)
			return false;
		
		for (var i = 0; i < Data.Count; i++)
        {
			if (Data[i] != data[i])
				return false;
        }
		return true;
    }

	private int  DataHash()
    {
		unchecked // Make sure overflow is allowed for integer hash codes
		{
			int hashCode = 17; // Initialize with a prime number
			foreach (var item in Data)
			{
				hashCode = hashCode * 31 + item.GetHashCode(); 
			}
			return hashCode;
		}
	}

	public static CommandMessage Parse(byte[] message)
    {
		int idx = 0;

		GetIdsFromAddress(message[idx++], out var qsuId, out var channelId);
		var command = Convert.ToChar(message[idx++]);
		var length = message[idx++]; //TODO: this is wrong, finish this and test, check length
		
		var data = new List<byte>();
		data.AddRange(message[idx..(idx + length)]);
		
		return new CommandMessage(qsuId, channelId, command, data);
	}

	public static byte CreateAddress (byte qsuId, byte channelId)
	{
		var low = (byte)qsuId & 0x0F;
		var high = (byte)channelId << 4;

		var result = low | high;

		return (byte)result;
	}

	public static void GetIdsFromAddress(byte address, out byte qsuId, out byte channelId)
	{
		qsuId = (byte)(address & 0x0F);
		channelId = (byte)(address >> 4);		
	}

	public static byte? GetDataLength(byte[] message)
    {
		if (message.Length < LENGTH_BYTE_IDX)
			return null;
		return message[LENGTH_BYTE_IDX];
    }

	public static int GetMetadataLength()
    {
		return 3; // QSU and Channel IDs, Command, Data Length
    }

}
