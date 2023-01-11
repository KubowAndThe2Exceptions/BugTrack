using BugTrack.Areas.Identity.Data;
using BugTrack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IssueReportEntity>()
                .HasOne(p => p.BugUser)
                .WithMany(p => p.IssueReportEntities)
                .HasForeignKey(p => p.BugUserId);
        }

        public DbSet<IssueReportEntity> IssueReport { get; set; }
        public DbSet<BugUser> BugUser { get; set; }
    }
}
