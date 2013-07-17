using photoshare.Models.Enums;
using System;
using System.Collections.Generic;
namespace photoshare.Models
{
    public class EmailEntity
    {
       
        public List<string> EmailTo { get; set; }
        public List<string> EmailFrom { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}