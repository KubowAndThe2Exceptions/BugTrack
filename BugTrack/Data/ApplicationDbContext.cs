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
            builder.Entity<Comment>()
                .HasOne(p => p.IssueReportEntity)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.IssueReportEntityId);
            builder.Entity<Comment>()
                .HasOne(p => p.BugUser)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.BugUserId);
            builder.Entity<BugUser>()
                .HasOne(p => p.Profile)
                .WithOne(p => p.BugUser)
                .HasForeignKey<Profile>(p => p.BugUserId);
                
        }

        public DbSet<IssueReportEntity> IssueReport { get; set; }
        public DbSet<BugUser> BugUser { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Profile> Profiles { get; set; }
    }
}
