using photoshare.Models.Enums;
using System;
using System.Collections.Generic;
namespace photoshare.Models
{
    public class UserModel : UserPhotoAlbumsModel
    {
        public int DbId { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
    }

    public class UserModelBase
    {
        public AccessLevel AccessLevel { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Id { get; set; }
    }
    
    public class UserPhotoAlbumsModel : UserModelBase
    {
        //public List<FavoriteAlbumModel> FavoriteAlbums { get; set; }
        public List<FavoritePhotoModel> FavoritePhotos { get; set; }
        public List<PhotoAlbumModel> PhotoAlbums { get; set; }
    }

    public class UserEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public Guid Id { get; set; }
        public int DbId { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        //public List<FavoriteAlbumModel> FavoriteAlbums { get; set; }
        public List<FavoritePhotoModel> FavoritePhotos { get; set; }
        public List<PhotoAlbumModel> PhotoAlbums { get; set; }
    }
}