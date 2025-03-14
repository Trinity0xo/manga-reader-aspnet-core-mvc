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
    }
}