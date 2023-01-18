using BugTrack.Data;
using Microsoft.AspNetCore.Identity;
using BugTrack.Areas.Identity.Data;
using Xunit;
using BugTrackTests.Data;
using BugTrack.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BugTrack.ViewModels.VMProfiles;

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
    }
}
