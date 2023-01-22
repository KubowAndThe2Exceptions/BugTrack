using BugTrack.Areas.Identity.Data;
using BugTrack.Models;
using BugTrack.ViewModels.VMComments;
using BugTrack.ViewModels.VMIssueReportEntities.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTrack.ViewModels.VMIssueReportEntities
{
    public class IssueReportEntityViewModel
    {
        public string IssueTitle { get; set; }
        public string BugUserId { get; set; }
        public string GeneralDescription { get; set; }
        public string ReplicationDescription { get; set; }
        public DateTime DateFound { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public Status IssueStatus { get; set; }
        public ThreatLevel IssueThreat { get; set; }
        public string ModuleOrClass { get; set; }
        public string? AvatarEncoded { get; set; }
        public string? Owner { get; set; }
        public IssueReportEntityViewModel()
        {
            Comments = new List<CommentViewModel>();
        }
        
        public virtual IssueReportEntity ConvertToIssueReportEntity(BugUser user)
        {
            IssueReportEntity issueReportEntity = new IssueReportEntity();
            issueReportEntity.IssueTitle = IssueTitle;
            issueReportEntity.IssueThreatId = IssueThreat.Id;
            issueReportEntity.GeneralDescription = GeneralDescription;
            issueReportEntity.ReplicationDescription = ReplicationDescription;
            issueReportEntity.DateFound = DateFound;
            
            //Probably shouldnt have user on this class
            issueReportEntity.BugUser = user;
            issueReportEntity.IssueStatusId = IssueStatus.Id;
            issueReportEntity.ModuleOrClass = issueReportEntity.ModuleOrClass;

            return issueReportEntity;
        }
    }
}
