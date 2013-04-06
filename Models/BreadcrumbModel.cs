using photoshare.Models.Enums;
using System;
namespace photoshare.Models
{
    public class BreadcrumbModel
    {
        public string Url { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool Active { get; set; }
    }
}