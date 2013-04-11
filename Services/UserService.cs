using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using photoshare.Models;
using photoshare.Repositories;
using photoshare.Interfaces;
using photoshare.Models.Enums;
using AutoMapper;
using System.Web.Security;

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

        public List<UserModel> GetUsers()
        {
            var user = this.mUserRepository.All();
            var userModels = Mapper.Map<List<UserModel>>(user);
            userModels.ForEach(x => this.SetAccessLevel(x));
            var owner = userModels.FirstOrDefault(x => x.AccessLevel == AccessLevel.Owner || x.AccessLevel == AccessLevel.AdminAndOwner);
            owner.Password = this.GetUserPassword(owner.Username);
            return userModels;
        }

        public UserModel GetUser(Guid id)
        {
            var user = this.mUserRepository.Get(id);
            var userModel = Mapper.Map<UserModel>(user);
            this.SetAccessLevel(userModel);
            userModel.Password = this.GetUserPassword(userModel.Username);
            return userModel;
        }

        private string GetUserPassword(string username)
        {
            var dbUser = Membership.GetUser(username);
            if (dbUser == null)
            {
                throw new HttpException(404, "User not found");
            }
            return dbUser.GetPassword();
        }

        public UserModel GetUser(string username)
        {
            var user = this.mUserRepository.Get(username);
            var userModel = Mapper.Map<UserModel>(user);
            this.SetAccessLevel(userModel);
            userModel.Password = this.GetUserPassword(username);
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

        private bool UserExists(UserModelBase model)
        {
            bool exists = false;

            if (Membership.FindUsersByName(model.Username).Count > 0 ||
                Membership.FindUsersByEmail(model.Email).Count > 0)
            {
                exists = true;
            }

            return exists;
        }

        public UserModelBase Add(UserModelBase model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Email))
            {
                throw new HttpException(400, "Invalid user information");
            }

            if (this.UserExists(model))
            {
                throw new HttpException(409, "User exists");
            }
            //Create user
            var user = Membership.CreateUser(model.Username, model.Password, model.Email);
            
            //Set Roles
            string[] roles = this.GetRoles(model);
            Roles.AddUserToRoles(model.Username, roles);

            var entity = Mapper.Map<UserEntity>(model);
            entity.DbId = ((int)user.ProviderUserKey);
            entity.ActivationDate = DateTime.UtcNow;
            entity.IsActive = true;

            this.mUserRepository.Add(entity);

            Mapper.Map(entity, model);

            return model;
        }

        private string[] GetRoles(UserModelBase model)
        {
            List<string> roles = new List<string>();

            switch (model.AccessLevel)
            {
                case AccessLevel.Admin:
                    roles.Add("Administrator");
                    break;
                case AccessLevel.AdminAndOwner:
                    roles.Add("Administrator");
                    roles.Add("Owner");
                    break;
                case AccessLevel.Owner:
                    roles.Add("Owner");
                    break;
                default:
                    roles.Add("Contributor");
                    break;
            }

            return roles.ToArray();
        }

        public UserModelBase Update(UserModelBase model)
        {
            //Only allowed to update name and email so only pay attention to those fields.
            var currentUser = this.GetUser(model.Id);
            currentUser.Email = model.Email;
            currentUser.Name = model.Name;
            currentUser.Phone = model.Phone;
            var entity = Mapper.Map<UserEntity>(currentUser);
            this.SetAccessLevel(model);
            
            this.mUserRepository.Update(entity);

            var dbUser = Membership.GetUser(model.Username);
            dbUser.Email = model.Email;

            //Only update pwd if the user is an owner of some type.
            if (model.AccessLevel == AccessLevel.AdminAndOwner || model.AccessLevel == AccessLevel.Owner)
            {
                string oldPwd = dbUser.GetPassword();
                if (oldPwd != model.Password)
                {
                    dbUser.ChangePassword(oldPwd, model.Password);
                }
            }

            if (model.AccessLevel == AccessLevel.Admin || model.AccessLevel == AccessLevel.AdminAndOwner)
            {
                var newRoles = this.GetRoles(model);
                var oldRoles = Roles.GetRolesForUser(model.Username);
                Roles.RemoveUserFromRoles(model.Username, oldRoles);
                Roles.AddUserToRoles(model.Username, newRoles);
            }

            Membership.UpdateUser(dbUser);
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