using BugTrack.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace BugTrack.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime TimePosted { get; set; }
        public string OwnerName { get; set; }

        public BugUser? BugUser { get; set; }
        public string? BugUserId { get; set; }

        public int IssueReportEntityId { get; set; }
        public IssueReportEntity IssueReportEntity { get; set; }

        public Comment() { }
        public Comment(string content, DateTime timePosted)
        {
            Content = content;
            TimePosted = timePosted;
        }
    }
}
