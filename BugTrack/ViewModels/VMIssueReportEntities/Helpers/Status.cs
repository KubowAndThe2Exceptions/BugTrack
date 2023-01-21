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
        public static Status ProcessId(int id)
        {
            switch (id)
            {
                case 1:
                    {
                        return new Status(id, "Waiting");
                    }
                case 2:
                    {
                        return new Status(id, "Assigned");
                    }
                case 3:
                    {
                        return new Status(id, "Fixed");
                    }
                default:
                    {
                        return new Status(0, "Error");
                    }
            }
        }
    }
}
