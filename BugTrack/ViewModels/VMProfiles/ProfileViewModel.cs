using BugTrack.Models;
using BugTrack.ViewModels.VMIssueReportEntities;
using System.ComponentModel.DataAnnotations;

namespace BugTrack.ViewModels.VMProfiles
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string UserJobTitle { get; set; }
        
        public string AvatarEncoded { get; set; }
        
        public List<IssueReportEntityWithIdViewModel> IssueReportVMs = new List<IssueReportEntityWithIdViewModel>();

        public ProfileViewModel() { }
    }
}
