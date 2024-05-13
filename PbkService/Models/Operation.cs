using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PbkService.Models
{
    public class Operation
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual required User User { get; set; }
        public int OutletId { get; set; }
        [ForeignKey(nameof(OutletId))]
        public virtual required Outlet Outlet { get; set; }
        [Precision(10, 2)]
        public required decimal Sum { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
