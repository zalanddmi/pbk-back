using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PbkService.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public required string Username { get; set; }
        [MaxLength(255)]
        public required string PasswordHash { get; set; }
        [MaxLength(255)]
        public required string Salt { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength(100)]
        public required string Role { get; set; }
    }
}
