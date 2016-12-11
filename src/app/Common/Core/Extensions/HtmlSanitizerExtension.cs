using System.Text.RegularExpressions;

namespace Clinic.Common.Core.Extensions
{

    /// <summary>
    /// It uses Regular Expressions to simply strip out any tags that do not appear in the white lists.
    /// </summary>
    public static class HtmlSanitizerExtension
    {
        private static Regex _tags = new Regex("<[^>]*(>|$)",
            RegexOptions.Singleline |
            RegexOptions.ExplicitCapture |
            RegexOptions.Compiled);

        private static Regex _whitelist = new Regex(@"
        ^</?(b(lockquote)?|code|d(d|t|l|el)|em|h(1|2|3)|i|kbd|
        li|ol|p(re)?|s(ub|up|trong|trike)?|ul)>$|
        ^<(b|h)r\s?/?>$",
            RegexOptions.Singleline |
            RegexOptions.ExplicitCapture |
            RegexOptions.Compiled |
            RegexOptions.IgnorePatternWhitespace);

        private static Regex _whitelist_a = new Regex(@"
        ^<a\s
        *(\stitle=""[^""<>]+"")?\s
        href=""(\#\d+|(https?|ftp)://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+)""
        (\starget=""[^""<>]*"")?
        (\srel=""nofollow"")?
        (\stitle=""[^""<>]+"")?\s?>$|
        ^</a>$",
            RegexOptions.Singleline |
            RegexOptions.ExplicitCapture |
            RegexOptions.Compiled |
            RegexOptions.IgnorePatternWhitespace);

        private static Regex _whitelist_img = new Regex(@"
        ^<img\s
        src=""https?://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+""
        (\swidth=""\d{1,3}"")?
        (\sheight=""\d{1,3}"")?
        (\salt=""[^""<>]*"")?
        (\stitle=""[^""<>]*"")?
        \s?/?>$",
            RegexOptions.Singleline |
            RegexOptions.ExplicitCapture |
            RegexOptions.Compiled |
            RegexOptions.IgnorePatternWhitespace);

        private const string _blacklist_tag = "</?{0}[^<]*?>";
        private const string _blacklist_attribute = @"(?<=<)([^/>]+)(\s{0}=['""][^'""]+?['""])([^/>]*)(?=/?>|\s)";

        /// <summary>
        /// It uses Regular Expressions to simply strip out any tags that do not appear in the white lists.
        /// </summary>
        public static string AsSafeHtml(this string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            string tagname;
            Match tag;
            var style1 = new Regex("<style>", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            var style2 = new Regex("</style>", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            var tag1 = style1.Matches(html);
            var tag2 = style2.Matches(html);
            if (tag1.Count > 0 && tag2.Count > 0)
            {
                html = html.Remove(tag1[0].Index, tag2[0].Index + tag2[0].Length);
            }

            // match every HTML tag in the input
            MatchCollection tags = _tags.Matches(html);
            for (int i = tags.Count - 1; i > -1; i--)
            {
                tag = tags[i];
                tagname = tag.Value.ToLowerInvariant();

                if (!(_whitelist.IsMatch(tagname) || _whitelist_a.IsMatch(tagname)
                    || _whitelist_img.IsMatch(tagname)
                    ))
                {
                    html = html.Remove(tag.Index, tag.Length);
                }
            }
            html.RemoveHtmlAttribute("style");

            return html;
        }

        /// <summary>
        /// It uses Regular Expressions to simply strip out all tags except the anchor
        /// </summary>
        public static string RemoveAllTagsAndKeepOnlyAnchors(this string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            string tagname;
            Match tag;
            var style1 = new Regex("<style>", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            var style2 = new Regex("</style>", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            var tag1 = style1.Matches(html);
            var tag2 = style2.Matches(html);
            if (tag1.Count > 0 && tag2.Count > 0)
            {
                html = html.Remove(tag1[0].Index, tag2[0].Index + tag2[0].Length);
            }

            // match every HTML tag in the input
            MatchCollection tags = _tags.Matches(html);
            for (int i = tags.Count - 1; i > -1; i--)
            {
                tag = tags[i];
                tagname = tag.Value.ToLowerInvariant();

                if (!_whitelist_a.IsMatch(tagname))
                {
                    html = html.Remove(tag.Index, tag.Length);
                }
            }
            html.RemoveHtmlAttribute("style");

            return html;
        }

        public static string RemoveHtmlAttribute(this string input, string attributeName)
        {
            //if (_validAttributeOrTagNameRegEx.IsMatch(attributeName))
            //{
            Regex reg = new Regex(string.Format(_blacklist_attribute, attributeName),
                   RegexOptions.IgnoreCase);
            return reg.Replace(input, item => item.Groups[1].Value + item.Groups[3].Value);
            //}
            //else
            //{
            //    throw new ArgumentException("Not a valid HTML attribute name", "attributeName");
            //}
        }

        public static string RemoveStyleAttributeParts(this string input, params string[] exceptList)
        {
            // http://stackoverflow.com/questions/12412388/regex-to-remove-all-styles-but-leave-color-and-background-color-if-they-exist
            var expression = @"(<[^>]+\s+)(?:style\s*=\s*""(?!(?:|[^""]*[;\s])text-align\s*:[^"";]*)(?!(?:|[^""]*[;\s])line-height\s*:[^"";]*)[^""]*"" |(style\s*=\s*"") (?=(?:|[^""]*[;\s])(text-align\s*:[^"";]*))? (?=(?:|[^""]*)(;))? (?=(?:|[^""]*[;\s])(line-height\s*:[^"";]*)) ?[^""]*(""))";
            Regex reg = new Regex(expression, RegexOptions.IgnoreCase);
            return reg.Replace(input, item => item.Groups[1].Value + item.Groups[3].Value);
        }

    }
}
