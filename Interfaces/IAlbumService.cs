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
        PhotoAlbumModel GetPortoflioAlbum(PhotoAlbumBaseModel model);
        PhotoAlbumModel GetAlbum(string id);
        PhotoAlbumModel GetAlbumPhotos(string id, string userId);
        PhotoAlbumModel CreateAlbum(PhotoAlbumModel model);
        void DeleteAlbum(string id, string userId);
        PhotoAlbumBaseModel UpdateAlbum(PhotoAlbumBaseModel model);
        PhotoModel AddPhoto(PhotoModel model, HttpRequestBase request);
        void Favorite(string id, string userId);
        void UnFavorite(string id, string userId);
        List<BreadcrumbModel> Breadcrumbs(string id);
        PortfolioModel GetPortfolio(PortfolioModel model);
        List<PortfolioModel> GetFullPortfolio();
    }
}
