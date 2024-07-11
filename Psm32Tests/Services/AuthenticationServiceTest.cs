using Moq;
using Psm32.Exceptions;
using Psm32.Models;
using Psm32.Services;
using Psm32Tests.Mocks;

namespace Psm32Tests.Services;

public class AuthenticationServiceTest
{
    private Mock<IUserDBService> _userDBServiceMock;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTest()
    {
        _userDBServiceMock = new Mock<IUserDBService>();
        _authenticationService = new AuthenticationService(
            _userDBServiceMock.Object,
            new LoggerMock().MockObject.Object);
    }

    [Fact]
    public async void Register_Success()
    {
        _userDBServiceMock.Setup(m => m.AddUser(It.IsAny<User>())).Verifiable();

        var user = new User("test", "test", "salt", UserRole.Clinician);

        await _authenticationService.Register("test", "test", UserRole.Tech);

        _userDBServiceMock.Verify(m => m.AddUser(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void Register_FailedRegister_Throws()
    {
        _userDBServiceMock.Setup(m => m.AddUser(It.IsAny<User>())).Returns((User user) =>
        {
            throw new AuthenticationServiceException("Dummy exception");
        });

        var exception = await Assert.ThrowsAsync<AuthenticationServiceException>(
            () => _authenticationService.Register("test", "test", UserRole.Tech));

        Assert.NotNull(exception);
    }

    [Fact]
    public async void Login_Success()
    {
        _userDBServiceMock.Setup(m => m.GetUser(It.IsAny<string>())).Returns((string username) =>
        {
            var hashedPassword = AuthenticationService.HashPassword("test", "salt");
            var newUser = new User("test", hashedPassword, "salt", UserRole.Clinician);
            return Task.FromResult(newUser ?? null);
        });

        var user = await _authenticationService.Login("test", "test");

        Assert.NotNull(user);
        Assert.Equal("test", user?.Username);
    }

    [Fact]
    public async void Login_WrongPassword_Throws()
    {
        _userDBServiceMock.Setup(m => m.GetUser(It.IsAny<string>())).Returns((string username) =>
        {
            var hashedPassword = AuthenticationService.HashPassword("test", "salt");
            var newUser = new User("test", hashedPassword, "salt", UserRole.Clinician);
            return Task.FromResult(newUser ?? null);
        });

        var exception = await Assert.ThrowsAsync<AuthenticationServiceException>(
            () => _authenticationService.Login("test", "wrongpassword"));

        Assert.NotNull(exception);  
        Assert.Equal("Invalid user credentials", exception.Message);
    }

    [Fact]
    public async void Login_UserNotFound_Throws()
    {
        _userDBServiceMock.Setup(m => m.GetUser(It.IsAny<string>())).Returns((string username) =>
        {
            User? newUser = null;
            return Task.FromResult(newUser ?? null);
        });

        var exception = await Assert.ThrowsAsync<AuthenticationServiceException>(
            () => _authenticationService.Login("test", "test"));

        Assert.NotNull(exception);
        Assert.Equal("Invalid user credentials", exception.Message);
    }
}
