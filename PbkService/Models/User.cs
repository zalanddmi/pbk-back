using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PbkService.Models
{
    [Index("Username", IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Salt { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
