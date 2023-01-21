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
        public static ThreatLevel ProcessId(int id)
        {
            switch (id)
            {
                case 1:
                    {
                        return new ThreatLevel(id, "Small Detail");
                    }
                case 2:
                    {
                        return new ThreatLevel(id, "Minor");
                    }
                case 3:
                    {
                        return new ThreatLevel(id, "Okay");
                    }
                case 4:
                    {
                        return new ThreatLevel(id, "Major");
                    }
                case 5:
                    {
                        return new ThreatLevel(id, "Fix Immediately");
                    }
                default:
                    {
                        return new ThreatLevel(0, "Error");
                    }
            }
        }
    }
}
