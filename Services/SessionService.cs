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
        private ITagRepository mTagRepository;
        public SessionService(ISessionRepository sessionRepository, IUserService userService, ITagRepository tagRepository)
        {
            this.mSessionRepository = sessionRepository;
            this.mUserService = userService;
            this.mTagRepository = tagRepository;
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

        public LoginHeaderModel GetHeaderSession()
        {
            var session = this.GetSession();
            var categories = this.Categories();

            var lhmodel = Mapper.Map<LoginHeaderModel>(session);
            lhmodel.Categories = categories;

            return lhmodel;
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

        public List<TagModel> Categories()
        {
            var all = this.mTagRepository.Categories();
            return Mapper.Map<List<TagModel>>(all);
        }

        public void Contact(ContactUsModel model)
        {
            var entity = Mapper.Map<EmailEntity>(model);
            this.mSessionRepository.SendEmail(entity);
        }
    }
}