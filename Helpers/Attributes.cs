using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace photoshare.Helpers
{
    public class AjaxAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
            }
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}