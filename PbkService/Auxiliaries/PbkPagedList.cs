namespace PbkService.Auxiliaries
{
    public class PbkPagedList<T> where T : class
    {
        public required int PageNumber { get; init; }
        public required int PageSize { get; init; }
        public required int PageCount { get; init; }
        public required int TotalCount { get; init; }
        public required List<T> Items { get; init; }
    }
}
