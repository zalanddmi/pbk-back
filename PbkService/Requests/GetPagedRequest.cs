namespace PbkService.Requests
{
    public record GetPagedRequest(int PageNumber = 1, int PageSize = 10, string? SearchString = null)
    {
    }
}
