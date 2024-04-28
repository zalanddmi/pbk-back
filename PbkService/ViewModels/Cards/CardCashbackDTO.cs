using PbkService.Auxiliaries;

namespace PbkService.ViewModels.Cards
{
    public class CardCashbackDTO
    {
        public int Id { get; set; }
        public DisplayModel<int> Category { get; set; }
        public decimal Percent { get; set; }
    }
}
