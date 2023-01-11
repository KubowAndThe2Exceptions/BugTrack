using BugTrack.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

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

        public BugUser BugUser { get; set; }
        public string BugUserId { get; set; }

    }
}
