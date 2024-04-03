﻿namespace PbkService.Requests
{
    public class RegisterRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
