using photoshare.Models.Enums;
using System;
using System.Collections.Generic;
namespace photoshare.Models
{
    public class ContactUsModel
    {
        public static string DEFAULT_TO = "kari@my2peasphotography.com";
        public static string DEFAULT_SUBJECT = "Inquiry";
        public string Email { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}