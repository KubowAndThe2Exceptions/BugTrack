namespace BugTrack.ViewModels.VMIssueReportEntities.Helpers
{
    public class ThreatLevel
    {
        public int Id { get; private set; }
        public string Description { get; private set; }

        private ThreatLevel(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public static List<ThreatLevel> GetAll()
        {
            var statuses = new List<ThreatLevel>
            {
                new ThreatLevel(1, "Small Detail"),
                new ThreatLevel(2, "Minor"),
                new ThreatLevel(3, "Okay"),
                new ThreatLevel(4, "Major"),
                new ThreatLevel(5, "Fix Immediately"),
            };
            return statuses;
        }
    }
}
