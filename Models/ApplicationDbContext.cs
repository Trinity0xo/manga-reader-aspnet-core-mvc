using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WEBTRUYEN.Models
{
    public class ApplicationDbContext : IdentityDbContext<NguoiDung>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        // th�m DbSet
        public DbSet<Truyen> Truyens { get; set; }
        public DbSet<TheLoai> TheLoais { get; set; }
    }
}