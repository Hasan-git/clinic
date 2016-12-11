using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Clinic.UI.Common.Results
{
    public class RssResult : ActionResult
    {
        private readonly SyndicationFeed _feed;

        /// <summary>
        /// Creates a new instance of RssResult
        /// </summary>
        /// <param name="feed">The feed to return the user.</param>
        public RssResult(SyndicationFeed feed)
        //: base("application/rss+xml")
        {
            _feed = feed;
        }

        /// <summary>
        /// Creates a new instance of RssResult
        /// </summary>
        /// <param name="title">The title for the feed.</param>
        /// <param name="feedItems">The items of the feed.</param>
        public RssResult(string title, List<SyndicationItem> feedItems)
        //: base("application/rss+xml")
        {
            _feed = new SyndicationFeed(title, title, HttpContext.Current.Request.Url) { Items = feedItems };
        }


        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.RequestContext.HttpContext.Response;
            response.ContentType = "text/xml";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            if (_feed == null) //check if the feed is null and output an empty string if so
            {
                context.HttpContext.Response.Write(string.Empty);
            }
            else //if the feed is not null, then clear the response and then write the content to it
            {
                //context.HttpContext.Response.ClearContent();
                response.ClearContent();
                //context.HttpContext.Response.Write(_feed.GetRss20Formatter().);
                var settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                using (var writer = XmlWriter.Create(response.Output, settings))
                {
                    _feed.GetRss20Formatter().WriteTo(writer);
                }
            }
        }
    }

}
