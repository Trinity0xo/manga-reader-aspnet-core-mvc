using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MangaReader.Models
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string,
            IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        }

        public DbSet<Manga> Mangas { get; set; }

        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Following> Followings { get; set; }

        public DbSet<ReadingHistory> ReadingHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(ur =>
            {
                ur.HasKey(x => new { x.UserId, x.RoleId });

                ur.HasOne(x => x.User)
                  .WithMany(u => u.UserRoles)
                  .HasForeignKey(x => x.UserId)
                  .IsRequired();

                ur.HasOne(x => x.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(x => x.RoleId)
                  .IsRequired();
            });

            builder.Entity<Following>(fo =>
            {
                fo.HasKey(x => new { x.UserId, x.MangaId });

                fo.HasOne(x => x.User)
                    .WithMany(u => u.Followings)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired();

                fo.HasOne(x => x.Manga)
                    .WithMany(m => m.Followings)
                    .HasForeignKey(x => x.MangaId)
                    .IsRequired();
            });

            builder.Entity<ReadingHistory>(rh =>
            {
                rh.HasKey(x => new { x.UserId, x.MangaId });

                rh.HasOne(x => x.User)
                    .WithMany(u => u.ReadingHistories)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired();

                rh.HasOne(x => x.Manga)
                    .WithMany(m => m.ReadingHistories)
                    .HasForeignKey(x => x.MangaId)
                    .IsRequired();

                rh.HasOne(x => x.Chapter)
                    .WithMany()
                    .HasForeignKey(x => x.ChapterId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });
        }
    }
}
