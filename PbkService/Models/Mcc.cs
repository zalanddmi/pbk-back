using System.ComponentModel.DataAnnotations;

namespace PbkService.Models
{
    public class Mcc
    {
        [Key]
        [MaxLength(4)]
        public required string Code { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<MccPbkCategory>? MccPbkCategories { get; set; }
    }
}
