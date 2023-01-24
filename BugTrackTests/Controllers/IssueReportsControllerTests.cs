using BugTrack.Areas.Identity.Data;
using BugTrack.Models;
using BugTrack.ViewModels.VMIssueReportEntities;
using BugTrack.ViewModels.VMIssueReportEntities.Helpers;
using BugTrackTests.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Security.Claims;
using Xunit;

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
            var model = Assert.IsAssignableFrom<List<IssueReportEntityWithIdViewModel>>(
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
            var model = Assert.IsAssignableFrom<List<IssueReportEntityWithIdViewModel>>(
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
            //Currently fails, needs specific mocking procedure of GetUserAsync,
            //which requires claims principal set up.

            //Will need a transaction and then a context.changetracker.clear()
            //Arrange
            using var context = Fixture.CreateContext();
            await context.Database.BeginTransactionAsync();
            var bugUser = new BugUser();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }));

            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);
            uManager.Setup(m => m.GetUserAsync(user)).ReturnsAsync(bugUser);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            var controller = new IssueReportsController(context, uManager.Object);
            controller.ControllerContext = controllerContext;

            var targetIssueReport = context.IssueReport.Where(i => i.Id == 2).FirstOrDefault();
            context.Entry(targetIssueReport).Collection(n => n.Comments).Load();
            int priorCount = targetIssueReport.Comments.Count();

            //Act
            var result = await controller.Details(2, "something of value");

            //Assert
            context.ChangeTracker.Clear();

            int postCount = targetIssueReport.Comments.Count();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IssueReportEntityWithIdViewModel>(viewResult.ViewData.Model);

            Assert.NotEqual(postCount, priorCount);
            Assert.NotEmpty(model.Comments);


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

            var issueReportVM = new IssueReportEntityEditCreateVM { GeneralDescription = "foo", DateFound = DateTime.Now, ReplicationDescription = "bar",
                IssueStatus = "1", IssueThreat = "1" 
            };
            
            
            //Act
            var result = await controller.Create(issueReportVM);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IssueReportEntityEditCreateVM>(viewResult.ViewData.Model);

        }

        [Fact()]
        public async Task CreatePost_ReturnsRedirectToAction_IfModelStateValid_AndCreates()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            await context.Database.BeginTransactionAsync();
            var bugUser = new BugUser();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }));

            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);
            uManager.Setup(m => m.GetUserAsync(user)).ReturnsAsync(bugUser);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            var controller = new IssueReportsController(context, uManager.Object);
            controller.ControllerContext = controllerContext;

            var issueReportVM = new IssueReportEntityEditCreateVM { DateFound = DateTime.Now, GeneralDescription = "foo",
                IssueTitle = "bar", ReplicationDescription = "foobar",
                IssueStatus = "1", IssueThreat = "1"
            };

            int priorCount = context.IssueReport.Count();

            //Act
            var result = await controller.Create(issueReportVM);

            //Assert
            int postCount = context.IssueReport.Count();

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotEqual(priorCount, postCount);
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
            var issueReportVM = new IssueReportEntityEditCreateVM();
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
            var issueReportVM = new IssueReportEntityEditCreateVM();
            issueReportVM.Id = 123441;

            //Act
            var result = await controller.Edit(123441, issueReportVM);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact()]
        public async Task EditPost_UpdatesDatabase_AndReturnsRedirectToActionResult()
        {
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);
            var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);
            context.Database.BeginTransaction();

            var issueReportVM = new IssueReportEntityEditCreateVM { Id = 1, DateFound = DateTime.Now, GeneralDescription = "Foo",
                ReplicationDescription = "bar", IssueTitle = "An issue",
                IssueStatus = "1", IssueThreat = "1"
            };

            //Act
            var result = await controller.Edit(1, issueReportVM);
            context.ChangeTracker.Clear();
            //Assert
            var updatedIssue = context.IssueReport.Where(m => m.Id == 1).FirstOrDefault();
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(issueReportVM.IssueTitle, updatedIssue.IssueTitle);
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
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();
            var controller = new IssueReportsController(context, uManager.Object);
            context.Database.BeginTransaction();

            //Act
            var result = await controller.DeleteConfirmed(1);
            context.ChangeTracker.Clear();
            //Assert

            var issueReport = context.IssueReport.Where(m => m.Id == 1).FirstOrDefault();
            var returnedResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(issueReport);
        }

        [Fact]
        public async Task MyIssues_ReturnsViewResult_WithUserIssues()
        {
            using var context = Fixture.CreateContext();
            var bugUser = new BugUser { Id="TESTID-T1"};
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }));

            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);
            uManager.Setup(m => m.GetUserAsync(user)).ReturnsAsync(bugUser);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            var controller = new IssueReportsController(context, uManager.Object);
            controller.ControllerContext = controllerContext;

            //Act
            var result = await controller.MyIssues();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<IssueReportEntityWithIdViewModel>>(viewResult.ViewData.Model);
            Assert.NotNull(model);

        }
    }
}