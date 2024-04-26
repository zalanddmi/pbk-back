using PbkService.Auxiliaries;

namespace PbkService.ViewModels
{
    public record OutletDTO(string Name, DisplayModel<int> Shop, DisplayModel<string> Mcc, int Id = 0)
    {
    }
}
