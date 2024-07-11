using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Psm32.Exceptions;
using Psm32.Models;
using Serilog;

namespace Psm32.Services;

public interface IAuthenticationService
{
    //TODO: need to return bool?
    Task<User> Login(string username, string password);
    Task Register(string username, string password, UserRole role);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserDBService _userDBService;
    private readonly ILogger _logger;

    public AuthenticationService(IUserDBService userDBService, ILogger logger)
    {
        _userDBService = userDBService;
        _logger = logger;
    }

    public async Task<User> Login(string username, string password)
    {
        var user = await _userDBService.GetUser(username);
        
        if (user == null)
        {
            _logger.Error($"Failed attempt to login. User `{username}` does not exist");
            throw new AuthenticationServiceException("Invalid user credentials");
        }

        if  (user.HashedPassword == HashPassword(password, user.Salt))
        {
            return user;
        }

        _logger.Error($"Failed attempt to login. Invalid password for user `{username}`");
        throw new AuthenticationServiceException("Invalid user credentials");
    }

    public async Task Register(string username, string password, UserRole role)
    {
        var salt = DateTime.Now.ToString();
        var hashedPassword = HashPassword(password, salt);
        var user = new User(username, hashedPassword, salt, role);

        try
        {
            await _userDBService.AddUser(user);
        }
        catch (UserDBServiceException e)
        {
            _logger.Error($"Failed to register user `{username}`");
            throw new AuthenticationServiceException(e.Message);
        }
    }


    public static string HashPassword(string password, string salt)
    {
        SHA256 hash = SHA256.Create();

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);

        byte[] hashedPasswordBytes = hash.ComputeHash(passwordBytes);

        return Convert.ToBase64String(hashedPasswordBytes);
    }
}
