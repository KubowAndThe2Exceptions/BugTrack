using Xunit;
using BugTrack.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTrackTests.Data;
using Microsoft.AspNetCore.Identity;
using BugTrack.Areas.Identity.Data;
using Moq;
using BugTrack.Models;
using Microsoft.AspNetCore.Mvc;
using BugTrack.ViewModels.VMIssueReportEntities;

namespace BugTrack.Controllers.Tests
{
    public class IssueReportsControllerTests : IClassFixture<TestDatabaseFixture>
    {
        public IssueReportsControllerTests(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        public TestDatabaseFixture Fixture { get; }


        [Fact()]
        public async Task Index_ReturnsAllIssues_AsList()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();

            var controller = new IssueReportsController(context, uManager.Object);

            //Act
            var result = await controller.Index("");


            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<IssueReportEntity>>(
            viewResult.ViewData.Model);

            Assert.Equal(context.IssueReport.Count(), model.Count());
        }

        [Fact()]
        public async Task Index_ReturnsSearchedIssues_AsList()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);

            //Act
            var result = await controller.Index("Test-Title 2");

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<IssueReportEntity>>(
                viewResult.ViewData.Model);
            Assert.Single(model);

        }

        [Fact()]
        public async Task Details_ReturnsNotFound_IfIdNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);

            //Act
            var result = await controller.Details(null);
            
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact()]
        public async Task Details_ReturnsNotFound_IfIssueReportNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);

            //Act
            var result = await controller.Details(1234556666);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact()]
        public async Task DetailsPost_ReturnsNotFound_IfIdNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);
            //Act
            var result = await controller.Details(null, "");

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact()]
        public async Task DetailsPost_ReturnsNotFound_IfIssueReportNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);
            //Act
            var result = await controller.Details(1293785, "");

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact()]
        public async Task DetailsPost_AddsComments_AndHasCommentsInModel()
        {
            //Will need a transaction and then a context.changetracker.clear()
            Assert.True(false, "Test Unwritten");
        }

        [Fact()]
        public async Task CreatePost_ReturnsModel_IfModelStateInvalid()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);
            controller.ModelState.AddModelError("IssueTitle", "Required");

            var issueReportVM = new IssueReportEntityViewModel { GeneralDescription = "foo", DateFound = DateTime.Now, ReplicationDescription = "bar",
                IssueTitle = "Something", ThreatLevel = 1 };
            
            
            //Act
            var result = await controller.Create(issueReportVM);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IssueReportEntityViewModel>(viewResult.ViewData.Model);

        }

        [Fact()]
        public async Task CreatePost_ReturnsRedirectToAction_IfModelStateValid_AndCreates()
        {
            //Will need a transaction and then a context.changetracker.clear()
            Assert.True(false, "Test Unwritten");
        }

        [Fact()]
        public async Task Edit_ReturnsNotFound_IfIdNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);

            //Act
            var result = await controller.Edit(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact()]
        public async Task Edit_ReturnsNotFound_IfIssueReportEntityNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);

            //Act
            var result = await controller.Edit(123441);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact()]
        public async Task Edit_ReturnsIssueReportEntityVM()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);


            //Act
            var result = await controller.Edit(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IssueReportEntityWithIdViewModel>(
                viewResult.ViewData.Model);
        }
        [Fact()]
        public async Task EditPost_ReturnsNotFound_IfIdDoesntMatchVMId()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);
            var issueReportVM = new IssueReportEntityWithIdViewModel();
            issueReportVM.Id = 1;

            //Act
            var result = await controller.Edit(233234, issueReportVM);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact()]
        public async Task EditPost_ReturnsNotFound_IfIssueReportEntityNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);
            var issueReportVM = new IssueReportEntityWithIdViewModel();
            issueReportVM.Id = 123441;

            //Act
            var result = await controller.Edit(123441, issueReportVM);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact()]
        public async Task EditPost_UpdatesDatabase_AndReturnsRedirectToActionResult()
        {
            Assert.True(false, "Test Unwritten");
        }
        [Fact()]
        public async Task Delete_ReturnsNotFound_IfIdNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);

            //Act
            var result = await controller.Delete(null);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact()]
        public async Task Delete_ReturnsNotFound_IfIssueReportEntityNull()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);

            //Act
            var result = await controller.Delete(123441);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact()]
        public async Task DeleteConfirmed_RemovesIssueReportEntity()
        {
            Assert.True(false, "Test Unwritten");
        }
    }
}