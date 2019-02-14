using System;
using BugTracker.Controllers;
using Xunit;
using Moq;
using BugTracker.Services;
using System.Collections.Generic;
using BugTracker.Repositories.Models;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerTests
{
    public class BugControllerTests
    {
        private Mock<IBugService> _service;

        public BugControllerTests()
        {
            _service = new Mock<IBugService>();
        }

        [Fact]
        public async void BugsController_When_Get_IsCalled_Then_AListOfBugsIsReturned()
        {
            var bugs = new List<Bug>();
            bugs.Add(new Bug { Title = "Test", Description = "Test description" });

            _service.Setup(_ => _.GetOpenBugs()).ReturnsAsync(bugs);
            var controller = new BugsController(_service.Object);

            var result = await controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBugs = Assert.IsType<List<Bug>>(okResult.Value);
            Assert.Single(returnedBugs);
        }

        [Fact]
        public async void BugsController_When_Get_IsCalledAndAnUnexpectedErrorOccurs_Then_500StatusCodeIsReturned()
        {
            _service.Setup(_ => _.GetOpenBugs()).Throws(new Exception("An exception"));
            var controller = new BugsController(_service.Object);

            var result = await controller.Get();

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_Get_IsCalledWithId_Then_ABugIsReturned()
        {
            var bug = new Bug { Id="123", Title = "Test", Description = "Test description" };

            _service.Setup(_ => _.FindBug("123")).ReturnsAsync(bug);
            var controller = new BugsController(_service.Object);

            var result = await controller.Get("123");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBug = Assert.IsType<Bug>(okResult.Value);
            Assert.Equal("123", returnedBug.Id);
        }

        [Fact]
        public async void BugsController_When_Get_IsCalledWithIdAndAnUnexpectedErrorOccurs_Then_500StatusCodeIsReturned()
        {
            _service.Setup(_ => _.FindBug("123")).Throws(new Exception("An exception"));
            var controller = new BugsController(_service.Object);

            var result = await controller.Get("123");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_Post_IsCalledWithABugAndABugIsCreatedSuccessfully_Then_Then201StatusCodeIsReturned()
        {
            var bug = new Bug { Id = "123", Title = "Test", Description = "Test description" };

            _service.Setup(_ => _.OpenBug(bug)).ReturnsAsync(true);
            var controller = new BugsController(_service.Object);

            var result = await controller.Post(bug);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_Post_IsCalledWithABugAndABugIsNotCreatedSuccessfully_Then_Then422StatusCodeIsReturned()
        {
            var bug = new Bug { Id = "123", Title = "Test", Description = "Test description" };

            _service.Setup(_ => _.OpenBug(bug)).ReturnsAsync(false);
            var controller = new BugsController(_service.Object);

            var result = await controller.Post(bug);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(422, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_Post_IsCalledWithABugAndAnUnexpectedErrorOccurs_Then_500StatusCodeIsReturned()
        {
            var bug = new Bug { Id = "123", Title = "Test", Description = "Test description" };

            _service.Setup(_ => _.OpenBug(bug)).Throws(new Exception("An exception"));
            var controller = new BugsController(_service.Object);

            var result = await controller.Post(bug);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_CloseBug_IsCalledWithAnIdAndABugIsClosedSuccessfully_Then_Then200StatusCodeIsReturned()
        {
            var id = "123";

            _service.Setup(_ => _.CloseBug(id)).ReturnsAsync(true);
            var controller = new BugsController(_service.Object);

            var result = await controller.CloseBug(id);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_CloseBug_IsCalledWithAnIdAndABugIsNotClosed_Then_Then501StatusCodeIsReturned()
        {
            var id = "123";

            _service.Setup(_ => _.CloseBug(id)).ReturnsAsync(false);
            var controller = new BugsController(_service.Object);

            var result = await controller.CloseBug(id);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(501, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_CloseBug_IsCalledWithAnIdAndAnUnexpectedErrorOccurs_Then_500StatusCodeIsReturned()
        {
            var id = "123";
            _service.Setup(_ => _.CloseBug(id)).Throws(new Exception("An exception"));
            var controller = new BugsController(_service.Object);

            var result = await controller.CloseBug(id);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_Update_IsCalledWithABugAndItIsUpdatedSuccessfully_Then_Then200StatusCodeIsReturned()
        {
            var bug = new Bug { Id = "123", Title = "Test", Description = "Test description" };

            _service.Setup(_ => _.UpdateBug(bug)).ReturnsAsync(true);
            var controller = new BugsController(_service.Object);

            var result = await controller.Update(bug);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_Update_IsCalledWithABugAndItIsNotUpdated_Then_Then501StatusCodeIsReturned()
        {
            var bug = new Bug { Id = "123", Title = "Test", Description = "Test description" };

            _service.Setup(_ => _.UpdateBug(bug)).ReturnsAsync(false);
            var controller = new BugsController(_service.Object);

            var result = await controller.Update(bug);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(501, statusResult.StatusCode);
        }

        [Fact]
        public async void BugsController_When_Update_IsCalledWithABugAndAnUnexpectedErrorOccurs_Then_500StatusCodeIsReturned()
        {
            var bug = new Bug { Id = "123", Title = "Test", Description = "Test description" };
            _service.Setup(_ => _.UpdateBug(bug)).Throws(new Exception("An exception"));
            var controller = new BugsController(_service.Object);

            var result = await controller.Update(bug);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}
