using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WEBTRUYEN.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        // th�m DbSet
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Comic> Comics { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<ReadingHistory> ReadingHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReadingHistory>()
                .HasKey(uc => new { uc.UsersId, uc.ChaptersId });

            modelBuilder.Entity<ReadingHistory>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.ReadingHistories)
                .HasForeignKey(uc => uc.UsersId);

            modelBuilder.Entity<ReadingHistory>()
                .HasOne(uc => uc.Chapter)
                .WithMany(c => c.ReadingHistories)
                .HasForeignKey(uc => uc.ChaptersId);
        }
    }
}