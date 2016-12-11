using System.Web.Mvc;
using Clinic.Common.Core.Services;

namespace Clinic.UI.Common.Filters
{
    public class HandleErrorAndLogItAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Logger.Error(filterContext.Controller, "Unhandled error", filterContext.Exception);

            filterContext.ExceptionHandled = true;
        }
    }
}
