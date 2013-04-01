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

        public PhotoModel GetPhoto(Guid id)
        {
            PhotoEntity entity = this.mPhotoRepository.Get(id);
            return Mapper.Map<PhotoModel>(entity);
        }

        public void DeletePhoto(Guid id, Guid userId)
        {

            var photo = this.GetPhoto(id);

            if (photo.Owner != userId)
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

        public void Favorite(Guid id, Guid userId)
        {
            var photo = this.GetPhoto(id);

            //if (photo.Owner != userId)
            //{
            //    throw new HttpException(403, string.Format("User {0} is not owner of photo {1}", userId, photo.Id));
            //}

            var entity = Mapper.Map<PhotoEntity>(photo);
            entity.Favorite = true;

            this.mPhotoRepository.Update(entity);
        }

        public void UnFavorite(Guid id, Guid userId)
        {
            var photo = this.GetPhoto(id);

            //if (photo.Owner != userId)
            //{
            //    throw new HttpException(403, string.Format("User {0} is not owner of photo {1}", userId, photo.Id));
            //}

            var entity = Mapper.Map<PhotoEntity>(photo);
            entity.Favorite = false;

            this.mPhotoRepository.Update(entity);
        }
    }
}