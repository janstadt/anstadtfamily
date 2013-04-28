using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using photoshare.Interfaces;
using photoshare.Models;
using AutoMapper;

namespace photoshare.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private photoshareEntities mEntities;
        public IEnumerable<PhotoEntity> All()
        {
            throw new NotImplementedException();
        }

        public PhotoEntity Get(string id)
        {
            PhotoEntity photo = new PhotoEntity();
            using (this.mEntities = new photoshareEntities())
            {
                var entity = this.mEntities.photos.FirstOrDefault(x => x.Id == new Guid(id));
                Mapper.Map(entity, photo);
                photo.Favorite = entity.favoritephotos.Count > 0;
            }
            return photo;
        }

        public PhotoEntity Add(PhotoEntity t)
        {
            photo photo = new photo();
            Mapper.Map(t, photo);
            using (this.mEntities = new photoshareEntities())
            {
                this.mEntities.photos.AddObject(photo);
                if (t.Favorite)
                {
                    favoritephoto favorite = new favoritephoto();
                    Mapper.Map(photo, favorite);
                    this.mEntities.favoritephotos.AddObject(favorite);
                }
                this.mEntities.SaveChanges();
            }
            return t;
        }

        public void Delete(PhotoEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                photo current = this.mEntities.photos.FirstOrDefault(x => x.Id == t.Id);
                if (current != null)
                {
                    this.mEntities.photos.DeleteObject(current);
                    this.mEntities.SaveChanges();
                }
            }
        }

        public void Update(PhotoEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                photo current = this.mEntities.photos.FirstOrDefault(x => x.Id == t.Id);
                this.mEntities.photos.Attach(current);
                photo photo = Mapper.Map<photo>(t);

                //Unselect all Main Images
                if (t.MainImage)
                {
                    this.mEntities.photos.Where(x => x.AlbumId == t.AlbumId).Each(x => x.MainImage = false);
                }

                this.mEntities.photos.ApplyCurrentValues(photo);

                if (t.Favorite)
                {
                    favoritephoto favorite = new favoritephoto();
                    favorite.Date = DateTime.UtcNow;
                    Mapper.Map(photo, favorite);
                    this.mEntities.favoritephotos.AddObject(favorite);
                }
                else
                {
                    var favorite = this.mEntities.favoritephotos.FirstOrDefault(x => x.PhotoId == t.Id && x.Owner == t.Owner);
                    this.mEntities.favoritephotos.DeleteObject(favorite);
                }

                this.mEntities.SaveChanges();
            }
        }

        public List<FavoritePhotoEntity> GetFavorites(string albumId, string userId)
        {
            using (this.mEntities = new photoshareEntities())
            {
                var favorites = this.mEntities.photos.Where(x => x.photoalbum.Id == new Guid(albumId) && x.favoritephotos.Any(y => y.Owner == new Guid(userId)));

                return Mapper.Map<List<FavoritePhotoEntity>>(favorites);
            }
        }

        public void Favorite(PhotoEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                photo current = this.mEntities.photos.FirstOrDefault(x => x.Id == t.Id);

                if (current != null)
                {
                    favoritephoto favoriteEntity = new favoritephoto();
                    Mapper.Map(t, favoriteEntity);
                    this.mEntities.favoritephotos.AddObject(favoriteEntity);
                    this.mEntities.SaveChanges();
                }
            }
        }

        public void UnFavorite(PhotoEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                favoritephoto current = this.mEntities.favoritephotos.FirstOrDefault(x => x.Owner == t.Owner && x.PhotoId == t.Id);
                photo p = this.mEntities.photos.FirstOrDefault(x => x.Id == current.PhotoId);
                if (current != null)
                {
                    if (p.MainImage)
                    {
                        p.MainImage = false;
                    }
                    this.mEntities.favoritephotos.DeleteObject(current);
                    this.mEntities.SaveChanges();
                }
            }
        }

        public bool FileNameExists(PhotoEntity entity)
        {
            using (this.mEntities = new photoshareEntities())
            {
                return this.mEntities.photos.Any(x => x.AlbumId == entity.AlbumId && x.FileName == entity.FileName);
            }
        }

        public List<PhotoEntity> Slideshow()
        {
            using (this.mEntities = new photoshareEntities())
            {
                return Mapper.Map<List<PhotoEntity>>(this.mEntities.photos.Where(x => x.Slideshow).ToList());
            }
        }
    }
}