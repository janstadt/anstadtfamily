
using photoshare.Models.Enums;
using System;
using System.Collections.Generic;
namespace photoshare.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }
        public LoginStatus LoginStatus { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class LoginHeaderModel : LoginModel
    {
        public List<TagModel> Categories { get; set; }
    }
}