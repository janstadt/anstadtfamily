using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using photoshare.Helpers;
using photoshare.Interfaces;
using photoshare.Models;
using photoshare.Models.Enums;
namespace photoshare.Controllers
{
    public class HomeController : BaseController
    {
        private ISessionService mSessionService;
        public HomeController(ISessionService sessionService)
        {
            this.mSessionService = sessionService;
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AjaxAuthorize]
        public ActionResult Photos()
        {
            return Json(new { });
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string ReturnUrl)
        {
            var response = this.mSessionService.Login(model);

            if (response.LoginStatus != LoginStatus.LoggedIn)
            {
                return View("Login", model);
            }
            else
            {
                ReturnUrl = string.IsNullOrWhiteSpace(ReturnUrl) ? "/" : ReturnUrl;
                return Redirect(ReturnUrl);
            }
        }
    }
}
