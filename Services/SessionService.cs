using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using photoshare.Models;
using photoshare.Repositories;
using photoshare.Interfaces;
using photoshare.Models.Enums;
using AutoMapper;

namespace photoshare.Services
{
    public class SessionService : ISessionService
    {
        private ISessionRepository mSessionRepository;
        private IUserService mUserService;

        public SessionService(ISessionRepository sessionRepository, IUserService userService)
        {
            this.mSessionRepository = sessionRepository;
            this.mUserService = userService;
        }
        public void Logout()
        {
            this.mSessionRepository.Logout();
        }
        public LoginModel GetSession()
        {
            LoginModel model = new LoginModel();
            model.LoginStatus = LoginStatus.InvalidUser;
            if (this.mSessionRepository.IsAuthenticated())
            {
                UserModel userModel = this.GetUser(this.mSessionRepository.GetCurrentUsername());
                this.mUserService.SetAccessLevel(userModel);
                Mapper.Map(userModel, model);
                model.LoginStatus = LoginStatus.LoggedIn;
            }
            return model;
        }

        private UserModel GetUser(string username)
        {
            var user = this.mUserService.GetUser(username);
            return user;
        }

        public LoginModel Login(LoginModel model)
        {
            this.mSessionRepository.Logout();
            model.LoginStatus = LoginStatus.InvalidUser;
            if (this.mSessionRepository.Validate(model))
            {
                if (!this.mSessionRepository.IsAuthenticated())
                {
                    var userModel = this.GetUser(model.Username);
                    this.mSessionRepository.CreateAuthCookie(userModel);
                    this.mUserService.SetAccessLevel(userModel);
                    Mapper.Map(userModel, model);
                    model.LoginStatus = LoginStatus.LoggedIn;
                }
            }
            else
            {
                HttpContext.Current.Response.StatusCode = 403;
                if (model.Username != null && this.mSessionRepository.IsUserLockedOut(model.Username))
                {
                    model.LoginStatus = LoginStatus.LockedOut;
                    HttpContext.Current.Response.StatusCode = 423;
                }
            }
            return model;
        }
    }
}