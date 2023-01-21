using BugTrack.Areas.Identity.Data;
using BugTrack.Models;
using BugTrack.ViewModels.VMComments;
using BugTrack.ViewModels.VMIssueReportEntities.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTrack.ViewModels.VMIssueReportEntities
{
    public class IssueReportEntityEditCreateVM
    {
        public int Id { get; set; }
        public string IssueTitle { get; set; }
        public string GeneralDescription { get; set; }
        public string ReplicationDescription { get; set; }
        public DateTime DateFound { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public string IssueStatus { get; set; }
        public string IssueThreat { get; set; }
        public List<SelectListItem> StatusesSelectList { get; set; }
        public List<SelectListItem> ThreatLevelsSelectList { get; set; }
        public string ModuleOrClass { get; set; }


        public IssueReportEntityEditCreateVM()
        {
            Comments = new List<CommentViewModel>();
            ThreatLevelsSelectList = new List<SelectListItem>();
            var threats = ThreatLevel.GetAll();
            foreach (var threat in threats)
            {
                ThreatLevelsSelectList.Add(
                    new SelectListItem(threat.Description, threat.Id.ToString()));
            }
            StatusesSelectList = new List<SelectListItem>();
            var statuses = Status.GetAll();
            foreach (var status in statuses)
            {
                StatusesSelectList.Add(
                    new SelectListItem(status.Description, status.Id.ToString()));
            }
        }
        public IssueReportEntity ConvertToIssueReportEntity(BugUser user)
        {
            IssueReportEntity issueReportEntity = new IssueReportEntity();
            issueReportEntity.Id = Id;
            issueReportEntity.IssueTitle = IssueTitle;
            issueReportEntity.IssueThreatId = Convert.ToInt32(IssueThreat);
            issueReportEntity.GeneralDescription = GeneralDescription;
            issueReportEntity.ReplicationDescription = ReplicationDescription;
            issueReportEntity.DateFound = DateFound;

            //Probably shouldnt have user on this class
            issueReportEntity.BugUser = user;
            issueReportEntity.IssueStatusId = Convert.ToInt32(IssueStatus);
            issueReportEntity.ModuleOrClass = issueReportEntity.ModuleOrClass;

            return issueReportEntity;
        }
    }
}
