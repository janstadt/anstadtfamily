using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using photoshare.Models;
using photoshare.Interfaces;

namespace photoshare.Controllers
{
    public class SecureController : BaseController
    {
        private ISessionService mSessionService;
        public SecureController(ISessionService sessionService)
        {
            this.mSessionService = sessionService;
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Current()
        {
            var model = this.mSessionService.GetSession();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var entity = this.mSessionService.Login(model);
            return Json(entity);
        }

        [HttpDelete]
        [ActionName("Login")]
        public void Logout()
        {
            this.mSessionService.Logout();
        }

        [HttpGet]
        [ActionName("Login")]
        public ActionResult GetSession()
        {
            var model = this.mSessionService.GetHeaderSession();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
