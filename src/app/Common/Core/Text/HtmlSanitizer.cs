using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Clinic.Common.Core.Text
{
    public interface IHtmlSanitizer
    {
        string Sanitize(string html);
        string RemoveAllTags(string html);
    }

    public class HtmlSanitizer : IHtmlSanitizer
    {
        public string Sanitize(string html)
        {
            StringBuilder htmlBuilder = new StringBuilder(html);
            MatchCollection tags = Tags.Matches(html);

            for (int i = tags.Count - 1; i >= 0; i--)
            {
                bool invalid = false;
                Match tag = tags[i];
                string tagName = tag.Groups["name"].Value.ToLowerInvariant();

                if (AllowedTagNames.IsMatch(tagName))
                {
                    MatchCollection attrs = Attributes.Matches(tag.Value.Trim());
                    foreach (Match attr in attrs)
                    {
                        string attrKey = attr.Groups["key"].Value.ToLowerInvariant();
                        string attrValue = attr.Groups["value"].Value.Trim();
                        if (AllowedAttributeNames[tagName].IsMatch(attrKey))
                        {
                            if (attrKey == "style")
                            {
                                MatchCollection declarations = Styles.Matches(attrValue);

                                foreach (Match d in declarations)
                                {
                                    string property = d.Groups["property"].Value.ToLowerInvariant();
                                    string value = d.Groups["value"].Value.Trim();

                                    if (!AllowedStyleProperties.IsMatch(property) || !AllowedStylePropertyValues[property].IsMatch(value))
                                    {
                                        invalid = true;
                                        break;
                                    }
                                }
                                if (invalid) break;
                            }
                            else
                            {
                                if (!AllowedAttributeValues[attrKey].IsMatch(attrValue))
                                {
                                    invalid = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            invalid = true;
                            break;
                        }
                    }
                }
                else
                {
                    invalid = true;
                }

                if (invalid)
                {
                    htmlBuilder.Remove(tag.Index, tag.Length);
                }
            }

            return htmlBuilder.ToString();
        }

        public string RemoveAllTags(string html)
        {
            StringBuilder htmlBuilder = new StringBuilder(html);
            MatchCollection tags = Tags.Matches(html);

            for (int i = tags.Count - 1; i >= 0; i--)
            {
                htmlBuilder.Remove(tags[i].Index, tags[i].Length);
            }

            return htmlBuilder.ToString();
        }

        public Regex Tags { get { return tags; } }
        public Regex AllowedTagNames { get { return allowTagNames; } }
        public Regex TagOnly { get { return tagOnly; } }
        public Regex AttributeAllowedTagNames { get { return attrAllowedTagNames; } }
        public Regex Attributes { get { return attributes; } }
        public Regex Styles { get { return styles; } }
        public Regex AllowedStyleProperties { get { return allowedStyleProperties; } }
        public Dictionary<string, Regex> AllowedAttributeNames { get { return allowedAttributeNames; } }
        public Dictionary<string, Regex> AllowedAttributeValues { get { return allowedAttributeValues; } }
        public Dictionary<string, Regex> AllowedStylePropertyValues { get { return allowedStylePropertyValues; } }

        private static Regex tags;
        private static Regex allowTagNames;
        private static Regex tagOnly;
        private static Regex attrAllowedTagNames;
        private static Regex attributes;
        private static Regex styles;
        private static Regex allowedStyleProperties;
        private static Dictionary<string, Regex> allowedAttributeNames;
        private static Dictionary<string, Regex> allowedAttributeValues;
        private static Dictionary<string, Regex> allowedStylePropertyValues;

        static HtmlSanitizer()
        {
            string allowedTagList = @"(a|b(lockquote|r)?|code|d(d|iv|t|l|el)|em|h[1-6]|i(mg)?|kbd|li|ol|p(re)?|s(ub|up|trong|trike|pan)?|t(able|body|d|h|r)|u(l)?)";
            RegexOptions regexOption = RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled;

            tags = new Regex(@"<+/?(?<name>[a-zA-Z0-9]+)[^>]*?(>+|$)", regexOption);

            allowTagNames = new Regex(@"^" + allowedTagList + "$", regexOption);

            tagOnly = new Regex(@"(</" + allowedTagList + @"\s*>)|" +
                @"(<(b(lockquote)?|code|d(d|t|l|el)|em|h[1-6]|i|kbd|s(ub|up|trong|trike)?|t(body|r)|u)\s*>)|" +
                @"<br\s*/?>", regexOption);

            // attributes

            attrAllowedTagNames = new Regex("^(a|b(lockquote|r)|img|pre|t(able|d|h|r)|span|ul)$", regexOption);

            attributes = new Regex(@"\s*(?<key>[a-zA-Z]+)\s*=\s*(""(?<value>[^""]*)""|(?<value>[^\s]*))", regexOption);

            allowedAttributeNames = new Dictionary<string, Regex>();
            allowedAttributeNames["a"] = new Regex(@"href|title", regexOption);
            allowedAttributeNames["img"] = new Regex(@"alt|src|width|height", regexOption);
            allowedAttributeNames["table"] = new Regex("border|c(lass|ell(spacing|padding))|style", regexOption);

            Regex classAndStyle = new Regex("class|style", regexOption);
            allowedAttributeNames["td"] = classAndStyle;
            allowedAttributeNames["span"] = classAndStyle;
            allowedAttributeNames["pre"] = classAndStyle;
            allowedAttributeNames["br"] = classAndStyle;

            Regex classOnly = new Regex("class", regexOption);
            allowedAttributeNames["blockquote"] = classOnly;
            allowedAttributeNames["ul"] = classOnly;

            allowedAttributeValues = new Dictionary<string, Regex>();

            Regex noRestrict = new Regex(@"^[^""]*$", regexOption);
            allowedAttributeValues["alt"] = noRestrict;
            allowedAttributeValues["title"] = noRestrict;
            allowedAttributeValues["class"] = noRestrict;

            Regex number = new Regex(@"^\d{1,3}$", regexOption);
            allowedAttributeValues["width"] = number;
            allowedAttributeValues["height"] = number;

            allowedAttributeValues["href"] = new Regex(@"^\s*(\#\d*|((https?|ftp)://|/)[-a-zA-Z0-9+&@#/%?=~_|!:,.;\(\)]+)\s*$", regexOption);
            allowedAttributeValues["src"] = new Regex(@"^\s*(https?://|/)[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+\s*$", regexOption);

            Regex number2 = new Regex(@"^\d$", regexOption);
            allowedAttributeValues["border"] = number2;
            allowedAttributeValues["cellspacing"] = number2;
            allowedAttributeValues["cellpadding"] = number2;


            //style tag

            styles = new Regex(@"\s*(?<property>[-_A-Za-z]*)\s*:\s*(?<value>[^"";]*)(;|\s*$)", regexOption);

            allowedStyleProperties = new Regex(@"^((b(ackground|order)-)?color|background|border-width|font-(family|s(ize|tyle)|weight)|line-height|margin|padding|text-(align|decoration))$", regexOption);

            allowedStylePropertyValues = new Dictionary<string, Regex>();

            Regex anyString = new Regex(@"^.+$", regexOption);
            allowedStylePropertyValues["background-color"] = anyString;
            allowedStylePropertyValues["border-color"] = anyString;
            allowedStylePropertyValues["color"] = anyString;
            allowedStylePropertyValues["background"] = anyString;
            allowedStylePropertyValues["font-weight"] = anyString;
            allowedStylePropertyValues["font-style"] = anyString;
            allowedStylePropertyValues["text-decoration"] = anyString;


            Regex width = new Regex(@"^\dpx$", regexOption);
            allowedStylePropertyValues["border-width"] = width;

            Regex box = new Regex(@"^(\s*\dpx\s*){1,4}$", regexOption);
            allowedStylePropertyValues["margin"] = box;
            allowedStylePropertyValues["padding"] = box;

            allowedStylePropertyValues["font-family"] = new Regex(@"^[^""]*$", regexOption);
            allowedStylePropertyValues["font-size"] = new Regex(@"^\d\d?(pt)?$", regexOption);

            allowedStylePropertyValues["line-height"] = new Regex(@"^[1-9](\.\d+)?$", regexOption);
            allowedStylePropertyValues["text-align"] = new Regex(@"^(left|right|center)$", regexOption);
        }
    }
}
