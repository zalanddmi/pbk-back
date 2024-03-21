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
        public required Mcc Mcc { get; set; }
        [ForeignKey(nameof(PbkCategoryId))]
        public required PbkCategory PbkCategory { get; set; }
    }
}
