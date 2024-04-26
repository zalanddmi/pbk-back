using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PbkService.Models
{
    [PrimaryKey(nameof(MccCode), nameof(PbkCategoryId))]
    public class MccPbkCategory
    {
        public required string MccCode { get; set; }
        public required int PbkCategoryId { get; set; }
        [ForeignKey(nameof(MccCode))]
        public virtual Mcc Mcc { get; set; }
        [ForeignKey(nameof(PbkCategoryId))]
        public virtual PbkCategory PbkCategory { get; set; }
    }
}
