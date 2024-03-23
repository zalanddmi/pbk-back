using System.ComponentModel.DataAnnotations;

namespace PbkService.Models
{
    public class Bank
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }

        public virtual ICollection<Card>? Cards { get; set; }
    }
}
