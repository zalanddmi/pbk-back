using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PbkService.Models
{
    [PrimaryKey(nameof(UserId), nameof(CardId))]
    public class UserCard
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public required virtual User User { get; set; }
        public int CardId { get; set; }
        [ForeignKey(nameof(CardId))]
        public required virtual Card Card { get; set; }
    }
}
