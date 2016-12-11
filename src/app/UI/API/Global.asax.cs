using System;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Clinic.UI.Common.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Clinic.UI.Common.NinjectWebCommon), "Stop")]
namespace Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           RegisterApis(GlobalConfiguration.Configuration);
        }

        public static void RegisterApis(HttpConfiguration config)
        {
            // Display errors in response locally

            GlobalConfiguration
                   .Configuration
                   .IncludeErrorDetailPolicy =
                    IncludeErrorDetailPolicy.Always;

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Insert(0, new QueryStringMapping("json", "true", "application/json"));

            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
        }


        private void Application_Error(object sender, EventArgs e)
        {
            var url = Request.Url.ToString();
            if (url.EndsWith("favicon.ico") || url.Contains("signalr")) return;
            var lastError = Server.GetLastError();
            if (lastError == null) return;

            var httpException = lastError as HttpException;
            var code = httpException?.GetHttpCode() ?? 500;
            Response.Clear();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            var action = code == 500 ? "Index" : "NotFound";
            routeData.Values.Add("action", action);

            //IController errorController = new ErrorController();
            //errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            //Logger.Fatal(this, "Application global error " + url, lastError);

        }
    }
}
