using photoshare.Models.Enums;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace photoshare.Models
{
    public class PhotoAlbumBaseModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public Guid Owner { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool Favorite { get; set; }
    }
    public class PhotoAlbumModel : PhotoAlbumBaseModel
    {
        public List<PhotoModel> Photos { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<string> AddedTags { get; set; }
        public string Clean { get; set; }
    }

    public class PhotoAlbumEntity : PhotoAlbumBaseModel
    {
       public List<PhotoEntity> Photos { get; set; }
       public new Guid Id { get; set; }
    }
}