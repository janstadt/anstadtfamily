using photoshare.Models.Enums;
using System;
using System.Collections.Generic;
namespace photoshare.Models
{
    public class PhotoModel
    {
        public string Id { get; set; }
        public Guid AlbumId { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public Guid Owner { get; set; }
        public bool MainImage { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<TagModel> Tags { get; set; }
        public bool Watermark { get; set; }
        public bool FileExists { get; set; }
        public bool Favorite { get; set; }
    }

    public class PhotoEntity
    {
        public Guid Id { get; set; }
        public Guid AlbumId { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public Guid Owner { get; set; }
        public bool MainImage { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<TagModel> Tags { get; set; }
        public bool Favorite { get; set; }
    }
}