using BugTrack.Areas.Identity.Data;
using BugTrack.Models;
using BugTrack.ViewModels.VMComments;

namespace BugTrack.ViewModels.VMIssueReportEntities
{
    public class IssueReportEntityViewModel
    {
        public string IssueTitle { get; set; }
        public int ThreatLevel { get; set; }
        public string GeneralDescription { get; set; }
        public string ReplicationDescription { get; set; }
        public DateTime DateFound { get; set; }
        public List<CommentViewModel> Comments { get; set; }

        public IssueReportEntityViewModel()
        {
            Comments = new List<CommentViewModel>();
        }



        public virtual IssueReportEntity ConvertToIssueReportEntity(BugUser user)
        {
            IssueReportEntity issueReportEntity = new IssueReportEntity();
            issueReportEntity.IssueTitle = IssueTitle;
            issueReportEntity.ThreatLevel = ThreatLevel;
            issueReportEntity.GeneralDescription = GeneralDescription;
            issueReportEntity.ReplicationDescription = ReplicationDescription;
            issueReportEntity.DateFound = DateFound;
            issueReportEntity.BugUser = user;

            return issueReportEntity;
        }
    }
}
