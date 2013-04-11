using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using photoshare.Models;
using photoshare.Interfaces;
using photoshare.Helpers;
using AutoMapper;
using photoshare.Models.Enums;
namespace photoshare.Controllers
{
    [AjaxAuthorize]
    public class UsersController : BaseController
    {
        private ISessionService mSessionService;
        private IUserService mUserService;
        public UsersController(ISessionService sessionService, IUserService userService)
        {
            this.mSessionService = sessionService;
            this.mUserService = userService;
        }

        [HttpGet]
        [AjaxAuthorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var users = this.mUserService.GetUsers();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("User")]
        [AjaxAuthorize(Roles = "Administrator")]
        public ActionResult Create(UserModelBase model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                model = this.mUserService.Add(model);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("User")]
        public ActionResult GetUser(Guid id)
        {
            var user = this.mSessionService.GetSession();
            UserModelBase model = new UserModelBase();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new {}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Mapper.Map(this.mUserService.GetUser(id), model);
                if ((user.AccessLevel == AccessLevel.Admin || user.AccessLevel == AccessLevel.AdminAndOwner) && user.Username != model.Username)
                {
                    model.AccessLevel = AccessLevel.CreateRights;
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [ActionName("User")]
        [AjaxAuthorize(Roles = "Administrator,Owner")]
        public ActionResult Update(UserModelBase model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            else
            {
               model = this.mUserService.Update(model);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Albums(UserModelBase model, int start = 0)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            var albums = this.mUserService.GetAlbums(model);
            return Json(albums.PhotoAlbums, JsonRequestBehavior.AllowGet);
        }

        
    }
}
