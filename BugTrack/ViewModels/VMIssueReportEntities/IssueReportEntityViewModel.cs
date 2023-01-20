using BugTrack.Areas.Identity.Data;
using BugTrack.Models;
using BugTrack.ViewModels.VMComments;
using BugTrack.ViewModels.VMIssueReportEntities.Helpers;

namespace BugTrack.ViewModels.VMIssueReportEntities
{
    public class IssueReportEntityViewModel
    {
        public string IssueTitle { get; set; }
        public string GeneralDescription { get; set; }
        public string ReplicationDescription { get; set; }
        public DateTime DateFound { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public Status Status { get; set; }
        public ThreatLevel ThreatLevel { get; set; }
        public List<Status> Statuses { get; set; }
        public List<ThreatLevel> ThreatLevels { get; set; }
        public string ModuleOrClass { get; set; }


        public IssueReportEntityViewModel()
        {
            Comments = new List<CommentViewModel>();
            ThreatLevels = ThreatLevel.GetAll();
            Statuses = Status.GetAll();
        }



        public virtual IssueReportEntity ConvertToIssueReportEntity(BugUser user)
        {
            IssueReportEntity issueReportEntity = new IssueReportEntity();
            issueReportEntity.IssueTitle = IssueTitle;
            //issueReportEntity.ThreatLevel = ThreatLevel;
            issueReportEntity.GeneralDescription = GeneralDescription;
            issueReportEntity.ReplicationDescription = ReplicationDescription;
            issueReportEntity.DateFound = DateFound;
            
            //Probably shouldnt have user on this class
            issueReportEntity.BugUser = user;
            issueReportEntity.Status = issueReportEntity.Status;
            issueReportEntity.ModuleOrClass = issueReportEntity.ModuleOrClass;

            return issueReportEntity;
        }
    }
}
