using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Clinic.Common.Core.Extensions
{
    public static class HtmlAgilitySanitizer
    {
        private static readonly IDictionary<string, string[]> Whitelist;
        private static List<string> DeletableNodesXpath = new List<string>();

        static HtmlAgilitySanitizer()
        {
            Whitelist = new Dictionary<string, string[]> {
                { "a", new[] { "href", "title", "target" } },
                { "strong", null },
                { "em", null },
                { "blockquote", null },
                { "b", null},
                { "p", null},
                { "ul", null},
                { "ol", null},
                { "li", null},
                { "div", new[] { "align" } },
                { "strike", null},
                { "u", null},                
                { "sub", null},
                { "sup", null},
                { "table", null },
                { "tr", null },
                { "td", null },
                { "th", null },
                { "h1", null },
                { "h2", null },
                { "h3", null },
                };
        }
        public static string ToYouTubeTitle(this string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            if (html.Length > 50)
                return html.Substring(0, 50);

            return html;
        }

        public static string ToSafeHtml(this string html, bool noFollow = true)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            var output = Sanitize(html);
            if (noFollow)
                output = output.ProcessNoFollow();

            return output;
        }

        public static string ProcessNoFollow(this string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            if (doc.DocumentNode.SelectNodes("//a[@href]") == null) return html;

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if (IsExternalLink(link) &&
                    link.Attributes.Any(x => x.Name == "rel" && x.Value == "nofollow") == false)
                {
                    link.Attributes.Add("rel", "nofollow");
                }
            }
            return doc.DocumentNode.WriteContentTo();
        }

        private static bool IsExternalLink(HtmlNode link)
        {
            Uri result;
            return Uri.TryCreate(link.Attributes["href"].Value, UriKind.Absolute, out result);
        }

        public static string Sanitize(string input)
        {
            if (input.Trim().Length < 1)
                return string.Empty;
            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(input);
            SanitizeNode(htmlDocument.DocumentNode);
            string xPath = HtmlAgilitySanitizer.CreateXPath();

            return StripHtml(htmlDocument.DocumentNode.WriteTo().Trim(), xPath);
        }

        private static void SanitizeChildren(HtmlNode parentNode)
        {
            for (int i = parentNode.ChildNodes.Count - 1; i >= 0; i--)
            {
                SanitizeNode(parentNode.ChildNodes[i]);
            }
        }

        private static void SanitizeNode(HtmlNode node)
        {
            //if (node.NodeType == HtmlNodeType.Comment)
            //{
            //    if (!DeletableNodesXpath.Contains("comment()"))
            //    {
            //        node.Name = "comment()";
            //        DeletableNodesXpath.Add(node.Name);
            //    }
            //}
            if (node.NodeType == HtmlNodeType.Element)
            {
                if (!Whitelist.ContainsKey(node.Name))
                {
                    if (!DeletableNodesXpath.Contains(node.Name))
                    {
                        //DeletableNodesXpath.Add(node.Name.Replace("?",""));
                        node.Name = "removeableNode";
                        DeletableNodesXpath.Add(node.Name);
                    }
                    if (node.HasChildNodes)
                    {
                        SanitizeChildren(node);
                    }

                    return;
                }

                if (node.HasAttributes)
                {
                    for (int i = node.Attributes.Count - 1; i >= 0; i--)
                    {
                        HtmlAttribute currentAttribute = node.Attributes[i];
                        string[] allowedAttributes = Whitelist[node.Name];
                        if (allowedAttributes != null)
                        {
                            if (!allowedAttributes.Contains(currentAttribute.Name))
                            {
                                node.Attributes.Remove(currentAttribute);
                            }
                        }
                        else
                        {
                            node.Attributes.Remove(currentAttribute);
                        }
                    }
                }
            }

            if (node.HasChildNodes)
            {
                SanitizeChildren(node);
            }
        }

        private static string StripHtml(string html, string xPath)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            if (xPath.Length > 0)
            {
                HtmlNodeCollection invalidNodes = htmlDoc.DocumentNode.SelectNodes(@xPath);
                if (invalidNodes != null)
                    foreach (HtmlNode node in invalidNodes)
                    {
                        node.ParentNode.RemoveChild(node, true);
                    }
            }
            return htmlDoc.DocumentNode.WriteContentTo();
        }

        private static string CreateXPath()
        {
            string _xPath = string.Empty;
            for (int i = 0; i < DeletableNodesXpath.Count; i++)
            {
                if (i != DeletableNodesXpath.Count - 1)
                {
                    _xPath += string.Format("//{0}|", DeletableNodesXpath[i].ToString());
                }
                else _xPath += string.Format("//{0}", DeletableNodesXpath[i].ToString());
            }
            return _xPath;
        }
    }
}
