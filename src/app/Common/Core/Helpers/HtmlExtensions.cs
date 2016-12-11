using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using HtmlHelper = System.Web.Mvc.HtmlHelper;

namespace Clinic.Common.Core.Helpers
{
    public static class HtmlExtensions
    {
        public static HtmlString MetaTag(this HtmlHelper html, string name, string value, int priority = 1)
        {
            return MetaTag(html, "name", name, value, priority);
        }

        public static HtmlString MetaTag(this HtmlHelper html, string tagName, string name, string value, int priority = 1)
        {
            if (string.IsNullOrEmpty(tagName) || string.IsNullOrEmpty(name)) return new HtmlString("");

            if (priority == -1 && !string.IsNullOrEmpty(value)) // write in-place
                return new HtmlString(string.Format("<meta {0}='{1}' content='{2}' />\n", tagName, name, value));

            var requiredTags = HttpContext.Current.Items["RequiredTags"] as List<MetaTagInclude>;
            if (requiredTags == null) HttpContext.Current.Items["RequiredTags"] = requiredTags = new List<MetaTagInclude>();
            if (requiredTags.All(i => i.Name != name.ToLower()))
                requiredTags.Add(new MetaTagInclude() { TagName = tagName.ToLower(), Name = name.ToLower(), Value = value.ToLower(), Priority = priority });
            return null;
        }

