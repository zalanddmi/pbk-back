using PbkService.Auxiliaries;

namespace PbkService.ViewModels.Cards
{
    public record CardDTO()
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public DisplayModel<int> Bank { get; init; }
        public DisplayModel<int> TypeCard { get; init; }
        public List<CardCashbackDTO> Cashbacks { get; init; }
    }
}
