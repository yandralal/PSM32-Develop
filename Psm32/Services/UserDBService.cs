using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Psm32.DB;
using Psm32.Models;
using Psm32.Exceptions;

namespace Psm32.Services;


public interface IUserDBService
{
    Task AddUser(User user);
    Task<User?> GetUser(string username);

}

public class UserDBService: IUserDBService
{
    private readonly Psm32DbContextFactory _psm32DbContextFactory;
    private readonly ILogger _logger;

    public UserDBService(
        Psm32DbContextFactory psm32DbContextFactory, 
        ILogger logger)
    {
        _psm32DbContextFactory = psm32DbContextFactory;
        _logger = logger;
    }

    public async Task AddUser(User user)
    {
        using var dbContext = _psm32DbContextFactory.CreateDbContext();
       
        if (dbContext.Users.Any(u => u.ID == user.ID))
        {
           _logger.Error("User with this ID already exists");
           throw new UserDBServiceException("User already exists");
        }
        if (dbContext.Users.Any(u => u.Username == user.Username))
        {
            _logger.Error("User with this Username already exists");
            throw new UserDBServiceException("User already exists");
        }
        
        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUser(string username)
    {
        using var dbContext = _psm32DbContextFactory.CreateDbContext();

        return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}
