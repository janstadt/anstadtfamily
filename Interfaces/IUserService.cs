using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;
using photoshare.Models.Enums;

namespace photoshare.Interfaces
{
    public interface IUserService
    {
        UserModel GetUser(string username);
        UserModel GetUser(Guid id);
        UserModelBase Update(UserModelBase model);
        UserPhotoAlbumsModel GetAlbums(UserModelBase model);
        void SetAccessLevel(UserModelBase model);
    }
}
