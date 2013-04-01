using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data;
using vdz.ca.Mappers;
using vdz.ca.IoC;
using photoshare.Interfaces;
using Microsoft.Practices.Unity;

namespace photoshare
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "API Routes",
                "api/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional, action = "Index" }
            );
            routes.MapRoute(
                 "Login", // Route name
                "Login/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Login", id = UrlParameter.Optional, ReturnUrl = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (Context.Items["AjaxPermissionDenied"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            SetupIoC();
            Mappers.Register();
        }

        private void SetupIoC()
        {
            IUnityContainer container = new UnityContainer();
            IoC.Register(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            //ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            //logger.Error(DateTime.Now, exception);
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }

            //IController errorsController = new ErrorController();
            //var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            //errorsController.Execute(rc);
        }
    }
}