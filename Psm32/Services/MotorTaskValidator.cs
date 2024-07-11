using Psm32.DB;
using Psm32.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace Psm32.Services;  

public interface IMotorTaskValidator //TODO: delete?
{
    Task<bool> DoesNameExist(MotorTask motorTask);
}

public class MotorTaskValidator : IMotorTaskValidator
{
    private readonly Psm32DbContextFactory _psm32DbdbContextFactory;

    public MotorTaskValidator(Psm32DbContextFactory psm32DbdbContextFactory)
    {
        _psm32DbdbContextFactory = psm32DbdbContextFactory;
    }

    public async Task<bool> DoesNameExist(MotorTask motorTask)
    {
        using var dbContext = _psm32DbdbContextFactory.CreateDbContext();

        return await dbContext.MotorTasks.AnyAsync(mt => mt.Name == motorTask.Name);
    }

    private static MotorTask ToMotorTask(MotorTaskDTO mtDTO)
    {
        var serializer = new MotorTaskConfigurationSerializer();
        var config = serializer.FromJson(mtDTO.Configuration);

        if (config == null)
        {
            throw Exception($"Invalid Motor Task Configuration read from DB for Motor Task `{mtDTO.Name}`");
        }
        return new MotorTask(mtDTO.ID, mtDTO.Name, mtDTO.PatientId, config, mtDTO.Created, mtDTO.Updated);

    }

    private static Exception Exception(string v)
    {
        throw new NotImplementedException();
    }
}