        public static HtmlString Script(this HtmlHelper html, string path, int priority = 1)
        {
            if (priority == -1) // write in-place
                return new HtmlString(string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>\n", path));

            var requiredScripts = HttpContext.Current.Items["RequiredScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) HttpContext.Current.Items["RequiredScripts"] = requiredScripts = new List<ResourceInclude>();
            if (requiredScripts.All(i => i.Path != path.ToLower())) requiredScripts.Add(new ResourceInclude() { Path = path.ToLower(), Priority = priority });
            return null;
        }

        public static HtmlString RenderScripts(this HtmlHelper html)
        {
            var requiredScripts = HttpContext.Current.Items["RequiredScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) return null;
            var sb = new StringBuilder();
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                foreach (var item in requiredScripts.OrderByDescending(i => i.Priority))
                {
                    sb.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>\n", item.Path);
                }
            }
            else
            {
                sb.Append("<script src=\"/Resources.ashx?javaScripts=");
                foreach (var item in requiredScripts.OrderByDescending(i => i.Priority))
                {
                    sb.Append(item.Path + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("&v=" + DateTime.Now.ToShortDateString());
                sb.Append("&n=15");
                sb.Append("\" type=\"text/javascript\"></script>\n");
            }
            return new HtmlString(sb.ToString());
        }

        public static HtmlString Style(this HtmlHelper html, string path, int priority = 1)
        {
            if (priority == -1) // write in-place
                return new HtmlString(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />\n", path));

            var requiredStyles = HttpContext.Current.Items["RequiredStyles"] as List<ResourceInclude>;
            if (requiredStyles == null) HttpContext.Current.Items["RequiredStyles"] = requiredStyles = new List<ResourceInclude>();
            if (requiredStyles.All(i => i.Path != path.ToLower())) requiredStyles.Add(new ResourceInclude { Path = path.ToLower(), Priority = priority });
            return null;
        }

        public static HtmlString RenderStyles(this HtmlHelper html)
        {
            var requiredStyles = HttpContext.Current.Items["RequiredStyles"] as List<ResourceInclude>;
            if (requiredStyles == null) return null;
            var sb = new StringBuilder();
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                foreach (var item in requiredStyles.OrderByDescending(i => i.Priority))
                {
                    sb.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />\n", item.Path);
                }
            }
            else
            {
                sb.Append("<link href=\"/Resources.ashx?styleSheets=");
                foreach (var item in requiredStyles.OrderByDescending(i => i.Priority))
                {
                    sb.Append(item.Path + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("&v=" + DateTime.Now.ToShortDateString());
                sb.Append("&n=5");
                sb.Append("\" rel=\"stylesheet\" type=\"text/css\" />\n");
            }
            return new HtmlString(sb.ToString());
        }

        public static HtmlString RenderMetaTags(this HtmlHelper html)
        {
            var requiredTags = HttpContext.Current.Items["RequiredTags"] as List<MetaTagInclude>;
            if (requiredTags == null) return null;
            var sb = new StringBuilder();
            foreach (var item in requiredTags.Where(x => !string.IsNullOrWhiteSpace(x.Value)).OrderByDescending(i => i.Priority))
            {
                sb.AppendFormat("<meta {0}='{1}' content='{2}' />\n", item.TagName, item.Name, item.Value);
            }
            return new HtmlString(sb.ToString());
        }


        public static MvcHtmlString EnhancedRoutingLink(this HtmlHelper helper, string linkText, string routeName, object routeValue, string activeClassName)
        {
            ViewContext context = helper.ViewContext;
            IDictionary<string, object> htmlAttributes = null;
            var routeValues = new RouteValueDictionary(routeValue);

            if (IsCurrentRoute(context, routeName, routeValues))
            {
                htmlAttributes = new Dictionary<string, object> { { "class", activeClassName } };
            }
            return new MvcHtmlString(HtmlHelper.GenerateRouteLink(helper.ViewContext.RequestContext, RouteTable.Routes, linkText, routeName, routeValues, htmlAttributes));
        }

        public static MvcHtmlString EnhancedActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues, string activeClassName, LinkType linkType)
        {
            return EnhancedActionLink(helper, linkText, actionName, controllerName, routeValues, activeClassName, linkType, false);
        }

        public static MvcHtmlString EnhancedActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object routeValue, string activeClassName, LinkType linkType, bool keepQueryStrings)
        {
            ViewContext context = helper.ViewContext;
            IDictionary<string, object> htmlAttributes = null;
            var routeValues = new RouteValueDictionary(routeValue);
            if (keepQueryStrings)
                routeValues = new RouteValueDictionary(context.RouteData.Values);

            if (keepQueryStrings && context.RequestContext.HttpContext.Request.QueryString.Keys.Count > 0)
            {
                foreach (string key in context.RequestContext.HttpContext.Request.QueryString.Keys)
                {
                    routeValues[key] = context.RequestContext.HttpContext.Request.QueryString[key];
                }
            }
            if ((IsARegularOrSideLink(linkType) && IsRouteDataMatchCurrentControllerAndAction(context, actionName, controllerName)) ||
                (linkType.Equals(LinkType.TopMenuLink) && context.RouteData.Values["controller"].ToString().ToLower() == controllerName.ToLower()))
            {
                htmlAttributes = new Dictionary<string, object> { { "class", activeClassName } };
            }
            var actionLink = helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            return actionLink;
        }

        public static MvcHtmlString ActionLinkWithHtml(this HtmlHelper helper, string linkText, string actionName, string controllerName, string lang, string activeClassName, string htmlTag, string htmlTagClass)
        {
            ViewContext context = helper.ViewContext;
            TagBuilder divBuilder = new TagBuilder(htmlTag);
            //  divBuilder.InnerHtml = linkText;
            divBuilder.AddCssClass(htmlTagClass);
            if (!IsRouteDataMatchCurrentControllerAndAction(context, actionName, controllerName))
            {
                activeClassName = "";
            }

            return BuildNestedAnchor(divBuilder.ToString(), string.Format("/{0}/{1}/{2}", lang, controllerName, actionName), linkText, activeClassName);
        }


        private static MvcHtmlString BuildNestedAnchor(string innerHtml, string url, string text, object htmlAttributes)
        {
            TagBuilder anchorBuilder = new TagBuilder("a");
            anchorBuilder.Attributes.Add("href", url);
            anchorBuilder.MergeAttributes(new Dictionary<string, object> { { "class", htmlAttributes } });
            anchorBuilder.InnerHtml = innerHtml + text;

            return new MvcHtmlString(anchorBuilder.ToString());
        }

        #region private methods

        private static bool IsCurrentRoute(ControllerContext controllerContext, string routeName, RouteValueDictionary routeValue)
        {
            var r = IsRouteDataMatchCurrentControllerAndAction(controllerContext, routeValue["action"].ToString(), routeValue["controller"].ToString());
            // var route = controllerContext.RouteData.Route == System.Web.Routing.RouteTable.Routes[routeName];
            return r;


        }

        private static bool IsRouteDataMatchCurrentControllerAndAction(ControllerContext context, string actionName, string controllerName)
        {
            return string.Compare(context.RouteData.Values["action"].ToString(), actionName, true) == 0 &&
                   string.Compare(context.RouteData.Values["controller"].ToString(), controllerName, true) == 0;
        }

        private static bool IsARegularOrSideLink(LinkType linkType)
        {
            return (linkType.Equals(LinkType.SideMenuLink) || (linkType.Equals(LinkType.RegularLink)));
        }

        public class ResourceInclude
        {
            public string Path { get; set; }
            public int Priority { get; set; }
        }

        public class MetaTagInclude
        {
            public string TagName { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public int Priority { get; set; }
        }

        public enum LinkType
        {
            TopMenuLink = 1,
            SideMenuLink = 2,
            RegularLink = 3
        }

        #endregion

        public static MvcHtmlString FormValidationSummary(this HtmlHelper html, string formName)
        {
            //var a = html.ViewData.ContainsKey("FormName");
            //var b = html.ViewData["FormName"].ToString();
            if (html.ViewData.ContainsKey("FormName")
                 && (String.Compare(html.ViewData["FormName"].ToString(), formName, StringComparison.OrdinalIgnoreCase) == 0))
            {
                return html.ValidationSummary();
            }

            return MvcHtmlString.Empty;
        }
        /// <summary>Renders a view to string.</summary> 
        public static string RenderPartialToString(this HtmlHelper html, string viewName, object viewData)
        {
            return RenderViewToString(html.ViewContext.Controller.ControllerContext, viewName, viewData);
        }
        /// <summary>Renders a view to string.</summary> 
        public static string RenderViewToString(this Controller controller, string viewName, object viewData)
        {
            return RenderViewToString(controller.ControllerContext, viewName, viewData);
        }

        private static string RenderViewToString(ControllerContext context,
                                                string viewName, object viewData)
        {
            //Create memory writer 
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            //Create fake http context to render the view 
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(HttpContext.Current.Request, fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                context.RouteData, context.Controller);

            var oldContext = HttpContext.Current;
            HttpContext.Current = fakeContext;

            //Use HtmlHelper to render partial view to fake context 
            var html = new HtmlHelper(new ViewContext(fakeControllerContext, new FakeView(), new ViewDataDictionary(), new TempDataDictionary(), memWriter),
                new ViewPage());
            html.RenderPartial(viewName, viewData);

            //Restore context 
            HttpContext.Current = oldContext;

            //Flush memory and return output 
            memWriter.Flush();
            return sb.ToString();
        }

        /// <summary>Fake IView implementation, only used to instantiate an HtmlHelper.</summary> 
        public class FakeView : IView
        {
            #region IView Members
            public void Render(ViewContext viewContext, System.IO.TextWriter writer)
            {
                throw new NotImplementedException();
            }
            #endregion
        }
    }
}
