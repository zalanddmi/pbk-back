using PbkService.Auxiliaries;

namespace PbkService.ViewModels
{
    public record OperationDTO
    {
        public int Id { get; init; } = 0;
        public required DisplayModel<int> Outlet { get; init; }
        public required decimal Sum { get; init; }
    }
}
