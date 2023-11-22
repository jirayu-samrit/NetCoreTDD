using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NetCoreTDD.API.Models;
using NetCoreTDD.API.Services;
using NetCoreTDD.UnitTests.Fixtures;

namespace NetCoreTDD.UnitTests.Systems.Controllers;

public class UnitTest1
{
    [Fact]
    public async void Get_Onsuccess_ReturnsStatusCode200()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());

        var sut = new UsersController(mockUsersService.Object);

        // Act
        var result = (OkObjectResult)await sut.Get();

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokeUserServiceExactlyOnce()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(mockUsersService.Object);

        // Act
        var result = await sut.Get();

        // Assert
        mockUsersService
            .Verify(service => service.GetAllUsers(), Times.Once());
    }


    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfUsers()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());

        var sut = new UsersController(mockUsersService.Object);

        // Act
        var result = await sut.Get();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<User>>();
    }


    [Fact]
    public async Task Get_OnNoUsersFound_Return404()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(mockUsersService.Object);

        // Act
        var result = await sut.Get();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }
}