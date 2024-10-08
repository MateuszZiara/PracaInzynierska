namespace PracaInzynierska_RentIt.Server.Controllers.Tests.AspNetUsersEntity;

using Microsoft.AspNetCore.Mvc;
using Moq;
using PracaInzynierska_RentIt.Server.Controllers.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;
using PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;
using System.Threading.Tasks;
using Xunit;

public class AspNetUsersControllerTests
{
    private readonly AspNetUsersController _controller;
    private readonly Mock<IAspNetUsersService> _mockService;

    public AspNetUsersControllerTests()
    {
        _mockService = new Mock<IAspNetUsersService>();
        _controller = new AspNetUsersController(_mockService.Object);
    }

    [Fact]
    public void CheckEmail_ReturnsTrueBool()
    {
        var email = "test@example.com";
        _mockService.Setup(service => service.CheckEmail(email)).Returns(true);
        var result = _controller.CheckEmail(email);
        Assert.True(result);
    }

    [Fact]
    public async Task Register_ReturnsActionResult_WithAspNetUsers()
    {
        var userDto = new AspNetUsersRegisterDto { Email = "test@example.com", Password = "password" };
        var user = new AspNetUsers { Email = "test@example.com" };
        _mockService.Setup(service => service.Register(userDto)).ReturnsAsync(user);
        var result = await _controller.Register(userDto);
        var actionResult = Assert.IsType<ActionResult<AspNetUsers>>(result);
        var returnValue = Assert.IsType<AspNetUsers>(actionResult.Value);
        Assert.Equal(user.Email, returnValue.Email);
    }

    [Fact]
    public async Task GetUserInfo_ReturnsUserInfo()
    {
        // Arrange
        var userInfo = new AspNetUsersResponseDTO { Email = "test@example.com" };
        _mockService.Setup(service => service.GetUserInfo()).ReturnsAsync(userInfo);

        // Act
        var result = await _controller.GetUserInfo();

        // Assert
        Assert.Equal(userInfo.Email, result.Email);
    }

    [Fact]
    public void Edit_ReturnsBool()
    {
        // Arrange
        var editDto = new AspNetUsersEditDTO { Email = "new@example.com" };
        _mockService.Setup(service => service.Edit(editDto)).Returns(true);

        // Act
        var result = _controller.Edit(editDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ConfirmEmail_ReturnsOkResult()
    {
        // Arrange
        var userId = "user123";
        var token = "token123";
        _mockService.Setup(service => service.ConfirmEmail(userId, token)).ReturnsAsync(new OkResult());

        // Act
        var result = await _controller.ConfirmEmail(userId, token);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SendConfirmationEmail_ReturnsBool()
    {
        // Arrange
        _mockService.Setup(service => service.SendConfirmationEmail()).ReturnsAsync(true);

        // Act
        var result = await _controller.SendConfirmationEmail();

        // Assert
        Assert.True(result);
    }
}
