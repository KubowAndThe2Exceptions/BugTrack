using BugTrack.Areas.Identity.Data;
using BugTrack.Models;

namespace BugTrack.ViewModels.VMIssueReportEntities
{
    public class IssueReportEntityWithIdViewModel : IssueReportEntityViewModel
    {
        public int Id { get; set;}

        public override IssueReportEntity ConvertToIssueReportEntity(BugUser user)
        {
            IssueReportEntity issueReportEntity = new IssueReportEntity();
            issueReportEntity.IssueTitle = this.IssueTitle;
            issueReportEntity.ThreatLevel = this.ThreatLevel;
            issueReportEntity.GeneralDescription = this.GeneralDescription;
            issueReportEntity.ReplicationDescription = this.ReplicationDescription;
            issueReportEntity.DateFound = this.DateFound;
            issueReportEntity.BugUser = user;
            issueReportEntity.Status = issueReportEntity.Status;
            issueReportEntity.ModuleOrClass = issueReportEntity.ModuleOrClass;
            issueReportEntity.Id = this.Id;

            return issueReportEntity;
        }
    }
}
