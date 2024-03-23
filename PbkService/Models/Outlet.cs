using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PbkService.Models
{
    public class Outlet
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public required int ShopId { get; set; }
        [ForeignKey(nameof(ShopId))]
        public required Shop Shop { get; set; }
        public required string MccCode { get; set; }
        [ForeignKey(nameof(MccCode))]
        public required Mcc Mcc { get; set; }
    }
}
