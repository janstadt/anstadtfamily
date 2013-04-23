using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;
using System.Collections;

namespace photoshare.Interfaces
{
    public interface ITagRepository : IDalRepository<TagEntity>
    {
        IEnumerable<TagEntity> All(TagEntity entity);
        List<TagEntity> Categories();
    }
}
