using Psm32.Models;
using System;
using System.Collections.Generic;

namespace Psm32.Services;


public class CommandBuilder
{
#region QSUCommands

    public static CommandMessage QsuReset_Command(int? unitId)
    {
        var id = ValidateUnitId(unitId);
        return new CommandMessage(qsuId: (byte)id, channelId: 0, command: '*');
    }

    public static CommandMessage QsuEnum_Command(int unitId)
    {
        var id = ValidateUnitId(unitId);
        return new CommandMessage(qsuId: (byte)id, channelId: 0, command: 'E'); 
    }

#endregion

#region ChannelCommands

    public static CommandMessage ChannelGoIdle_Command(int? unitId, char? letter)
    {
        var id = ValidateUnitId(unitId);
        var channelId = GetChannelId(letter);

        return new CommandMessage(qsuId: (byte)id, channelId: (byte)channelId, command: 'I');
    }

    public static CommandMessage ChannelGoRun_Command(int? unitId, char? letter)
    {
        var id = ValidateUnitId(unitId);
        var channelId = GetChannelId(letter);

        return new CommandMessage(qsuId: (byte)id, channelId: (byte)channelId, command: 'G');
    }

    public static CommandMessage ChannelGoOff_Command(int? unitId, char? letter)
    {
        var id = ValidateUnitId(unitId);
        var channelId = GetChannelId(letter);

        return new CommandMessage(qsuId: (byte)id, channelId: (byte)channelId, command: 'F');
    }

    public static CommandMessage ChannelSetRamp_Command(int? unitId, char? letter, int rampTargetPercent, int rampPeriod)
    {
        var id = ValidateUnitId(unitId);    
        var channelId = GetChannelId(letter);

        if (rampTargetPercent < 0 || rampTargetPercent > 100)
        {
            throw new Exception($"Invalid Ramp Target Percent value: `{rampTargetPercent}`. Must be between 0 and 100");
        }

        if (rampPeriod < 0 || rampPeriod > 50) //TODO: where is the limitation to 50 coming from?
        {
            throw new Exception($"Invalid Ramp Period value: `{rampPeriod}`. Must be between 0 and 50");
        }

        var data = new List<byte> { (byte)rampTargetPercent, (byte)rampPeriod };
        return new CommandMessage(qsuId: (byte)id, channelId: (byte)channelId, command: 'R', data: data);
    }

    public static CommandMessage ChannelSetPulse_Command(int? unitId, char? letter, int ampPos, int ampNeg, int pwPos, int pwNeg, int frequency)
    {
        //TODO: figure out floating point format for Amp and Pw
        var id = ValidateUnitId(unitId);
        var channelId = GetChannelId(letter);

        if (ampPos < 0 || ampPos > 250)
        {
            throw new Exception($"Invalid Amp+ value: `{ampPos}`. Must be between 0 and 250");
        }

        if (ampNeg < 0 || ampNeg > 250)
        {
            throw new Exception($"Invalid Amp- value: `{ampNeg}`. Must be between 0 and 250");
        }

        if (pwPos < 0 || pwPos > 30)
        {
            throw new Exception($"Invalid PulseWidth+ value: `{pwPos}`. Must be between 0 and 30");
        }

        if (pwNeg < 0 || pwNeg > 30)
        {
            throw new Exception($"Invalid PulseWidth- value: `{pwNeg}`. Must be between 0 and 30");
        }

        if (frequency < 1 || frequency > 100)
        {
            throw new Exception($"Invalid Frequency value: `{frequency}`. Must be between 1 and 100");
        }

        var data = new List<byte> { (byte)frequency, (byte)ampPos, (byte)ampNeg, (byte)pwPos, (byte)pwNeg };
        return new CommandMessage(qsuId: (byte)id, channelId: (byte)channelId, command: 'P', data: data);
    }
 #endregion

    private static int ValidateUnitId(int? unitId)
    {
        if (unitId == null)
        {
            return 0;
        }

        if (unitId < 1 || unitId > QSUnit.MAX_UNIT_ID)
        {
            throw new Exception($"Invalid unitId `{unitId}`. Must be between 1 and {QSUnit.MAX_UNIT_ID}");
        }

        return unitId.Value;
    }

    private static int GetChannelId(char? letter)
    {
       return  letter == null ? 0 : Muscle.ChannelLetterToId(letter.Value);
    }

}
