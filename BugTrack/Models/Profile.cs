using BugTrack.Areas.Identity.Data;
using BugTrack.ViewModels.VMProfiles;
using System.ComponentModel.DataAnnotations;

namespace BugTrack.Models
{
    public class Profile
    {
        public int Id { get; set; }
        
        public string OwnerName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string UserJobTitle { get; set; }
        
        public BugUser? BugUser { get; set; }
        public string? BugUserId { get; set; }

        public Profile() { }

        public Profile(string firstName, string lastName, string email, string jobTitle)
        {
            Email = email;
            OwnerName = firstName + " " + lastName;
            UserJobTitle = jobTitle;
        }

        public ProfileViewModel ConvertToProfileVM()
        {
            var profileVM = new ProfileViewModel();
            profileVM.Id = Id;
            profileVM.OwnerName = OwnerName;
            profileVM.Email = Email;
            profileVM.UserJobTitle = UserJobTitle;

            if (this.BugUser != null && this.BugUser.IssueReportEntities != null)
            {
                foreach (var issue in this.BugUser.IssueReportEntities)
                {
                    var convertedIssue = issue.ConvertToIssueReportEntityWithIdVM();
                    profileVM.IssueReportVMs.Add(convertedIssue);
                }
            }
            return profileVM;
        }
    }
}
