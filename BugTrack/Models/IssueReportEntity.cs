using BugTrack.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using BugTrack.ViewModels.VMIssueReportEntities;

namespace BugTrack.Models
{
    public class IssueReportEntity
    {
        [Column("IssueId")]
        public int Id { get; set; }
        public string IssueTitle { get; set; }
        public int ThreatLevel { get; set; }
        public string GeneralDescription { get; set; }
        public string ReplicationDescription { get; set; }
        public DateTime DateFound { get; set; }

        public BugUser? BugUser { get; set; }
        public string? BugUserId { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public IssueReportEntity() { }

        public IssueReportEntityWithIdViewModel ConvertToIssueReportEntityWithIdVM()
        {
            IssueReportEntityWithIdViewModel issueReportEntityWithIdVM = new IssueReportEntityWithIdViewModel();
            issueReportEntityWithIdVM.IssueTitle = this.IssueTitle;
            issueReportEntityWithIdVM.ThreatLevel = this.ThreatLevel;
            issueReportEntityWithIdVM.GeneralDescription = this.GeneralDescription;
            issueReportEntityWithIdVM.ReplicationDescription = this.ReplicationDescription;
            issueReportEntityWithIdVM.DateFound = this.DateFound;
            issueReportEntityWithIdVM.Id = this.Id;

            return issueReportEntityWithIdVM;
        }

    }
}
