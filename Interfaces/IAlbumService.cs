using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;
using photoshare.Models.Enums;
using System.Web;

namespace photoshare.Interfaces
{
    public interface IAlbumService
    {
        PhotoAlbumModel GetAlbum(Guid id);
        PhotoAlbumModel GetAlbumPhotos(Guid id, Guid userId);
        PhotoAlbumModel CreateAlbum(PhotoAlbumModel model);
        void DeleteAlbum(Guid id, Guid userId);
        PhotoAlbumBaseModel UpdateAlbum(PhotoAlbumBaseModel model);
        PhotoModel AddPhoto(PhotoModel model, HttpRequestBase request);
        void Favorite(Guid id, Guid userId);
        void UnFavorite(Guid id, Guid userId);
        List<BreadcrumbModel> Breadcrumbs(Guid id);
    }
}
