using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NetCoreTDD.API.Config;
using NetCoreTDD.API.Models;
using NetCoreTDD.API.Services;
using NetCoreTDD.UnitTests.Fixtures;
using NetCoreTDD.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreTDD.UnitTests.Systems.Services
{
    public class TestUsersService
    {
        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest()
        {
            // Arrange
            var expectedResponse = UsersFixture.GetTestUsers();
            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com/users";

            var config = Options.Create(new UsersApiOption
            {
                EndPoint = endpoint
            });
            var sut = new UserService(httpClient, config);

            // Act
            await sut.GetAllUsers();

            // Assert
            // Verify HTTP request is made!
            handlerMock
                .Protected()
                .Verify(
                "SendAsync"
                , Times.Exactly(1)
                , ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get)
                , ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetAllUsers_WhenHits404_ReturnsEmptpyListOfUsers()
        {
            // Arrange
            var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com/users";

            var config = Options.Create(new UsersApiOption
            {
                EndPoint = endpoint
            });
            var sut = new UserService(httpClient, config);

            // Act
            var result = await sut.GetAllUsers();

            // Assert
            result.Count.Should().Be(0);
        }


        [Fact]
        public async Task GetAllUsers_WhenCalled_ReturnsListOfUsersOfExpectedSize()
        {
            // Arrange
            var expectedResponse = UsersFixture.GetTestUsers();
            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com/users";

            var config = Options.Create(new UsersApiOption
            {
                EndPoint = endpoint
            });
            var sut = new UserService(httpClient, config);

            // Act
            var result = await sut.GetAllUsers();

            // Assert
            result.Count.Should().Be(expectedResponse.Count);
        }


        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
        {
            // Arrange
            var expectedResponse = UsersFixture.GetTestUsers();
            var endpoint = "https://example.com/users";
            var handlerMock = MockHttpMessageHandler<User>
                .SetupBasicGetResourceList(expectedResponse, endpoint);
            var httpClient = new HttpClient(handlerMock.Object);

            var config = Options.Create(new UsersApiOption
            {
                EndPoint = endpoint
            });
            var sut = new UserService(httpClient, config);

            // Act
            var result = await sut.GetAllUsers();

            var uri = new Uri(endpoint);

            // Assert
            handlerMock
                .Protected()
                .Verify(
                "SendAsync"
                , Times.Exactly(1)
                , ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get 
                    && req.RequestUri == uri)
                , ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
