using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using photoshare.Models;
using photoshare.Interfaces;
using photoshare.Helpers;

namespace photoshare.Controllers
{
    public class PhotosController : BaseController
    {
        private ISessionService mSessionService;
        private IAlbumService mAlbumService;
        private IPhotoService mPhotoService;
        public PhotosController(ISessionService sessionService, IAlbumService albumService, IPhotoService photoService)
        {
            this.mSessionService = sessionService;
            this.mAlbumService = albumService;
            this.mPhotoService = photoService;
        }

        [HttpPost]
        [AjaxAuthorize]
        public ActionResult Index(PhotoModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            
            this.mAlbumService.AddPhoto(model, this.HttpContext.Request);
            List<PhotoModel> result = new List<PhotoModel>();
            result.Add(model);
            return Json(new { files = result}, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [AjaxAuthorize]
        [ActionName("Photo")]
        public ActionResult DeletePhoto(PhotoModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            this.mPhotoService.DeletePhoto(model.Id, user.Id.Value);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [AjaxAuthorize]
        public ActionResult Favorite(Guid id)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            this.mPhotoService.Favorite(id, user.Id.Value);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [AjaxAuthorize]
        [ActionName("Favorite")]
        public ActionResult UnFavorite(Guid id)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            this.mPhotoService.UnFavorite(id, user.Id.Value);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}
