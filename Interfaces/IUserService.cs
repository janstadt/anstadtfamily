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
        UserModel GetUser(string id, string loggedInUser);
        UserModelBase Update(UserModelBase model);
        UserModelBase Add(UserModelBase model);
        UserPhotoAlbumsModel GetAlbums(UserModelBase model, string id);
        void SetAccessLevel(UserModelBase model);
        List<UserModel> GetUsers();
    }
}
