namespace BugTrack.ViewModels.VMIssueReportEntities.Helpers
{
    public class Status
    {
        public int Id { get; private set; }
        public string Description { get; private set; }

        private Status(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public static List<Status> GetAll()
        {
            var statuses = new List<Status>
            {
                new Status(1, "Waiting"),
                new Status(2, "Assigned"),
                new Status(3, "Fixed"),
            };
            return statuses;
        }
    }
}
