using System;
using Xunit;
using Moq;
using BugTracker.Repositories;
using BugTracker.Repositories.Models;
using BugTracker.Services;

namespace BugTrackerTests
{
    public class BugServiceTests
    {
        private readonly Mock<IDatabase<Bug>> _repository;

        public BugServiceTests()
        {
            _repository = new Mock<IDatabase<Bug>>();
        }

        [Fact]
        public void BugService_When_OpenBugIsCalledWithABugWithMissingMandatoryData_Then_ApplicationExceptionIsThrown()
        {
            var bug = new Bug();

            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.OpenBug(bug));
        }

        [Fact]
        public async void BugService_When_OpenBugIsCalledWithABugWithMandatoryData_Then_ReturnsTrue()
        {
            var bug = new Bug { Title = "Test Bug", Description = "Test description" };

            _repository.Setup(_ => _.Create(bug)).ReturnsAsync(true);
            var service = new BugService(_repository.Object);

            var result = await service.OpenBug(bug);

            Assert.Equal(Status.Opened, bug.Status);
            Assert.NotEqual(DateTime.MinValue, bug.ReportedOn);
            Assert.True(result);
        }

        [Fact]
        public async void BugService_When_OpenBugIsCalledWithABugWithMandatoryDataAndRepositoryReturnsFalse_Then_ReturnsFalse()
        {
            var bug = new Bug { Title = "Test Bug", Description = "Test description" };

            _repository.Setup(_ => _.Create(bug)).ReturnsAsync(false);
            var service = new BugService(_repository.Object);

            var result = await service.OpenBug(bug);

            Assert.False(result);
        }

