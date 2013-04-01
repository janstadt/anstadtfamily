using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using photoshare.Models;
using photoshare.Interfaces;
namespace photoshare.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        public void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.UtcNow.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.UtcNow.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie2);

            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.User = null;
        }

        public bool Validate(LoginModel user)
        {
            return Membership.ValidateUser(user.Username, user.Password);
        }

        public bool IsUserLockedOut(string Username)
        {
            var user = Membership.GetUser(Username);
            if (user == null)
            {
                return false;
            }
            return user.IsLockedOut;

        }

        public bool IsAuthenticated()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        public string GetCurrentUsername()
        {
            try
            {
                return HttpContext.Current.User.Identity.Name;
            }
            catch (Exception e)
            {
                //User doesnt exist yet.
            }
            return "";
        }

        public void CreateAuthCookie(UserModel user)
        {
            DateTime cookieExpiration = DateTime.UtcNow.AddMinutes(FormsAuthentication.Timeout.TotalMinutes); // change to months for production
            var authenticationTicket =
                new FormsAuthenticationTicket(
                    1,
                    user.Username,
                    DateTime.UtcNow,
                    cookieExpiration,
                    false,
                    user.Id.ToString(),
                    FormsAuthentication.FormsCookiePath);

            //ticket must be encrypted
            string encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);

            //create cookie to contain encrypted auth ticket
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.Expires = authenticationTicket.Expiration;
            authCookie.Path = FormsAuthentication.FormsCookiePath;
            // clear out existing cookie for good measure (probably overkill) then add
            HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }
    }
}