using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WEBTRUYEN.Models
{
    public class User : IdentityUser
    {
        public List<Comic>? Comics { get; set; }

        public List<ReadingHistory>? ReadingHistories { get; set; }
    }
}