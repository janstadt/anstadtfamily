using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;
using photoshare.Models.Enums;

namespace photoshare.Interfaces
{
    public interface ISessionService
    {
        LoginModel Login(LoginModel model);
        LoginModel GetSession();
        void Logout();
    }
}
