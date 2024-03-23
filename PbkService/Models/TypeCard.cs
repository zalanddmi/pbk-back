using System.ComponentModel.DataAnnotations;

namespace PbkService.Models
{
    public class TypeCard
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}
