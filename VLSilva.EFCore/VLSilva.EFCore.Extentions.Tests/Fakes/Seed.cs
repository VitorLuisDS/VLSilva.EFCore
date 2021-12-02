using Microsoft.EntityFrameworkCore;
using System;

namespace VLSilva.EFCore.Extentions.Tests.Fakes
{
    internal static class Seed
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(
                new User[]
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testActiveUser",
                        Active = true,
                        CreationDate = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testActiveUser2",
                        Active = true,
                        CreationDate = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testInactiveUser",
                        Active = false,
                        CreationDate = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testInactiveUser2",
                        Active = false,
                        CreationDate = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testActiveUserCreatedYesterday",
                        Active = true,
                        CreationDate = DateTime.UtcNow.AddDays(-1)
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testActiveUserCreatedYesterday2",
                        Active = true,
                        CreationDate = DateTime.UtcNow.AddDays(-1)
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testInactiveUserCreatedYesterday",
                        Active = false,
                        CreationDate = DateTime.UtcNow.AddDays(-1)
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testInactiveUserCreatedYesterday2",
                        Active = false,
                        CreationDate = DateTime.UtcNow.AddDays(-1)
                    },
                });
        }
    }
}
