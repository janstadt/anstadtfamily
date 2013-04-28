using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using photoshare.Models;
using photoshare.Interfaces;
using photoshare.Helpers;
using photoshare.Models.Enums;

namespace photoshare.Controllers
{
    public class PhotosController : BaseController
    {
        private ISessionService mSessionService;
        private IAlbumService mAlbumService;
        private IPhotoService mPhotoService;
        private ITagService mTagService;
        public PhotosController(ISessionService sessionService, IAlbumService albumService, IPhotoService photoService, ITagService tagService)
        {
            this.mSessionService = sessionService;
            this.mAlbumService = albumService;
            this.mPhotoService = photoService;
            this.mTagService = tagService;
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

            this.mPhotoService.DeletePhoto(model.Id, user.Id);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [AjaxAuthorize]
        public ActionResult Favorite(string id)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            this.mPhotoService.Favorite(id, user.Id);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [AjaxAuthorize]
        [ActionName("Photo")]
        public ActionResult UpdatePhoto(PhotoModel photo)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            this.mPhotoService.UpdatePhoto(photo);

            return Json(photo, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [AjaxAuthorize]
        [ActionName("Favorite")]
        public ActionResult UnFavorite(string id)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            this.mPhotoService.UnFavorite(id, user.Id);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxAuthorize]
        public ActionResult Tags(TagModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            model.Type = TagType.Photos;
            var tags = this.mTagService.GetTags(model);
            return Json(new { Available = tags }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Slideshow()
        {
            var model = this.mPhotoService.SlideshowPhotos();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [AjaxAuthorize]
        [ActionName("Slideshow")]
        public ActionResult DeleteSlideshow(PhotoModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            this.mPhotoService.RemoveSlideshowPhoto(model);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAuthorize]
        [ActionName("Slideshow")]
        public ActionResult AddSlideshow(PhotoModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            model.Owner = new Guid(user.Id);
            model.AlbumId = new Guid(user.Id);
            this.mPhotoService.AddSlideshowPhoto(model, this.HttpContext.Request);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
