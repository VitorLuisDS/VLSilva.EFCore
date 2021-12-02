using Microsoft.EntityFrameworkCore;

namespace VLSilva.EFCore.Extentions.Tests.Fakes
{
    internal partial class FakeDbContext : DbContext
    {
        public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        private static DbContextOptions<FakeDbContext> GetInMemoryOptions()
        {
            DbContextOptions<FakeDbContext> options = new DbContextOptionsBuilder<FakeDbContext>()
                   .UseInMemoryDatabase("VLSilva.EFCore.Extentions.Fakes")
                   .Options;

            return options;
        }

        public static FakeDbContext GetFakeDbContext()
        {
            FakeDbContext fakeDbContext = new FakeDbContext(GetInMemoryOptions());
            fakeDbContext.Database.EnsureCreated();

            return fakeDbContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>(userBuilder =>
                {
                    userBuilder
                        .HasKey(user => user.Id);

                    userBuilder
                        .Property(user => user.Username)
                        .IsRequired(false)
                        .HasMaxLength(255);

                    userBuilder
                        .Property(user => user.CreationDate)
                        .IsRequired();

                    userBuilder
                        .Property(user => user.Active)
                        .IsRequired();
                });

            modelBuilder.SeedData();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
