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
    public class TagsController : BaseController
    {
        private ISessionService mSessionService;
        private ITagService mTagService;
        public TagsController(ISessionService sessionService, ITagService tagService)
        {
            this.mSessionService = sessionService;
            this.mTagService = tagService;
        }
       
        [HttpGet]
        [AjaxAuthorize]
        public ActionResult Index(TagModel model)
        {
            //var user = this.mSessionService.GetSession();
            //if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            //{
            //    this.HttpContext.Response.StatusCode = 401;
            //    return Json(new { }, JsonRequestBehavior.AllowGet);
            //}

            //this.mPhotoService.DeletePhoto(model.Id, user.Id.Value);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxAuthorize]
        public ActionResult Photos(TagModel model)
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
        [AjaxAuthorize]
        public ActionResult Albums(TagModel model)
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

        [HttpPost]
        [AjaxAuthorize]
        [ActionName("Index")]
        public ActionResult AddTag(TagModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            model = this.mTagService.AddTag(model);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpPut]
        [AjaxAuthorize]
        [ActionName("Index")]
        public ActionResult DeleteTag(TagModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            //this.mPhotoService.DeletePhoto(model.Id, user.Id.Value);
            this.mTagService.DeleteTag(model);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
