using Microsoft.AspNetCore.Identity;

namespace MangaReader.Models
{
    public class UserRole: IdentityUserRole<string>
    {
        public virtual Role Role { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}
