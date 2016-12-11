using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Optimization;
using Clinic.Common.Core.Dates;
using Clinic.Common.Core.Services;

namespace Clinic.UI.Common.Handlers
{
    /// <summary>
    /// Minifies all selected resources and generate a single file for it.
    /// </summary>
    public class ResourcesHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                IBundleTransform minifier;
                string resources = "", response = "";

                var styleSheets = context.Server.UrlDecode(context.Request.QueryString["styleSheets"]);
                var javaScripts = context.Server.UrlDecode(context.Request.QueryString["javaScripts"]);

                if (!string.IsNullOrEmpty(styleSheets))
                {
                    minifier = new CssMinify();
                    resources = styleSheets;
                    context.Response.ContentType = "text/css";
                }
                else if (!string.IsNullOrEmpty(javaScripts))
                {
                    minifier = new JsMinify();
                    resources = javaScripts;
                    context.Response.ContentType = "application/x-javascript";
                }
                else
                {
                    return;
                }

                // Try get from cache
                var cacheKey = GetCacheKey(resources);
                response = (string)HttpRuntime.Cache[cacheKey];

                if (response == null)
                {
                    var files = resources.Split(',');
                    var localFiles = new List<string>();
                    response = GetCombinedResources(files, ref localFiles);

                    // Minify content
                    var bundle = new System.Web.Optimization.BundleResponse(response, null);
                    minifier.Process(new BundleContext(new System.Web.HttpContextWrapper(context), new BundleCollection(), ""), bundle);
                    response = bundle.Content;

                    // Add to cache with dependency on local files
                    var dependency = new CacheDependency(localFiles.ToArray());
                    HttpRuntime.Cache.Insert(cacheKey, response, dependency, DateTime.Now.AddDays(1), TimeSpan.Zero);
                }

                // Write response
                var refresh = new TimeSpan(1, 0, 0, 0);
                context.Response.Clear();
                context.Response.AddHeader("Last-Modified", DateTime.UtcNow.ToStandardFormat(true));
                context.Response.Cache.SetExpires(DateTime.Now.Add(refresh));
                context.Response.Cache.SetMaxAge(refresh);
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.CacheControl = HttpCacheability.Public.ToString();
                context.Response.Cache.SetValidUntilExpires(true);
                GZipEncodeResponse();
                context.Response.Write(response);
                context.Response.Flush();
            }
            catch (Exception exception)
            {
                Logger.Error(this, "Error in ResourcesHandler.", exception);
            }
        }

        /// <summary>
        /// Sets up the current page or handler to use GZip through a Response.Filter
        /// IMPORTANT:  
        /// You have to call this method before any output is generated!
        /// </summary>
        private void GZipEncodeResponse()
        {
            if (IsGZipSupported())
            {
                HttpResponse Response = HttpContext.Current.Response;

                string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
                if (AcceptEncoding.Contains("gzip"))
                {
                    Response.Filter = new System.IO.Compression.GZipStream(Response.Filter,
                                              System.IO.Compression.CompressionMode.Compress);
                    Response.AppendHeader("Content-Encoding", "gzip");
                }
                else
                {
                    Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter,
                                              System.IO.Compression.CompressionMode.Compress);
                    Response.AppendHeader("Content-Encoding", "deflate");
                }
            }
        }
        /// <summary>
        /// Determines if GZip is supported
        /// </summary>
        /// <returns></returns>
        private bool IsGZipSupported()
        {
            string acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding) &&
                 (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate")))
                return true;
            return false;
        }

        protected string GetCacheKey(string key)
        {
            return "ResourcesCombiner-" + key;
        }

        private string GetCombinedResources(string[] files, ref List<string> localFiles)
        {
            var scriptBuilder = new StringBuilder();

            foreach (var filePath in files)
            {
                if (string.IsNullOrEmpty(filePath)) continue;

                string absolutePath = HttpContext.Current.Server.MapPath(filePath);
                if (File.Exists(absolutePath) && filePath.StartsWith("/Content", StringComparison.InvariantCultureIgnoreCase) &&
                    (absolutePath.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase) ||
                     absolutePath.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase)))
                {
                    localFiles.Add(absolutePath);
                    using (var objJsReader = new StreamReader(absolutePath, true))
                    {
                        scriptBuilder.Append(objJsReader.ReadToEnd());
                        scriptBuilder.AppendLine();
                    }
                }
                else
                {
                    if (absolutePath.ToLower().StartsWith("http://") || absolutePath.ToLower().StartsWith("https://"))
                    {
                        var req = (HttpWebRequest)WebRequest.Create(absolutePath);
                        req.KeepAlive = false;
                        req.UseDefaultCredentials = true;

                        var response = (HttpWebResponse)req.GetResponse();
                        Encoding enc = System.Text.Encoding.UTF8;
                        var loResponseStream = new StreamReader(response.GetResponseStream(), enc);
                        string responseString = loResponseStream.ReadToEnd();

                        scriptBuilder.Append(responseString);
                        scriptBuilder.AppendLine();

                        loResponseStream.Close();
                        response.Close();
                    }
                }
            }
            return scriptBuilder.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
