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
    public class TagService : ITagService
    {
        private IUsersRepository mUserRepository;
        private ITagRepository mTagRepository;
        public TagService(IUsersRepository usersRepository, ITagRepository tagRepository)
        {
            this.mUserRepository = usersRepository;
            this.mTagRepository = tagRepository;
        }

        public List<TagModel> GetTags()
        {
            return Mapper.Map<List<TagModel>>(this.mTagRepository.All().ToList());
        }

        public List<TagModel> GetUniqeTags()
        {
            return Mapper.Map<List<TagModel>>(this.mTagRepository.All().Where(x => x.IsCategory).GroupBy(x => new {x.Name, x.IsCategory}).Select(x => x.First()).OrderBy(x => x.Name).ToList());
        }

        public List<TagModel> GetTags(TagModel model)
        {
            TagEntity entity = new TagEntity()
            {
                Type = model.Type.ToString(),
                ParentId = model.ParentId
            };
            return Mapper.Map<List<TagModel>>(this.mTagRepository.All(entity).ToList());
        }

        public TagModel GetTag(Guid id)
        {
            throw new NotImplementedException();
        }

        public TagModel AddTag(TagModel tag)
        {
            TagEntity entity = Mapper.Map<TagEntity>(tag);
            this.mTagRepository.Add(entity);
            return Mapper.Map<TagModel>(entity);
        }

        public TagModel UpdateTag(TagModel tag)
        {
            TagEntity entity = Mapper.Map<TagEntity>(tag);
            this.mTagRepository.Update(entity);
            return Mapper.Map<TagModel>(entity);
        }

        public void DeleteTag(TagModel tag)
        {
            TagEntity entity = Mapper.Map<TagEntity>(tag);
            this.mTagRepository.Delete(entity);
        }
    }
}