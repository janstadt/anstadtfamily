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
    public class AlbumService : IAlbumService
    {
        private IUsersRepository mUserRepository;
        private IAlbumRepository mAlbumRepository;
        private IPhotoRepository mPhotoRepository;
        private ITagRepository mTagRepository;
        public AlbumService(IUsersRepository usersRepository, IAlbumRepository albumRepository, IPhotoRepository photoRepository, ITagRepository tagRepository)
        {
            this.mUserRepository = usersRepository;
            this.mAlbumRepository = albumRepository;
            this.mPhotoRepository = photoRepository;
            this.mTagRepository = tagRepository;
        }

        //public UserModel GetUser(Guid id)
        //{
        //    var user = this.mUserRepository.Get(id);
        //    var userModel = Mapper.Map<UserModel>(user);
        //    this.SetAccessLevel(userModel);
        //    return userModel;
        //}

        //public UserModel GetUser(string username)
        //{
        //    var user = this.mUserRepository.Get(username);
        //    var userModel = Mapper.Map<UserModel>(user);
        //    this.SetAccessLevel(userModel);
        //    return userModel;
        //}

        //public void SetAccessLevel(UserModelBase model)
        //{
        //    var rights = this.mUserRepository.GetRights(model.Username);
        //    string currentUser = this.mSessionRepository.GetCurrentUsername();
        //    if (rights.Contains("Administrator"))
        //    {
        //        if (currentUser == model.Username)
        //        {
        //            model.AccessLevel = AccessLevel.AdminAndOwner;
        //        }
        //        else
        //        {
        //            model.AccessLevel = AccessLevel.Admin;
        //        }
        //    }
        //    else if (currentUser == model.Username)
        //    {
        //        model.AccessLevel = AccessLevel.Owner;
        //    }
        //    else
        //    {
        //        model.AccessLevel = AccessLevel.NoAccess;
        //    }
        //}

        //public UserModelBase Update(UserModelBase model)
        //{
        //    //Only allowed to update name and email so only pay attention to those fields.
        //    var currentUser = this.GetUser(model.Id);
        //    currentUser.Email = model.Email;
        //    currentUser.Name = model.Name;
        //    var entity = Mapper.Map<UserEntity>(currentUser);
        //    this.SetAccessLevel(model);
        //    this.mUserRepository.Update(entity);
        //    return model;
        //}

        //public UserPhotoAlbumsModel GetAlbums(UserModelBase model)
        //{
        //    var currentUser = this.GetUser(model.Id);
        //    var photoModel = Mapper.Map<UserPhotoAlbumsModel>(currentUser);
        //    this.SetAccessLevel(photoModel);
        //    return photoModel;
        //}

        public PhotoAlbumModel GetAlbum(Guid id)
        {
            PhotoAlbumEntity entity = this.mAlbumRepository.Get(id);
            PhotoAlbumModel model = Mapper.Map<PhotoAlbumModel>(entity);
            var tags = this.mTagRepository.All(Mapper.Map<TagEntity>(entity));
            model.Tags = Mapper.Map<List<TagModel>>(tags.ToList());
            return model;
        }

        public PhotoAlbumModel CreateAlbum(PhotoAlbumModel model)
        {
            PhotoAlbumEntity entity = Mapper.Map<PhotoAlbumEntity>(model);
            entity.Date = DateTime.UtcNow;
            this.mAlbumRepository.Add(entity);
            this.CreateDirectory(entity);
            model.Id = entity.Id;
            this.AddTags(model);
            
            return Mapper.Map(entity, model);
        }

        private void AddTags(PhotoAlbumModel model)
        {
            var tagEntities = Mapper.Map<List<TagEntity>>(model.AddedTags);
            for(int i = 0; i < tagEntities.Count; i++)
            {
                tagEntities[i].Owner = model.Owner;
                tagEntities[i].ParentId = model.Id;
                tagEntities[i].Id = Guid.NewGuid();
                tagEntities[i].Type = TagType.Albums.ToString();
                tagEntities[i].Date = DateTime.UtcNow;
                this.mTagRepository.Add(tagEntities[i]);
            }
        }

        private void CreateDirectory(PhotoAlbumEntity entity)
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}", entity.Id)));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Directory.CreateDirectory(path + "/Thumb");
                Directory.CreateDirectory(path + "/Small");
                Directory.CreateDirectory(path + "/Medium");
                Directory.CreateDirectory(path + "/Large");
                Directory.CreateDirectory(path + "/Original");
            }
        }

        private void RemoveDirectory(Guid id)
        {
             var path = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}", id)));
             if (Directory.Exists(path))
             {
                 Directory.Delete(path, true);
             }
        }

        public void DeleteAlbum(Guid id, Guid userId)
        {

            var album = this.GetAlbum(id);

            if (album.Owner != userId)
            {
                throw new HttpException(403, string.Format("User {0} is not owner of album {1}", userId, album.Id));
            }

            var entity = Mapper.Map<PhotoAlbumEntity>(album);

            this.mAlbumRepository.Delete(entity);

            //Delete directory
            this.RemoveDirectory(id);
        }

        public PhotoAlbumBaseModel UpdateAlbum(PhotoAlbumBaseModel model)
        {
            //Only allowed to update name and email so only pay attention to those fields.
            var currentAlbum = this.GetAlbum(model.Id);
            currentAlbum.Title = model.Title;
            currentAlbum.Favorite = model.Favorite;
            currentAlbum.Description = model.Description;
            var entity = Mapper.Map<PhotoAlbumEntity>(currentAlbum);
            this.mAlbumRepository.Update(entity);
            return model;
        }

        private void ScaleAndSavePhotos(PhotoModel model, HttpRequestBase request)
        {
            var file = request.Files[0];
            string extension = Path.GetExtension(file.FileName);
            string filename = file.FileName;

            //Copied Image and add watermark.
            ResizeSettings settings = new ResizeSettings();
            settings.Add("format", "jpg");
            settings.Add("quality", "90");
            settings.Add("carve", "true");
            settings.Add("scale", "down");
            settings.Add("mode", "carve");
            if (model.Watermark)
            {
                settings.Add("watermark", "wm");
            }

            var path = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/Albums/{0}", model.AlbumId)));
            if (Directory.Exists(path))
            {
                //thumb is 75x75
                //small is 240x159
                //medium is 500 x 332
                //large is 1024 x 680

                Image img = Image.FromStream(file.InputStream);
                
                //PropertyItem[] properties = img.PropertyItems;

                //foreach (PropertyItem prop in properties)
                //{

                //}

                bool vert = img.Height > img.Width;
                
                settings.Add("maxwidth", vert ? "680" : "1024");
                //settings.Add("maxheight", vert ? "1024" : "680");

                Bitmap copy = ImageResizer.ImageBuilder.Current.Build(img, settings, false);
                copy.Save(Path.Combine(path, "Large\\" + filename));

                settings.Remove("maxwidth");
                //settings.Remove("maxheight");
                settings.Remove("watermark");
                settings.Add("maxwidth", vert ? "332" : "500");
                //settings.Add("maxheight", vert ? "500" : "332");
                copy = ImageResizer.ImageBuilder.Current.Build(img, settings, false);
                copy.Save(Path.Combine(path, "Medium\\" + filename));

                settings.Remove("maxwidth");
                //settings.Remove("maxheight");
                settings.Remove("watermark");
                settings.Add("maxwidth", vert ? "200" : "301");
                //settings.Add("maxheight", vert ? "301" : "200");
                copy = ImageResizer.ImageBuilder.Current.Build(img, settings, false);
                copy.Save(Path.Combine(path, "Small\\" + filename));

                settings.Remove("maxwidth");
                settings.Remove("maxwidth");
                settings.Remove("watermark");
                settings.Add("maxwidth", "75");
                settings.Add("maxheight", "75");
                copy = ImageResizer.ImageBuilder.Current.Build(img, settings, false);
                copy.Save(Path.Combine(path, "Thumb\\" + filename));

                file.SaveAs(Path.Combine(path, "Original\\" + filename));
            }
        }

        public PhotoModel AddPhoto(PhotoModel model, HttpRequestBase request)
        {
            //Extract image
            for(int i = 0; i < request.Files.Count; i++) 
            {
                PhotoEntity entity = Mapper.Map<PhotoEntity>(model);
                entity.Date = DateTime.UtcNow;
                entity.FileName = request.Files[i].FileName;

                //does file exist? Do a better deal here.
                if (this.mPhotoRepository.FileNameExists(entity))
                {
                    model.FileExists = true;
                    //throw new HttpException(409, "File name exists!");
                }
                else
                {
                    Mapper.Map(this.mPhotoRepository.Add(entity), model);
                    //Save the photos.
                    this.ScaleAndSavePhotos(model, request);
                }
            }
            return model;
        }

        public void Favorite(Guid id, Guid userId)
        {
            var album = this.GetAlbum(id);

            //if (album.Owner != userId)
            //{
            //    throw new HttpException(403, string.Format("User {0} is not owner of album {1}", userId, album.Id));
            //}

            var entity = Mapper.Map<PhotoAlbumEntity>(album);
            entity.Favorite = true;

            this.mAlbumRepository.Update(entity);
        }

        public void UnFavorite(Guid id, Guid userId)
        {
            var album = this.GetAlbum(id);

            //if (album.Owner != userId)
            //{
            //    throw new HttpException(403, string.Format("User {0} is not owner of album {1}", userId, album.Id));
            //}

            var entity = Mapper.Map<PhotoAlbumEntity>(album);
            entity.Favorite = false;

            this.mAlbumRepository.Update(entity);
        }

        public List<BreadcrumbModel> Breadcrumbs(Guid id)
        {
            var album = this.GetAlbum(id);

            if (album.Owner == Guid.Empty)
            {
                throw new HttpException(404, string.Format("Album {0} does not have an owner!", id));
            }
            
            var user = this.mUserRepository.Get(album.Owner);

            List<BreadcrumbModel> crumbs = new List<BreadcrumbModel>();
            
            BreadcrumbModel parent = new BreadcrumbModel();
            parent.Text = string.Format("{0}'s Albums", user.Username);
            parent.Url = string.Format("/#/user/{0}", user.Id); //"/#/user/<%- Id %>";
            parent.Id = user.Id;

            crumbs.Add(parent);

            BreadcrumbModel active = new BreadcrumbModel();
            active.Active = true;
            active.Text = album.Title;
            active.Id = album.Id;

            crumbs.Add(active);

            return crumbs;
        }
    }
}