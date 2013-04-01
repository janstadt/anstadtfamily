
using photoshare.Models.Enums;
using System;
namespace photoshare.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid? Id { get; set; }
        public LoginStatus LoginStatus { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string ReturnUrl { get; set; }
    }
}