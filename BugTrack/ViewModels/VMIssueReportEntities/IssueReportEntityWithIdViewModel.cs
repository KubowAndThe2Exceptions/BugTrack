using BugTrack.Areas.Identity.Data;
using BugTrack.Models;
using BugTrack.ViewModels.VMIssueReportEntities.Helpers;

namespace BugTrack.ViewModels.VMIssueReportEntities
{
    public class IssueReportEntityWithIdViewModel : IssueReportEntityViewModel
    {
        public int Id { get; set;}

        public override IssueReportEntity ConvertToIssueReportEntity(BugUser user)
        {
            IssueReportEntity issueReportEntity = new IssueReportEntity();
            issueReportEntity.IssueTitle = this.IssueTitle;
            issueReportEntity.IssueThreatId = this.IssueThreat.Id;
            issueReportEntity.GeneralDescription = this.GeneralDescription;
            issueReportEntity.ReplicationDescription = this.ReplicationDescription;
            issueReportEntity.DateFound = this.DateFound;
            issueReportEntity.BugUser = user;
            issueReportEntity.IssueStatusId = issueReportEntity.IssueStatusId;
            issueReportEntity.ModuleOrClass = issueReportEntity.ModuleOrClass;
            issueReportEntity.Id = this.Id;

            return issueReportEntity;
        }
    }
}
