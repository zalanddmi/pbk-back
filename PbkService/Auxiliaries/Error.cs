namespace PbkService.Auxiliaries
{
    public class Error
    {
        public string Code { get; set; } = "ServerError";
        public required string Message { get; set; }
    }
}
