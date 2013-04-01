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
    public class UserService : IUserService
    {
        private IUsersRepository mUserRepository;
        private ISessionRepository mSessionRepository;
        public UserService(IUsersRepository usersRepository, ISessionRepository sessionRepository)
        {
            this.mUserRepository = usersRepository;
            this.mSessionRepository = sessionRepository;
        }
        
        public UserModel GetUser(Guid id)
        {
            var user = this.mUserRepository.Get(id);
            var userModel = Mapper.Map<UserModel>(user);
            this.SetAccessLevel(userModel);
            return userModel;
        }

        public UserModel GetUser(string username)
        {
            var user = this.mUserRepository.Get(username);
            var userModel = Mapper.Map<UserModel>(user);
            this.SetAccessLevel(userModel);
            return userModel;
        }

        public void SetAccessLevel(UserModelBase model)
        {
            var rights = this.mUserRepository.GetRights(model.Username);
            string currentUser = this.mSessionRepository.GetCurrentUsername();
            if (rights.Contains("Administrator"))
            {
                if (currentUser == model.Username)
                {
                    model.AccessLevel = AccessLevel.AdminAndOwner;
                }
                else
                {
                    model.AccessLevel = AccessLevel.Admin;
                }
            }
            else if (currentUser == model.Username)
            {
                model.AccessLevel = AccessLevel.Owner;
            }
            else
            {
                model.AccessLevel = AccessLevel.NoAccess;
            }
        }

        public UserModelBase Update(UserModelBase model)
        {
            //Only allowed to update name and email so only pay attention to those fields.
            var currentUser = this.GetUser(model.Id);
            currentUser.Email = model.Email;
            currentUser.Name = model.Name;
            var entity = Mapper.Map<UserEntity>(currentUser);
            this.SetAccessLevel(model);
            this.mUserRepository.Update(entity);
            return model;
        }

        public UserPhotoAlbumsModel GetAlbums(UserModelBase model)
        {
            var currentUser = this.GetUser(model.Id);
            var photoModel = Mapper.Map<UserPhotoAlbumsModel>(currentUser);
            this.SetAccessLevel(photoModel);
            return photoModel;
        }
    }
}