namespace PbkService.Auxiliaries
{
    public class DisplayModel<TId>
    {
        public required TId Id { get; set; }
        public required string DisplayName { get; set; }
    }
}
