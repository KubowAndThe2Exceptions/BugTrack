using BugTrack.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace BugTrack.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        
        public BugUser? BugUser { get; set; }
        public string? BugUserId { get; set; }

        public Profile() { }
    }
}
