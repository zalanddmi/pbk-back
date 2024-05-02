namespace PbkService.Auxiliaries
{
    public class DisplayModel<TId>
    {
        public required TId Id { get; set; }
        public string? DisplayName { get; set; }
    }
}
