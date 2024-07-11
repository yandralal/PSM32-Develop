using System;
using System.Collections.Generic;

namespace Psm32.Models;

public enum MuscleStatus
{
	Up,
	Down,
	NA
}

public class Muscle
{
    private static readonly List<char> ALLOWED_CHANNEL_LETTERS = new() { 'A', 'B', 'C', 'D', 'G' };
	public static readonly int MAX_CHANNEL_ID = 4;

	public Muscle(int unitId, char letter)
	{
		ValidateChannelLetter(letter);

		if (unitId <= 0 || unitId > QSUnit.MAX_UNIT_ID )
        {
			throw new Exception($"Invalid Unit Id `{unitId}`");
		}

		Letter = letter;
		UnitId = unitId;
		Status = MuscleStatus.Down;
		ID = $"{UnitId}{Letter}";
		MuscleConfig = new MuscleConfig();
	}

    public Muscle(Muscle muscle)
	{
		Letter = muscle.Letter;
		UnitId = muscle.UnitId;
		ID = muscle.ID;
		Status = muscle.Status;
		MuscleConfig = new MuscleConfig(muscle.MuscleConfig);
	}

    public Muscle(
		string id, 
		string shortName, 
		decimal ampPos, 
		decimal ampNeg, 
		decimal pwPos, 
		decimal pwNeg, 
		int freq, 
		string polarity, 
		string side,
		string status,
		bool enabled)
    {
        var unitId = (int)id[0] - '0';
        var letter = id[1];

        ValidateChannelLetter(letter);

        if (unitId <= 0 || unitId > QSUnit.MAX_UNIT_ID)
        {
            throw new Exception($"Invalid Unit Id `{unitId}`");
        }

        try
        {
            Status = (MuscleStatus)Enum.Parse(typeof(MuscleStatus), status);
        }
        catch
        {
            throw new ArgumentException("Invalid Channel Status Value");
        }

        Letter = letter;
        UnitId = unitId;
        Status = MuscleStatus.Down;
        ID = $"{UnitId}{Letter}";
		MuscleConfig = new MuscleConfig(shortName, ampPos, ampNeg, pwPos, pwNeg, freq, polarity, side, enabled);

    }

    public char Letter {get;}
	public int UnitId { get; }
	public string ID { get;  }
	public MuscleStatus Status { get; set; }

	public MuscleConfig MuscleConfig { get; set; }

    public bool AvailableForCommand()
    {
        return Status == MuscleStatus.Up && MuscleConfig.Enabled == true;
    }

    public static int ChannelLetterToId(char letter)
    {
		switch (char.ToUpper(letter))
        {
			case 'A':
				return 1;
			case 'B':
				return 2;
			case 'C':
				return 3;
			case 'D':
				return 4;
			default:
				throw new Exception($"Invalid Channel letter `{letter}`");
		}

	}

	public override bool Equals(object? obj)
	{
		Muscle? muscle = obj as Muscle;

		if (muscle == null)
		{
			return false;
		}

		return Letter == muscle.Letter
			& UnitId == muscle.UnitId
			& ID == muscle.ID
			& Status == muscle.Status
			& MuscleConfig == muscle.MuscleConfig;
	}

    public override int GetHashCode()
    {
		return Letter.GetHashCode()
			^ UnitId.GetHashCode()
			^ ID.GetHashCode()
			^ Status.GetHashCode()
			^ MuscleConfig.GetHashCode();
    }

	private static void ValidateChannelLetter(char letter)
	{
		if (!ALLOWED_CHANNEL_LETTERS.Contains(letter))
		{
			throw new Exception($"Invalid Channel letter `{letter}`");
		}
	}
}
