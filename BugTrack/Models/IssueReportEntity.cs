namespace BugTrack.Models
{
    public class IssueReportEntity
    {
        public int Id { get; set; }
        public string IssueTitle { get; set; }
        public int ThreatLevel { get; set; }
        public string GeneralDescription { get; set; }
        public string ReplicationDescription { get; set; }
        public DateTime DateFound { get; set; }

    }
}
