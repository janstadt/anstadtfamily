using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using photoshare.Interfaces;
using photoshare.Models;
using AutoMapper;
using EntityFramework.Extensions;

namespace photoshare.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private photoshareEntities mEntities;
        
        public PhotoAlbumEntity Get(Guid id)
        {
            PhotoAlbumEntity album = new PhotoAlbumEntity();
            using (this.mEntities = new photoshareEntities())
            {
                var entity = this.mEntities.photoalbums.FirstOrDefault(x => x.Id == id);
                Mapper.Map(entity, album);
                album.Favorite = entity.favoritealbums.Count > 0;
            }
            return album;
        }

        public IEnumerable<PhotoAlbumEntity> All()
        {
            throw new NotImplementedException();
        }

        public PhotoAlbumEntity Add(PhotoAlbumEntity t)
        {
            photoalbum album = new photoalbum();
            Mapper.Map(t, album);
            using (this.mEntities = new photoshareEntities())
            {
                this.mEntities.photoalbums.AddObject(album);
                if (t.Favorite)
                {
                    favoritealbum favorite = new favoritealbum();
                    Mapper.Map(album, favorite);
                    this.mEntities.favoritealbums.AddObject(favorite);
                }
                this.mEntities.SaveChanges();
            }
            return t;
        }

        public void Delete(PhotoAlbumEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                photoalbum current = this.mEntities.photoalbums.FirstOrDefault(x => x.Id == t.Id);
                if(current != null)
                {
                    this.mEntities.photoalbums.DeleteObject(current);
                }
                this.mEntities.SaveChanges();
            }
        }

        public void Update(PhotoAlbumEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                photoalbum current = this.mEntities.photoalbums.FirstOrDefault(x => x.Id == t.Id);
                this.mEntities.photoalbums.Attach(current);
                photoalbum album = Mapper.Map<photoalbum>(t);
                this.mEntities.photoalbums.ApplyCurrentValues(album);

                
                if (t.Favorite)
                {
                    var favorite = this.mEntities.favoritealbums.FirstOrDefault(x => x.AlbumId == t.Id && x.Owner == t.Owner);
                    if (favorite == null)
                    {
                        favoritealbum favoriteEntity = new favoritealbum();
                        Mapper.Map(album, favoriteEntity);
                        this.mEntities.favoritealbums.AddObject(favoriteEntity);
                    }
                }
                else
                {
                    var favorite = this.mEntities.favoritealbums.FirstOrDefault(x => x.AlbumId == t.Id && x.Owner == t.Owner);
                    if (favorite != null)
                    {
                        this.mEntities.favoritealbums.DeleteObject(favorite);
                    }
                }

                this.mEntities.SaveChanges();
            }
        }
    }
}