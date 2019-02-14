using System;
using System.Collections.Generic;
using BugTracker.Controllers;
using BugTracker.Repositories.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BugTrackerTests
{
    public class UsersControllerTests
    {
        private Mock<IUserService> _service;
        public UsersControllerTests()
        {
            _service = new Mock<IUserService>();
        }

        [Fact]
        public async void UsersController_When_Get_IsCalled_Then_AListOfUsersIsReturned()
        {
            var users = new List<User>();
            users.Add(new User { Username = "Test User" });

            _service.Setup(_ => _.GetUsers()).ReturnsAsync(users);
            var controller = new UsersController(_service.Object);

            var result = await controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Single(returnedUsers);
        }

        [Fact]
        public async void UsersController_When_Get_IsCalledAndAnUnexpectedErrorOccurs_Then_500StatusCodeIsReturned()
        {
            _service.Setup(_ => _.GetUsers()).Throws(new Exception("An exception"));
            var controller = new UsersController(_service.Object);

            var result = await controller.Get();

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async void UsersController_When_Post_IsCalledWithAUserAndAUserIsCreatedSuccessfully_Then_Then201StatusCodeIsReturned()
        {
            var user = new User { Username = "Test User" };

            _service.Setup(_ => _.AddUser(user)).ReturnsAsync(true);
            var controller = new UsersController(_service.Object);

            var result = await controller.Post(user);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, statusResult.StatusCode);
        }

        [Fact]
        public async void UsersController_When_Post_IsCalledWithAUserAndAUserIsNotCreatedSuccessfully_Then_Then422StatusCodeIsReturned()
        {
            var user = new User { Username = "Test User" };

            _service.Setup(_ => _.AddUser(user)).ReturnsAsync(false);
            var controller = new UsersController(_service.Object);

            var result = await controller.Post(user);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(422, statusResult.StatusCode);
        }

        [Fact]
        public async void UsersController_When_Post_IsCalledWithAUserAndAnUnexpectedErrorOccurs_Then_500StatusCodeIsReturned()
        {
            var user = new User { Username = "Test User" };

            _service.Setup(_ => _.AddUser(user)).Throws(new Exception("An exception"));
            var controller = new UsersController(_service.Object);

            var result = await controller.Post(user);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}
