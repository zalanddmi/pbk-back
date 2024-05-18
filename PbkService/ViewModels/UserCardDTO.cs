using PbkService.Auxiliaries;

namespace PbkService.ViewModels
{
    public record UserCardDTO
    {
        public required DisplayModel<int> Card { get; init; }
        public required DisplayModel<int> Bank { get; init; }
        public required DisplayModel<int> TypeCard { get; init; }
    }
}
