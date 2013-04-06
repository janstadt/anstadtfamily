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
                var item = this.mEntities.tags.FirstOrDefault(x => x.Id == t.Id);
                if (item != null)
                {
                    //Delete ALL of the specific tag references.
                    if (t.IsCategory)
                    {
                        var tags = this.mEntities.tags.Where(x => x.Name == item.Name && x.Type == item.Type);
                        tags.ToList().ForEach(this.mEntities.tags.DeleteObject);
                    }
                    else
                    {
                        this.mEntities.tags.DeleteObject(item);
                    }
                    this.mEntities.SaveChanges();
                }
            }
        }

        public void Update(TagEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                tag current = this.mEntities.tags.FirstOrDefault(x => x.Id == t.Id);
                this.mEntities.tags.Attach(current);
                tag tag = Mapper.Map<tag>(t);
                this.mEntities.tags.ApplyCurrentValues(tag);
                this.mEntities.SaveChanges();
            }
        }
    }
}