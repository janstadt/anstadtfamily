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
        public ActionResult Index()
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            var tags = this.mTagService.GetUniqeTags();

            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAuthorize]
        [ActionName("tag")]
        public ActionResult AddTag(TagModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            if (model.Owner == Guid.Empty)
            {
                model.Owner = user.Id;
            }
            if (model.ParentId == Guid.Empty)
            {
                model.ParentId = user.Id;
            }


            //if (model.Id == Guid.Empty)
            //{
                model = this.mTagService.AddTag(model);
            //}
            //else
            //{
            //    model = this.mTagService.UpdateTag(model);
            //}
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [AjaxAuthorize]
        [ActionName("tag")]
        public ActionResult UpdateTag(TagModel model)
        {
            var user = this.mSessionService.GetSession();
            if (user.LoginStatus != Models.Enums.LoginStatus.LoggedIn || user.AccessLevel == Models.Enums.AccessLevel.NoAccess)
            {
                this.HttpContext.Response.StatusCode = 401;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            model = this.mTagService.UpdateTag(model);
           
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpDelete]
        [AjaxAuthorize]
        [ActionName("tag")]
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
