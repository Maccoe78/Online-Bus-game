using Moq;
using Xunit;
using OnlineBussen.Logic.Interfaces;
using OnlineBussen.Logic.Controllers;
using OnlineBussen.Logic.Models;

namespace OnlineBussen.Tests;

public class UserControllerTest
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;

    public UserControllerTest()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var expectedUser = new User { Username = "testuser", Password = "testpass" };
        _mockUserService
            .Setup(x => x.GetUserByUsernameAsync("testuser"))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUserByUsernameAsync("testuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Username, result.Username);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ReturnsUser_WhenUserDoesNotExists()
    {
        // Arrange
        var expectedUser = new User { Username = "testuser", Password = "testpass" };
        _mockUserService
            .Setup(x => x.GetUserByUsernameAsync("testuser"))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUserByUsernameAsync("user");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ReturnsError_WhenUsernameExists()
    {
        // Arrange
        var user = new User { Username = "oldname", Password = "pass" };

        _mockUserService
            .Setup(x => x.UsernameExistsAsync("existingname"))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateUserAsync(user, "existingname");

        // Assert
        Assert.False(result.succes);
        Assert.Equal("A User with that name already exists", result.message);
    }

    [Fact]
    public async Task UpdateUserAsync_ReturnsSucces()
    {
        var user = new User { Username = "oldname", Password = "pass" };
        _mockUserService
            .Setup(x => x.UsernameExistsAsync("newname"))
            .ReturnsAsync(false);

        var result = await _controller.UpdateUserAsync(user, "newname");

        Assert.True(result.succes);
        Assert.Equal("User succesfully updated", result.message);
    }

    [Fact]
    public async Task CreateUser_ReturnsSucces()
    {
        
        _mockUserService
            .Setup(x => x.UsernameExistsAsync("existinguser"))
            .ReturnsAsync(true);

        _mockUserService
            .Setup(x => x.UsernameExistsAsync("nonuser"))
            .ReturnsAsync(false);


        var result = await _controller.CreateUserAsync("nonuser", "newpass");

        Assert.True(result.success);
        Assert.Equal("User succesfully created", result.message);
    }

    [Fact]
    public async Task CreateUser_WhenUsernameExist()
    {
        _mockUserService
            .Setup(x => x.UsernameExistsAsync("existinguser"))
            .ReturnsAsync(true);

        var result = await _controller.CreateUserAsync("existinguser", "pass");

        Assert.False(result.success);
        Assert.Equal("A User with this name already exists", result.message);
    }

    [Fact]
    public async Task AuthenticateUser_ReturnsTrue()
    {
        var user = new User { Username = "testuser", Password = "testpass" };
        _mockUserService
            .Setup(x => x.GetUserByCredentialsAsync("testuser", "testpass"))
            .ReturnsAsync(user);

        var result = await _controller.AuthenticateUserAsync("testuser", "testpass");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task AuthenticateUser_ReturnsFalse_UserDoesNotExist()
    { 
        var user = new User { Username = "testuser", Password = "testpass" };
        _mockUserService
            .Setup(x => x.GetUserByCredentialsAsync("testuser", "testpass"))
            .ReturnsAsync(user);

        var result = await _controller.AuthenticateUserAsync("nonuser", "nonpass");

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateUser_ReturnTrue_CorrectPassword()
    {
        _mockUserService
            .Setup(x => x.GetUserByCredentialsAsync("testuser", "testpass"))
            .ReturnsAsync(new User { Username = "testuser", Password = "testpass" });


        var result = await _controller.AuthenticateUserAsync("testuser", "testpass");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task AuthenticateUser_ReturnFalse_InCorrectPassword()
    { 
        _mockUserService
            .Setup(x => x.GetUserByCredentialsAsync("testuser", "testpass"))
            .ReturnsAsync(new User { Username = "testuser", Password = "testpass" });

        
        var result = await _controller.AuthenticateUserAsync("testuser", "testwrong");

        Assert.Null(result);
    }
}
