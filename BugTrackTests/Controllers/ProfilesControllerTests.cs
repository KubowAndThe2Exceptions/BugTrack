using BugTrack.Data;
using Microsoft.AspNetCore.Identity;
using BugTrack.Areas.Identity.Data;
using Xunit;
using BugTrackTests.Data;
using BugTrack.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BugTrack.ViewModels.VMProfiles;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BugTrackTests
{
    public class ProfilesControllerTests : IClassFixture<TestDatabaseFixture>
    {
        public ProfilesControllerTests(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        public TestDatabaseFixture Fixture { get; }

        [Fact]
        public async Task Index_ReturnsListofProfilesAsProfileVMs()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();

            var controller = new ProfilesController(context, uManager.Object);

            //Act
            var result = await controller.Index("");

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ProfileViewModel>>(viewResult.ViewData.Model);
            Assert.NotEmpty(model);
        }

        [Fact]
        public async Task Index_ReturnsListofProfiles_BasedOnSearch()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();

            var controller = new ProfilesController(context, uManager.Object);

            //Act
            var result = await controller.Index("John");

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ProfileViewModel>>(viewResult.ViewData.Model);
            Assert.NotEmpty(model);
            Assert.Contains("John", model[0].OwnerName);
        }

        [Fact]
        public async Task Details_ReturnsViewResultWithIssues()
        {
            //Arrange
            var store = new Mock<IUserStore<BugUser>>();
            var uManager = new Mock<UserManager<BugUser>>(store.Object, null, null, null, null, null, null, null, null);

            using var context = Fixture.CreateContext();

            var controller = new ProfilesController(context, uManager.Object);

            //Act
            var result = await controller.Details(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProfileViewModel>(viewResult.ViewData.Model);
            Assert.NotEmpty(model.IssueReportVMs);
        }

        [Fact]
        public async Task MyProfile_ReturnsViewResultWithIssues()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            await context.Database.BeginTransactionAsync();
            var bugUser = new BugUser { Id = "TESTID-T1"};
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

            var controller = new ProfilesController(context, uManager.Object);
            controller.ControllerContext = controllerContext;

            //Act
            var result = await controller.MyProfile();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProfileWithIFormFileViewModel>(viewResult.ViewData.Model);
            Assert.NotEmpty(model.IssueReportVMs);
        }

        [Fact]
        public async Task MyProfilePost_FileIsUploaded()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            await context.Database.BeginTransactionAsync();
            var bugUser = new BugUser { Id = "TESTID-T1" };
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

            var controller = new ProfilesController(context, uManager.Object);
            controller.ControllerContext = controllerContext;

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string filePath = projectDirectory + @"\Data\A-Cat.jpg";

            var stream = File.OpenRead(filePath);
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            var profile = new ProfileWithIFormFileViewModel { AvatarFile = file, Id = 1 };

            //Act
            var result = await controller.MyProfile(1, profile);

            //Assert
            context.ChangeTracker.Clear();
            var dbProfile = context.Profiles.Where(p => p.Id == profile.Id).FirstOrDefault();
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(dbProfile.Avatar);
        }
    }
}
