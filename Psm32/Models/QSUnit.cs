using System;

namespace Psm32.Models;

public enum UnitStatus
{
    Ok,
    Error,
    NA
}

public class QSUnit
{
    public static readonly int MAX_UNIT_ID = 8;
    public int ID => _id;
    public UnitStatus Status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;
        }
    }
    public Muscle ChannelA
    {
        get
        {
            return _channelA;
        }
        set
        {
            _channelA = value;
        }
    }

    public Muscle ChannelB
    {
        get
        {
            return _channelB;
        }
        set
        {
            _channelB = value;
        }
    }

    public Muscle ChannelC
    {
        get
        {
            return _channelC;
        }
        set
        {
            _channelC = value;
        }
    }
    public Muscle ChannelD
    {
        get
        {
            return _channelD;
        }
        set
        {
            _channelD = value;
        }
    }
    public Muscle ChannelG
    {
        get
        {
            return _channelG;
        }
        set
        {
            _channelG = value;
        }
    }

    //public IEnumerable<Channel> Channels => _channels;

    public QSUnit(int iD)
    {
        if (iD <= 0 || iD > MAX_UNIT_ID)
        {
            throw new Exception($"Invalid Unit Id `{iD}`");
        }

        _channelA = new Muscle(iD, 'A');
        _channelB = new Muscle(iD, 'B');
        _channelC = new Muscle(iD, 'C');
        _channelD = new Muscle(iD, 'D');
        _channelG = new Muscle(iD, 'G');

        _id = iD;
        _status = UnitStatus.Ok;
    }

    public QSUnit(QSUnit unit)
    {
        _id = unit.ID;
        _status = unit.Status;
        _channelA = new Muscle(unit.ChannelA);
        _channelB = new Muscle(unit.ChannelB);
        _channelC = new Muscle(unit.ChannelC);
        _channelD = new Muscle(unit.ChannelD);
        _channelG = new Muscle(unit.ChannelG);
    }

    public override bool Equals(object? obj)
    {
        QSUnit? unit = obj as QSUnit;

        if (unit == null)
        {
            return false;
        }

        return _channelA.Equals(unit.ChannelA)
            & _channelB.Equals(unit.ChannelB)
            & _channelC.Equals(unit.ChannelC)
            & _channelD.Equals(unit.ChannelD)
            & _channelG.Equals(unit.ChannelG)
            & _id == unit.ID
            & _status == unit.Status;
    }

    public override int GetHashCode()
    {
        return _channelA.GetHashCode()
            ^ _channelB.GetHashCode()
            ^ _channelC.GetHashCode()
            ^ _channelD.GetHashCode()
            ^ _channelG.GetHashCode()
            ^ _id.GetHashCode()
            ^ _status.GetHashCode();
    }

    public void SetUnitAsNA()
    {
        _status = UnitStatus.NA;
        ChannelA.Status = MuscleStatus.NA;
        ChannelB.Status = MuscleStatus.NA;
        ChannelC.Status = MuscleStatus.NA;
        ChannelD.Status = MuscleStatus.NA;
        ChannelG.Status = MuscleStatus.NA;
    }


    //private readonly List<Channel> _channels;
    private Muscle _channelA;
    private Muscle _channelB;
    private Muscle _channelC;
    private Muscle _channelD;
    private Muscle _channelG;
    private readonly int _id;
    private UnitStatus _status;
}
