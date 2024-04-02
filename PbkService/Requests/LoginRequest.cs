using PbkService.Requests.Enums;

namespace PbkService.Requests
{
    public class LoginRequest
    {
        public required LoginTypes LoginType { get; set; } = LoginTypes.Username;
        public required string Value { get; set; }
        public required string Password { get; set; }
    }
}
