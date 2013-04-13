using photoshare.Models.Enums;
using System;
namespace photoshare.Models
{
    public class FavoritePhotoModel
    {
        public Guid Id { get; set; }
        public Guid PhotoId { get; set; }
        public DateTime Date { get; set; }
    }

    public class FavoritePhotoEntity
    {
        public Guid Id { get; set; }
        public Guid PhotoId { get; set; }
        public DateTime Date { get; set; }
    }

    public class FavoriteAlbumModel
    {
        public Guid Id { get; set; }
        public Guid AlbumId { get; set; }
        public DateTime Date { get; set; }
    }
}