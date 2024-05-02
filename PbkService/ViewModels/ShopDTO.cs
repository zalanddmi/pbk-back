using PbkService.Auxiliaries;

namespace PbkService.ViewModels
{
    public record ShopDTO(string Name, List<DisplayModel<int>>? Outlets, int Id = 0)
    {
    }
}
