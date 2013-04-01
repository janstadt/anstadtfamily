using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;
using photoshare.Models.Enums;
using System.Web;

namespace photoshare.Interfaces
{
    public interface ITagService
    {
        List<TagModel> GetTags(TagModel model);
        TagModel GetTag(Guid id);
        void DeleteTag(TagModel model);
        TagModel AddTag(TagModel model);
    }
}
