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
        PhotoModel GetPhoto(Guid id);
        void DeletePhoto(Guid id, Guid userId);
        void Favorite(Guid id, Guid userId);
        void UnFavorite(Guid id, Guid userId);
    }
}
