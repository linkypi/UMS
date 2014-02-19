using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

using Microsoft.Data.Edm;
using APIModel.Models;
using System.Web.SessionState;


namespace UMSMVC
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        //public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //{
        //    filters.Add(new HandleErrorAttribute());
        //}

        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        //    routes.MapRoute(
        //        "Default", // Route name
        //        "{controller}/{action}/{id}/{name}", // URL with parameters
        //        new { controller = "View_BorowAduit", action = "Index", id = UrlParameter.Optional, name = UrlParameter.Optional } // Parameter defaults
        //    );
        //    //routes.Formatters.JsonFormatter.AddQueryStringMapping("$format", "json", "application/json"); config.Formatters.XmlFormatter.AddQueryStringMapping("$format", "xml", "application/xml");

        //    //config.EnableQuerySupport();
        //}

        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();

        //    RegisterGlobalFilters(GlobalFilters.Filters);
        //    RegisterRoutes(RouteTable.Routes);
        //}
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //var route = routes.MapHttpRoute(
            //    "DefaultApi",
            //    "{controller}/{action}/{id}/{name}/",//Report_IMEIInfo
            //    new { controller = "View_BorowAduit", action = "Text", id = UrlParameter.Optional, name = UrlParameter.Optional });
            //route.RouteHandler = new MyHttpControllerRouteHandler();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var route = routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}/{name}",
                defaults: new { controller = "", action = "", id = RouteParameter.Optional, name = RouteParameter.Optional }
            );
            route.RouteHandler = new MyHttpControllerRouteHandler();

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "View_BorowAduit", action = "Get", id = UrlParameter.Optional }
            //);
        }
        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    if (HttpContext.Current.Session != null)
        //    {
        //        //this code is never executed, current session is always null
        //       // HttpContext.Current.Session.Add("__MySessionVariable", new object());
        //    }
        //}
        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // Session is Available here
            //HttpContext context = HttpContext.Current;
            DAL.Sys_LoginInfo log = new DAL.Sys_LoginInfo() { };
            Model.WebReturn r = log.Add(new Model.Sys_UserInfo() { UserName = "1", UserPwd = "1" });
            System.Web.HttpContext.Current.Session["User"] = r.Obj;
            

        }
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();

            //RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterRoutes(RouteTable.Routes);


            //GlobalConfiguration
            //        .Configuration
            //        .Formatters
            //        .Insert(0, new JsonpFormatter());



            //BundleTable.Bundles.RegisterTemplateBundles();




            // Creates the model for our Movies entity set
            //ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            //var entitySetConfiguration = modelBuilder.EntitySet<Report_BorrowAduitInfo>("Report_BorrowAduitInfo");
            //modelBuilder.EntitySet<Report_BorrowAduitInfo>("Report_BorrowAduitInfo1");
            //IEdmModel model = modelBuilder.GetEdmModel();

            //// Adds an OData route with the 'api' prefix. Requests can then be made to /api/Movies for example
            //GlobalConfiguration.Configuration.Routes.MapODataRoute(routeName: "OData", routePrefix: "odata", model: model);
            
            
            //RegisterRoutes(RouteTable.Routes);


            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.MessageHandlers.Add(new MetadataHandler());
            GlobalConfiguration.Configuration.Formatters.RemoveAt(1);

            BundleTable.Bundles.RegisterTemplateBundles();
        }
    }
}