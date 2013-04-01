using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using photoshare.Interfaces;
using photoshare.Models;
using AutoMapper;

namespace photoshare.Repositories
{
    public class TagRepository : ITagRepository
    {
        private photoshareEntities mEntities;
        public IEnumerable<TagEntity> All()
        {
            using (this.mEntities = new photoshareEntities())
            {
                var list = this.mEntities.tags.ToList();
                return Mapper.Map<IEnumerable<TagEntity>>(list);
            }
        }

        public IEnumerable<TagEntity> All(TagEntity entity)
        {
            IEnumerable<TagEntity> all = this.All();
            all = all.Where(x => x.Type == entity.Type);
            if (entity.ParentId != Guid.Empty)
            {
                all = all.Where(x => x.ParentId == entity.ParentId);
            }
            all = all.GroupBy(x => x.Name).Select(x => x.First());
            return all;
        }

        public TagEntity Get(Guid id)
        {
            TagEntity tag = new TagEntity();
            using (this.mEntities = new photoshareEntities())
            {
                var entity = this.mEntities.tags.FirstOrDefault(x => x.Id == id);
                Mapper.Map(entity, tag);
            }
            return tag;
        }

        public TagEntity Add(TagEntity t)
        {
            tag tag = new tag();
            Mapper.Map(t, tag);
            using (this.mEntities = new photoshareEntities())
            {
                tag existingTag = this.mEntities.tags.FirstOrDefault(x => x.Name.ToUpper() == t.Name.ToUpper() && x.Type == t.Type && x.ParentId == t.ParentId);
                if (existingTag == null)
                {
                    this.mEntities.tags.AddObject(tag);
                    this.mEntities.SaveChanges();
                }
            }
            return t;
        }

        public void Delete(TagEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                tag current = this.mEntities.tags.FirstOrDefault(x => x.Id == t.Id);
                if (current != null)
                {
                    this.mEntities.tags.DeleteObject(current);
                    this.mEntities.SaveChanges();
                }
            }
        }

        public void Update(TagEntity t)
        {
            throw new NotImplementedException();
            //using (this.mEntities = new photoshareEntities())
            //{
            //    photo current = this.mEntities.photos.FirstOrDefault(x => x.Id == t.Id);
            //    this.mEntities.photos.Attach(current);
            //    photo photo = Mapper.Map<photo>(t);
            //    this.mEntities.photos.ApplyCurrentValues(photo);

            //    if (t.Favorite)
            //    {
            //        favoritephoto favorite = new favoritephoto();
            //        favorite.Date = DateTime.UtcNow;
            //        Mapper.Map(photo, favorite);
            //        this.mEntities.favoritephotos.AddObject(favorite);
            //    }
            //    else
            //    {
            //        var favorite = this.mEntities.favoritephotos.FirstOrDefault(x => x.PhotoId == t.Id && x.Owner == t.Owner);
            //        this.mEntities.favoritephotos.DeleteObject(favorite);
            //    }

            //    this.mEntities.SaveChanges();
            //}
        }
    }
}