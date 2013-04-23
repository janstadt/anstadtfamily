using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;

namespace photoshare.Interfaces
{
    public interface IPhotoRepository : IDalRepository<PhotoEntity>
    {
        List<FavoritePhotoEntity> GetFavorites(string albumId, string userId);
        bool FileNameExists(PhotoEntity entity);
        void Favorite(PhotoEntity entity);
        void UnFavorite(PhotoEntity entity);
    }
}
