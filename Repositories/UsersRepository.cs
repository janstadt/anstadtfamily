using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using photoshare.Interfaces;
using photoshare.Models;
using AutoMapper;
using System.Web.Security;
using photoshare.Models.Enums;

namespace photoshare.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private photoshareEntities mEntities;
        public UsersRepository()
        {

        }

        /// <summary>
        /// Get User by username
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public UserEntity Get(string Username)
        {
            using (this.mEntities = new photoshareEntities())
            {
                user user = this.mEntities.users.FirstOrDefault(x => x.Username == Username);
                UserEntity entity = Mapper.Map<UserEntity>(user);
                entity.PhotoAlbums.ForEach(x => x.Favorite = user.favoritealbums.Any(y => y.Id == x.Id));
                return entity;
            }
        }
        
        public string[] GetRights(string userName)
        {
            string[] rights = Roles.GetRolesForUser(userName);
            return rights;
        }

        public IEnumerable<UserEntity> All()
        {
            using (this.mEntities = new photoshareEntities())
            {
                var users = this.mEntities.users.Where(x => x.IsActive);
                return Mapper.Map<IEnumerable<UserEntity>>(users);
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserEntity Get(Guid id)
        {
            using (this.mEntities = new photoshareEntities())
            {
                user user = this.mEntities.users.FirstOrDefault(x => x.Id == id);
                UserEntity entity = Mapper.Map<UserEntity>(user);
                entity.PhotoAlbums.ForEach(x => x.Favorite = user.favoritealbums.Any(y => y.AlbumId == x.Id));
                return entity;
            }
        }

        public UserEntity Add(UserEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                user user = Mapper.Map<user>(t);
                this.mEntities.users.AddObject(user);
                this.mEntities.SaveChanges();
            }
            return t;
        }

        public void Delete(UserEntity t)
        {
            throw new NotImplementedException();
        }

        public void Update(UserEntity t)
        {
            using (this.mEntities = new photoshareEntities())
            {
                user current = this.mEntities.users.FirstOrDefault(x => x.Id == t.Id);
                this.mEntities.users.Attach(current);
                user user = Mapper.Map<user>(t);
                this.mEntities.users.ApplyCurrentValues(user);
                this.mEntities.SaveChanges();
            }
        }
    }
}