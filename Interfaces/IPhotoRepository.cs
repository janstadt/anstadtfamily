using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;

namespace photoshare.Interfaces
{
    public interface IPhotoRepository : IDalRepository<PhotoEntity>
    {
        bool FileNameExists(PhotoEntity entity);
    }
}
