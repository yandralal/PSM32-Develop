
using Psm32.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psm32.Services;

public interface IQSUScanner
{
    Task<List<QSUnit>> Scan();
}

public class QSUScanner : IQSUScanner
{
    private readonly IQSUMessageSender _messageSender;

    public QSUScanner(IQSUMessageSender messageSender)
    {
        _messageSender = messageSender;
    }


    public async Task<List<QSUnit>> Scan()
    {
        for (int i = 1; i <= QSUnit.MAX_UNIT_ID; i++)
        {
            //TODO:/ Implement this 
            _messageSender.QsuEnum(i);
        }

        //TODO: DEBUG CODE, REMOVE THIS
        var units = new List<QSUnit>()
        {
            BuildUnit(1),
            BuildUnit(2)
        };

        return await Task.FromResult(units);
    }

    private QSUnit BuildUnit(int iD)
    {
        var unit = new QSUnit(iD);
        unit.ChannelA.Status = MuscleStatus.Up;
        unit.ChannelB.Status = MuscleStatus.Up;
        unit.ChannelC.Status = MuscleStatus.NA;

        return unit;
    }
}
