using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using photoshare.Models;
using photoshare.Repositories;
using photoshare.Interfaces;
using photoshare.Models.Enums;
using AutoMapper;
using System.IO;
using ImageResizer.Configuration;
using ImageResizer.Plugins.Watermark;
using ImageResizer;
using System.Drawing;
using System.Net;
using System.Drawing.Imaging;

namespace photoshare.Services
{
    public class PhotoService : IPhotoService
    {
        private IUsersRepository mUserRepository;
        private IPhotoRepository mPhotoRepository;
        public PhotoService(IUsersRepository usersRepository, IPhotoRepository photoRepository)
        {
            this.mUserRepository = usersRepository;
            this.mPhotoRepository = photoRepository;
        }

        public PhotoModel GetPhoto(string id)
        {
            PhotoEntity entity = this.mPhotoRepository.Get(id);
            return Mapper.Map<PhotoModel>(entity);
        }

        public void DeletePhoto(string id, string userId)
        {

            var photo = this.GetPhoto(id);

            if (photo.Owner.ToString() != userId)
            {
                throw new HttpException(403, string.Format("User {0} is not owner of photo {1}", userId, photo.Id));
            }

            var entity = Mapper.Map<PhotoEntity>(photo);

            this.mPhotoRepository.Delete(entity);

            //Delete photo
            this.RemovePhoto(photo);
        }

        private void RemovePhoto(PhotoModel photo)
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}", photo.AlbumId)));
            if (Directory.Exists(path))
            {
                string o = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}/{1}/{2}", photo.AlbumId, "Original", photo.FileName)));
                File.Delete(o);
                string l = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}/{1}/{2}", photo.AlbumId, "Large", photo.FileName)));
                File.Delete(l);
                string m = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}/{1}/{2}", photo.AlbumId, "Medium", photo.FileName)));
                File.Delete(m);
                string s = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}/{1}/{2}", photo.AlbumId, "Small", photo.FileName)));
                File.Delete(s);
                string t = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}/{1}/{2}", photo.AlbumId, "Thumb", photo.FileName)));
                File.Delete(t);
            }
        }

        public void Favorite(string id, string userId)
        {
            var photo = this.GetPhoto(id);

            var entity = Mapper.Map<PhotoEntity>(photo);
            entity.Owner = new Guid(userId);

            this.mPhotoRepository.Favorite(entity);
        }

        public void UnFavorite(string id, string userId)
        {
            var photo = this.GetPhoto(id);

            var entity = Mapper.Map<PhotoEntity>(photo);
            entity.Owner = new Guid(userId);

            this.mPhotoRepository.UnFavorite(entity);
        }

        public void UpdatePhoto(PhotoModel photo)
        {
            var entity = Mapper.Map<PhotoEntity>(photo);
            this.mPhotoRepository.Update(entity);
        }

        public PhotoModel AddSlideshowPhoto(PhotoModel photo, HttpRequestBase request)
        {
            var entity = Mapper.Map<PhotoEntity>(photo);
            entity.Slideshow = true;
            this.SaveSlideshowImage(photo, request);
            var result = this.mPhotoRepository.Add(entity);
            Mapper.Map(result, photo);
            //add photo to slideshow folder.
            return photo;
        }

        private string InitSlideshowDirectory()
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Content/img/slideshow")));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        private void SaveSlideshowImage(PhotoModel photo, HttpRequestBase request)
        {
            var file = request.Files[0];
            string filename = file.FileName;
            photo.FileName = filename;
            string path = this.InitSlideshowDirectory();
            path = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("{0}\\{1}", path, filename)));
            file.SaveAs(path);
        }

        public void RemoveSlideshowPhoto(PhotoModel photo)
        {
            var entity = Mapper.Map<PhotoEntity>(photo);
            var path = this.InitSlideshowDirectory();
            path = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("{0}\\{1}", path, entity.FileName)));
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            this.mPhotoRepository.Delete(entity);
        }

        public List<PhotoModel> SlideshowPhotos()
        {
            return Mapper.Map<List<PhotoModel>>(this.mPhotoRepository.Slideshow());
        }
    }
}