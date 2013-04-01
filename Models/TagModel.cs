using photoshare.Models.Enums;
using System;
namespace photoshare.Models
{
    public class TagModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Guid Owner { get; set; }
        public Guid ParentId { get; set; }
        public TagType Type { get; set; }
        public bool IsCategory { get; set; }
    }

    public class TagEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Guid Owner { get; set; }
        public Guid ParentId { get; set; }
        public string Type { get; set; }
        public bool IsCategory { get; set; }
    }
}