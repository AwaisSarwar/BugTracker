using System;
using BugTracker.Repositories;
using BugTracker.Repositories.Models;
using BugTracker.Services;
using Moq;
using Xunit;

namespace BugTrackerTests
{
    public class UserServiceTests
    {
        private readonly Mock<IDatabase<User>> _repository;

        public UserServiceTests()
        {
            _repository = new Mock<IDatabase<User>>();
        }

        [Fact]
        public void UserService_When_AddUserIsCalledWithEmptyUsername_Then_ApplicationExceptionIsThrown()
        {
            var user = new User { Username = ""};

            var service = new UserService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.AddUser(user));
        }

        [Fact]
        public void UserService_When_AddUserIsCalledWithEmptySpaceUsername_Then_ApplicationExceptionIsThrown()
        {
            var user = new User { Username = " "};

            var service = new UserService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.AddUser(user));
        }

        [Fact]
        public async void UserService_When_AddUserIsCalledWithValidDataAndCreateUserIsCalledOnRepositorySuccessfully_Then_TrueIsReturned()
        {
            var user = new User { Username = "Test User" };
            _repository.Setup(_ => _.Create(user)).ReturnsAsync(true);
            var service = new UserService(_repository.Object);

            var result = await service.AddUser(user);
            Assert.True(result);
        }

        [Fact]
        public async void UserService_When_AddUserIsCalledWithValidDataAndCreateUserCallOnRepositoryFails_Then_FalseIsReturned()
        {
            var user = new User { Username = "Test User" };
            _repository.Setup(_ => _.Create(user)).ReturnsAsync(false);
            var service = new UserService(_repository.Object);

            var result = await service.AddUser(user);
            Assert.False(result);
        }

        [Fact]
        public async void UserService_When_GetUsersIsCalled_Then_FindAllIsCalledOnRepository()
        {
            var service = new UserService(_repository.Object);

            await service.GetUsers();
            _repository.Verify(_ => _.FindAll(), Times.Once);
        }
    }
}
