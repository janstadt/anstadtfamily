using AutoMapper;
using photoshare.Models;
using System.Linq;
using Microsoft.Practices.Unity;
using photoshare.Repositories;
using photoshare.Interfaces;
using photoshare.Services;
using System.Web.Mvc;
using System;
using System.Web.Routing;
using System.Reflection;
using System.Collections.Generic;
namespace vdz.ca.IoC
{
    public static class IoC
    {
        public static void Register(IUnityContainer container)
        {
            IoC.RegisterControllers(container);
            IoC.RegisterServices(container);
            IoC.RegisterRepositories(container);
        }
        private static void RegisterControllers(IUnityContainer container)
        {

        }
        private static void RegisterServices(IUnityContainer container)
        {
            container.RegisterType<ITagService, TagService>();
            container.RegisterType<ISessionService, SessionService>();
            container.RegisterType<IAlbumService, AlbumService>();
            container.RegisterType<IPhotoService, PhotoService>();
            container.RegisterType<IUserService, UserService>();
        }
        private static void RegisterRepositories(IUnityContainer container)
        {
            container.RegisterType<ITagRepository, TagRepository>();
            container.RegisterType<IUsersRepository, UsersRepository>();
            container.RegisterType<IPhotoRepository, PhotoRepository>();
            container.RegisterType<IAlbumRepository, AlbumRepository>();
            container.RegisterType<ISessionRepository, SessionRepository>();
        }
    }

    public class UnityDependencyResolver : IDependencyResolver
    {
        readonly IUnityContainer _container;
        public UnityDependencyResolver(IUnityContainer container)
        {
            this._container = container;
        }
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}
