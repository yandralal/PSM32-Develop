using Microsoft.EntityFrameworkCore;
using Psm32.DB;
using Psm32.Models;
using Psm32.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Psm32.Services;

public interface IMotorTaskDBService
{
    Task SaveMotorTask(MotorTask motorTask);
    Task<IEnumerable<MotorTask>> GetAllMotorTasks();
    Task<MotorTask?> GetMotorTaskByName(string name);

}

public class MotorTaskDBService : IMotorTaskDBService
{
    private readonly Psm32DbContextFactory _psm32DbContextFactory;

    public MotorTaskDBService(Psm32DbContextFactory psm32DbContextFactory)
    {
        _psm32DbContextFactory = psm32DbContextFactory;
    }

    public async Task SaveMotorTask(MotorTask motorTask)
    {
        using var dbContext = _psm32DbContextFactory.CreateDbContext();
        var entity = ToMotorTaskDTO(motorTask);
        if (dbContext.MotorTasks.Any(mt => mt.ID == motorTask.ID))
        {
            dbContext.MotorTasks.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            dbContext.MotorTasks.Add(entity);
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<MotorTask>> GetAllMotorTasks()
    {
        using var dbContext = _psm32DbContextFactory.CreateDbContext();
        IEnumerable<MotorTaskDTO> motorTasksDTOs = await dbContext.MotorTasks
            .Select(mt => new MotorTaskDTO(
                mt.ID,
                mt.Name,
                mt.PatientId,
                mt.Configuration,  //TODO: don't select configuration for effeciency
                mt.Created,
                mt.Updated)).ToListAsync();

        return motorTasksDTOs.Select(mt => ToMotorTask(mt));
    }

    public async Task<MotorTask?> GetMotorTaskByName(string name)
    {
        using var dbContext = _psm32DbContextFactory.CreateDbContext();
        var motorTaskDTO = await dbContext.MotorTasks.FirstOrDefaultAsync(mt => mt.Name == name);

        return motorTaskDTO is null ? null : ToMotorTask(motorTaskDTO);
    }

    private static MotorTaskDTO ToMotorTaskDTO(MotorTask motorTask)
    {
        var serializer = new MotorTaskConfigurationSerializer();
        return new MotorTaskDTO(
            motorTask.ID,
            motorTask.Name,
            motorTask.PatientId,
            serializer.ToJson(motorTask.Configuration),
            motorTask.Created,
            motorTask.Updated);
    }

    private static MotorTask ToMotorTask(MotorTaskDTO mtDTO)
    {
        var serializer = new MotorTaskConfigurationSerializer();
        var configuration = serializer.FromJson(mtDTO.Configuration);

        if (configuration == null)
        {
            throw Exception("Invalid Motor Task Configuration read from DB");
        }

        return new MotorTask(
            mtDTO.ID, 
            mtDTO.Name, 
            mtDTO.PatientId,
            configuration, 
            mtDTO.Created, 
            mtDTO.Updated);

    }

    private static Exception Exception(string v)
    {
        throw new NotImplementedException();
    }
}
