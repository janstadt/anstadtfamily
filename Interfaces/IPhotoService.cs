using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;
using photoshare.Models.Enums;
using System.Web;

namespace photoshare.Interfaces
{
    public interface IPhotoService
    {
        PhotoModel GetPhoto(string id);
        void DeletePhoto(string id, string userId);
        void Favorite(string id, string userId);
        void UnFavorite(string id, string userId);
        void UpdatePhoto(PhotoModel photo);
    }
}
