using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using photoshare.Models;
using photoshare.Interfaces;
using photoshare.Helpers;
using Newtonsoft.Json;
using photoshare.Models.Enums;

namespace photoshare.Controllers
{
    public class AlbumsController : BaseController
    {
        private ISessionService mSessionService;
        private IAlbumService mAlbumService;
        private ITagService mTagService;
        public AlbumsController(ISessionService sessionService, IAlbumService albumService, ITagService tagService)
        {
            this.mSessionService = sessionService;
            this.mAlbumService = albumService;
            this.mTagService = tagService;
        }

        [HttpGet]
        public ActionResult Slideshows(SlideshowModel model)
        {
            model.ShowIndicator = true;
            model.ShowNavigation = true;
            model.Id = "1269aea1-2771-41e6-ac7d-f8e74a0418be";
            PhotoModel p = new PhotoModel()
            { 
                Id = new Guid("1269aea1-2771-41e6-ac7d-f8e74a0418be"),
                FileName = "slide-03.jpg",
                Caption = "Suck it Trebeck"
            };
            model.Items.Add(p);

            PhotoModel p1 = new PhotoModel()
            {
                Id = new Guid("1269aea1-2771-41e6-ac7d-f8e74a0418be"),
                FileName = "slide-02.jpg",
                Caption = "Suck it Trebeck"
            };
            model.Items.Add(p1);


            PhotoModel p2 = new PhotoModel()
            {
                Id = new Guid("1269aea1-2771-41e6-ac7d-f8e74a0418be"),
                FileName = "slide-01.jpg",
                Caption = "Suck it Trebeck"
            };
            model.Items.Add(p2);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxAuthorize]
        public ActionResult Album(PhotoAlbumBaseModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            model = this.mAlbumService.GetAlbum(model.Id);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [AjaxAuthorize]
        [ActionName("Album")]
        public ActionResult DeleteAlbum(PhotoAlbumBaseModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            this.mAlbumService.DeleteAlbum(model.Id, user.Id.Value);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [AjaxAuthorize]
        [ActionName("Album")]
        public ActionResult UpdateAlbum(PhotoAlbumBaseModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            model = this.mAlbumService.UpdateAlbum(model);
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("Album")]
        [AjaxAuthorize]
        public ActionResult CreateAlbum(PhotoAlbumModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            model = this.mAlbumService.CreateAlbum(model);
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxAuthorize]
        public ActionResult Photos(Guid id, int start = 0)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            var album = this.mAlbumService.GetAlbumPhotos(id, user.Id.Value);

            return Json(album.Photos, JsonRequestBehavior.AllowGet);
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

            this.mAlbumService.Favorite(id, user.Id.Value);

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

            this.mAlbumService.UnFavorite(id, user.Id.Value);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxAuthorize]
        public ActionResult Breadcrumb(Guid id)
        {
            var crumbs = this.mAlbumService.Breadcrumbs(id);
            return Json(new { Id = id, Crumbs = crumbs }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Tags(TagModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            model.Type = TagType.Albums;
            var tags = this.mTagService.GetTags(model);
            return Json(new { Available = tags }, JsonRequestBehavior.AllowGet);
        }
    }
}
