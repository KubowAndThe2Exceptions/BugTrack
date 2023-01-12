using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BugTrack.Models;

namespace BugTrack.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BugUser class
public class BugUser : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }
    
    public ICollection<IssueReportEntity> IssueReportEntities { get; set; }
    //Likely will not be used quite yet.  Still thinking about how I want to implement comments in the future
    public ICollection<Comment> Comments { get; set; }

    public Profile Profile { get; set; }

    public BugUser() { }
}

