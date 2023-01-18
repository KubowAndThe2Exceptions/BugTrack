using BugTrack.Areas.Identity.Data;
using BugTrack.Data;
using BugTrack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BugTrackTests.Data
{
    public class TestDatabaseFixture : IDisposable
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=TestDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        context.Database.OpenConnection();
                        
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Comments OFF");
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.IssueReport OFF");

                        context.Add(
                            new BugUser { FirstName = "John", LastName = "Doe", Email = "something@gmail.com", Id = "TESTID-T1" });
                        context.Add(new BugUser { FirstName = "Rebecca", LastName = "Cornwheel", Email = "somethingelse@gmail.com", Id = "TESTID-T2" });
                        context.Add(new BugUser { FirstName = "Bob", LastName = "Lehman", Email = "Bobby@gmail.com", Id = "TESTID-T3" });

                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.IssueReport ON");
                        context.Add(
                            new IssueReportEntity
                            {
                                Id = 1,
                                BugUserId = "TESTID-T1",
                                DateFound = DateTime.Now,
                                IssueTitle = "Test-Title 1",
                                GeneralDescription = "Foo",
                                ReplicationDescription = "Bar",
                                ThreatLevel = 2
                            });
                        context.Add(new IssueReportEntity
                        {
                            Id = 2,
                            BugUserId = "TESTID-T2",
                            DateFound = DateTime.Now,
                            IssueTitle = "Test-Title 2",
                            GeneralDescription = "Fus",
                            ReplicationDescription = "Roh Dah",
                            ThreatLevel = 4
                        });
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.IssueReport OFF");
                        
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Comments ON");
                        context.Add(
                            new Comment { BugUserId = "TESTID-T1", Content = "Test content 1", Id = 1, IssueReportEntityId = 1, TimePosted = DateTime.Now, OwnerName = "John Doe" });
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Comments OFF");

                    }

                    _databaseInitialized = true;
                }
            }
        }

        public ApplicationDbContext CreateContext()
            => new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);

        public void Dispose()
        {

        }

    }

}
