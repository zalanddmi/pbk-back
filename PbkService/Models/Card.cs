using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PbkService.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public required int BankId { get; set; }
        [ForeignKey(nameof(BankId))]
        public virtual Bank Bank { get; set; }
        public required int TypeCardId { get; set; }
        [ForeignKey(nameof(TypeCardId))]
        public virtual TypeCard TypeCard { get; set; }

        public virtual ICollection<Cashback>? Cashbacks { get; set; }
    }
}
