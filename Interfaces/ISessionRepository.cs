using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;

namespace photoshare.Interfaces
{
    public interface ISessionRepository
    {
        void Logout();
        bool Validate(LoginModel user);
        bool IsUserLockedOut(string Username);
        bool IsAuthenticated();
        string GetCurrentUsername();
        void CreateAuthCookie(UserModel user);
    }
}
