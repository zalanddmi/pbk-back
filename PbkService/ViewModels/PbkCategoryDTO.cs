using PbkService.Auxiliaries;

namespace PbkService.ViewModels
{
    public record PbkCategoryDTO(string Name, List<DisplayModel<string>> Mccs, int Id = 0)
    {
    }
}
