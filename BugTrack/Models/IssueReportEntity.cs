using BugTrack.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using BugTrack.ViewModels.VMIssueReportEntities;
using BugTrack.ViewModels.VMComments;
using BugTrack.ViewModels.VMIssueReportEntities.Helpers;

namespace BugTrack.Models
{
    public class IssueReportEntity
    {
        [Column("IssueId")]
        public int Id { get; set; }
        public string IssueTitle { get; set; }
        public int IssueThreatId { get; set; }
        public string GeneralDescription { get; set; }
        public string ReplicationDescription { get; set; }
        public DateTime DateFound { get; set; }

        public int IssueStatusId { get; set; }
        public string? ModuleOrClass { get; set; }

        public BugUser? BugUser { get; set; }
        public string? BugUserId { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public IssueReportEntity() { }

        public IssueReportEntityWithIdViewModel ConvertToIssueReportEntityWithIdVM()
        {
            IssueReportEntityWithIdViewModel issueReportEntityWithIdVM = new IssueReportEntityWithIdViewModel();
            issueReportEntityWithIdVM.IssueTitle = this.IssueTitle;
            issueReportEntityWithIdVM.IssueThreat = ThreatLevel.ProcessId(this.IssueThreatId);
            issueReportEntityWithIdVM.GeneralDescription = this.GeneralDescription;
            issueReportEntityWithIdVM.ReplicationDescription = this.ReplicationDescription;
            issueReportEntityWithIdVM.DateFound = this.DateFound;
            issueReportEntityWithIdVM.Id = this.Id;
            issueReportEntityWithIdVM.IssueStatus = Status.ProcessId(this.IssueStatusId);
            issueReportEntityWithIdVM.ModuleOrClass = this.ModuleOrClass;

            if (Comments != null)
            {
                foreach (var comment in Comments)
                {
                    //var commentViewModel = new CommentViewModel();
                    //commentViewModel.TimePosted = comment.TimePosted;
                    //commentViewModel.CommentId = comment.Id;
                    //commentViewModel.OwnerName = comment.OwnerName;
                    //commentViewModel.BugUserId = comment.BugUserId;
                    //commentViewModel.Content = comment.Content;
                    issueReportEntityWithIdVM.Comments.Add(CommentViewModel.CommentToVM(comment));
                }
            }

            return issueReportEntityWithIdVM;
        }

    }
}