        [Fact]
        public void BugService_When_CloseBugIsCalledWithAEmptyId_Then_ApplicationExceptionIsThrown()
        {
            var id = "";

            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.CloseBug(id));
        }

        [Fact]
        public void BugService_When_CloseBugIsCalledWithAEmptySpaceId_Then_ApplicationExceptionIsThrown()
        {
            var id = " ";

            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.CloseBug(id));
        }

        [Fact]
        public void BugService_When_CloseBugIsCalledWithAnIdThatDoesntExist_Then_ApplicationExceptionIsThrown()
        {
            var id = "123";
            Bug bug = null;
            _repository.Setup(_ => _.Find(id)).ReturnsAsync(bug);
            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.CloseBug(id));
        }

        [Fact]
        public async void BugService_When_CloseBugIsCalledWithAnIdThatExistsAndUpdateBugIsCalledOnRepositorySuccessfullyThen_TrueIsReturned()
        {
            var id = "123";
            var bug = new Bug { Id= "123", Title= "Test Title", Description="Test description", ClosedOn=DateTime.MinValue, Status = Status.Opened};
            _repository.Setup(_ => _.Find(id)).ReturnsAsync(bug);
            _repository.Setup(_ => _.Update(bug)).ReturnsAsync(true);
            var service = new BugService(_repository.Object);

            var result = await service.CloseBug(id);
            Assert.Equal(Status.Closed, bug.Status);
            Assert.NotEqual(DateTime.MinValue, bug.ClosedOn);
            Assert.True(result);
        }

        [Fact]
        public async void BugService_When_CloseBugIsCalledWithAnIdThatExistsAndUpdateBugIsCalledOnRepositoryButFails_Then_FalseIsReturned()
        {
            var id = "123";
            var bug = new Bug { Id = "123", Title = "Test Title", Description = "Test description", ClosedOn = DateTime.MinValue, Status = Status.Opened };
            _repository.Setup(_ => _.Find(id)).ReturnsAsync(bug);
            _repository.Setup(_ => _.Update(bug)).ReturnsAsync(false);
            var service = new BugService(_repository.Object);

            var result = await service.CloseBug(id);
            Assert.False(result);
        }

        [Fact]
        public void BugService_When_UpdateBugIsCalledWithAEmptyId_Then_ApplicationExceptionIsThrown()
        {
            var bug = new Bug { Id = "" };

            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.UpdateBug(bug));
        }

        [Fact]
        public void BugService_When_UpdateBugIsCalledWithAEmptySpaceId_Then_ApplicationExceptionIsThrown()
        {
            var bug = new Bug { Id = " " };

            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.UpdateBug(bug));
        }

        [Fact]
        public void BugService_When_UpdateBugIsCalledWithAnIdThatDoesntExist_Then_ApplicationExceptionIsThrown()
        {
            var bug = new Bug { Id = "123" };
            Bug nullBug = null;
            _repository.Setup(_ => _.Find(bug.Id)).ReturnsAsync(nullBug);
            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.UpdateBug(bug));
        }

        [Fact]
        public async void BugService_When_UpdateBugIsCalledWithAnIdThatExistsAndUpdateBugIsCalledOnRepositorySuccessfullyThen_TrueIsReturned()
        {
            var newBugData = new Bug 
            {
                Id = "123", 
                Title = "Updated Title", 
                Description = "Updated description", 
                ClosedOn = DateTime.Now, 
                Status = Status.Closed,
                AssignedTo = "User",
                ReportedBy = "User",
                ReportedOn = DateTime.Now,
                Severity = Severity.High
            };
            var existingBugData = new Bug
            {
                Id = "123",
                Title = "Existing Title",
                Description = "Existing description",
                ClosedOn = DateTime.MinValue,
                Status = Status.Opened,
                AssignedTo = "",
                ReportedBy = "",
                ReportedOn = DateTime.MinValue,
                Severity = Severity.Low
            };
            _repository.Setup(_ => _.Find(newBugData.Id)).ReturnsAsync(existingBugData);
            _repository.Setup(_ => _.Update(existingBugData)).ReturnsAsync(true);
            var service = new BugService(_repository.Object);

            var result = await service.UpdateBug(newBugData);

            Assert.Equal(newBugData.Id, existingBugData.Id);
            Assert.Equal(newBugData.Title, existingBugData.Title);
            Assert.Equal(newBugData.Description, existingBugData.Description);
            Assert.Equal(newBugData.ClosedOn, existingBugData.ClosedOn);
            Assert.Equal(Status.Assigned, existingBugData.Status);
            Assert.Equal(newBugData.AssignedTo, existingBugData.AssignedTo);
            Assert.Equal(newBugData.ReportedBy, existingBugData.ReportedBy);
            Assert.Equal(newBugData.ReportedOn, existingBugData.ReportedOn);
            Assert.Equal(newBugData.Severity, existingBugData.Severity);

            Assert.True(result);
        }

        [Fact]
        public async void BugService_When_UpdateBugIsCalledWithAnIdThatExistsAndUpdateBugIsCalledOnRepositorySuccessfullyAndAssignedToIsntChangedThen_TrueIsReturned()
        {
            var newBugData = new Bug
            {
                Id = "123",
                Title = "Updated Title",
                Description = "Updated description",
                ClosedOn = DateTime.Now,
                Status = Status.Closed,
                AssignedTo = "User",
                ReportedBy = "User",
                ReportedOn = DateTime.Now,
                Severity = Severity.High
            };
            var existingBugData = new Bug
            {
                Id = "123",
                Title = "Existing Title",
                Description = "Existing description",
                ClosedOn = DateTime.MinValue,
                Status = Status.Opened,
                AssignedTo = "User",
                ReportedBy = "",
                ReportedOn = DateTime.MinValue,
                Severity = Severity.Low
            };
            _repository.Setup(_ => _.Find(newBugData.Id)).ReturnsAsync(existingBugData);
            _repository.Setup(_ => _.Update(existingBugData)).ReturnsAsync(true);
            var service = new BugService(_repository.Object);

            var result = await service.UpdateBug(newBugData);

            Assert.Equal(newBugData.Id, existingBugData.Id);
            Assert.Equal(newBugData.Title, existingBugData.Title);
            Assert.Equal(newBugData.Description, existingBugData.Description);
            Assert.Equal(newBugData.ClosedOn, existingBugData.ClosedOn);
            Assert.Equal(newBugData.Status, existingBugData.Status);
            Assert.Equal(newBugData.AssignedTo, existingBugData.AssignedTo);
            Assert.Equal(newBugData.ReportedBy, existingBugData.ReportedBy);
            Assert.Equal(newBugData.ReportedOn, existingBugData.ReportedOn);
            Assert.Equal(newBugData.Severity, existingBugData.Severity);

            Assert.True(result);
        }

        [Fact]
        public async void BugService_When_UpdateBugIsCalledWithAnIdThatExistsAndUpdateBugCalledOnRepositoryFails_Then_FalseIsReturned()
        {
            var newBugData = new Bug
            {
                Id = "123",
                Title = "Updated Title",
                Description = "Updated description",
                ClosedOn = DateTime.Now,
                Status = Status.Closed,
                AssignedTo = "User",
                ReportedBy = "User",
                ReportedOn = DateTime.Now,
                Severity = Severity.High
            };
            var existingBugData = new Bug
            {
                Id = "123",
                Title = "Existing Title",
                Description = "Existing description",
                ClosedOn = DateTime.MinValue,
                Status = Status.Opened,
                AssignedTo = "",
                ReportedBy = "",
                ReportedOn = DateTime.MinValue,
                Severity = Severity.Low
            };
            _repository.Setup(_ => _.Find(newBugData.Id)).ReturnsAsync(existingBugData);
            _repository.Setup(_ => _.Update(existingBugData)).ReturnsAsync(false);
            var service = new BugService(_repository.Object);

            var result = await service.UpdateBug(newBugData);

            Assert.False(result);
        }

        [Fact]
        public void BugService_When_FindBugIsCalledWithAEmptyId_Then_ApplicationExceptionIsThrown()
        {
            var id = "";

            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.FindBug(id));
        }

        [Fact]
        public void BugService_When_FindBugIsCalledWithAEmptySpaceId_Then_ApplicationExceptionIsThrown()
        {
            var id = " ";

            var service = new BugService(_repository.Object);

            Assert.ThrowsAsync<ApplicationException>(() => service.FindBug(id));
        }

        [Fact]
        public async void BugService_When_FindBugIsCalledWithAValidId_Then_ABugIsReturned()
        {
            var id = "123";
            var bug = new Bug { Id = "123", Title = "Bug Title", Description = "Bug Description" };

            _repository.Setup(_ => _.Find(id)).ReturnsAsync(bug);
            var service = new BugService(_repository.Object);

            var result = await service.FindBug(id);
            Assert.Same(bug, result);
        }

        [Fact]
        public async void BugService_When_FindBugIsCalledWithAValidIdButNoBugFound_Then_NullIsReturned()
        {
            var id = "123";
            Bug bug = null;

            _repository.Setup(_ => _.Find(id)).ReturnsAsync(bug);
            var service = new BugService(_repository.Object);

            var result = await service.FindBug(id);
            Assert.Null(result);
        }

        [Fact]
        public async void BugService_When_GetOpenBugsIsCalled_Then_FindAllIsCalledOnRepository()
        {
            var service = new BugService(_repository.Object);

            await service.GetOpenBugs();
            _repository.Verify(_ => _.FindAll(x => x.Status != Status.Closed), Times.Once);
        }
    }
}
