using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PbkService.Models
{
    public class Cashback
    {
        [Key]
        public int Id { get; set; }
        public required int CardId { get; set; }
        [ForeignKey(nameof(CardId))]
        public virtual Card Card { get; set; }
        public required int PbkCategoryId { get; set; }
        [ForeignKey(nameof(PbkCategoryId))]
        public virtual PbkCategory PbkCategory { get; set; }
        [Precision(4, 2)]
        public required decimal Percent { get; set; }
    }
}
