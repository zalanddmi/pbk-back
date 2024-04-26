using System.ComponentModel.DataAnnotations;

namespace PbkService.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }

        public virtual ICollection<Outlet>? Outlets { get; set; }
    }
}
