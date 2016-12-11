using System.Linq;
using System.Web.Mvc;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;


namespace Clinic.UI.Common.Filters
{
    public class EnableCorsAttribute : ActionFilterAttribute
    {
        const string Origin = "Origin";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

        public void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.HttpContext.Request.Headers.AllKeys.Contains(Origin))
            {
                string originHeader = actionExecutedContext.HttpContext.Request.Headers[Origin];
                if (!string.IsNullOrEmpty(originHeader))
                {
                    actionExecutedContext.HttpContext.Response.Headers.Add(AccessControlAllowOrigin, originHeader);
                }
            }
        }
    }
}