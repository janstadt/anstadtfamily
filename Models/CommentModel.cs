using photoshare.Models.Enums;
using System;
namespace photoshare.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Guid Owner { get; set; }
    }
}