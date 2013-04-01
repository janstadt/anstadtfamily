using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using photoshare.Models;
using photoshare.Interfaces;
using photoshare.Helpers;
using AutoMapper;

namespace photoshare.Controllers
{
    [AjaxAuthorize]
    public class UserController : BaseController
    {
        private ISessionService mSessionService;
        private IUserService mUserService;
        public UserController(ISessionService sessionService, IUserService userService)
        {
            this.mSessionService = sessionService;
            this.mUserService = userService;
        }

        [HttpGet]
        public ActionResult Info(Guid id)
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
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [ActionName("Info")]
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
