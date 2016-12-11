using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Clinic.Common.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Compares this string with another while considering 'null' or 'empty' strings to be the same
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns>True if both strings are equal</returns>
        public static bool EqualTo(this string str1, string str2)
        {
            return str1.EqualTo(str2, false);
        }

        /// <summary>
        /// Compares this string with another while considering 'null' or 'empty' strings to be the same
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="caseSensitive"></param>
        /// <returns>True if both strings are equal</returns>
        public static bool EqualTo(this string str1, string str2, bool caseSensitive)
        {
            if (str1 == null && str2 == null)
                return true;
            if (str1 == null)
                return string.Compare(Convert.ToString((object)str1), str2) == 0;
            if (str2 == null)
                return string.Compare(str1, Convert.ToString((object)str2)) == 0;
            return string.Compare(str1, str2, caseSensitive) == 0;
        }

        public static bool NotEqualTo(this string str1, string str2)
        {
            return !str1.EqualTo(str2, false);
        }
        public static bool NotEqualTo(this string str1, string str2, bool caseSensitive)
        {
            return !str1.EqualTo(str2, caseSensitive);
        }
        ///// <summary>
        ///// Trims this text by the specified number of characters
        ///// </summary>
        ///// <returns>The trimmed text</returns>
        //public static string Trim(this string text, int numOfChars)
        //{
        //    if (string.IsNullOrEmpty(text)) return string.Empty;
        //    if (text.Length < numOfChars) return text;
        //    if (numOfChars + 3 >= text.Length) numOfChars = text.Length;
        //    return string.Concat(text.Substring(0, numOfChars), "...");
        //}

        /// <summary>
        /// Trims this text by the specified number of characters
        /// </summary>
        /// <returns>The trimmed text</returns>
        public static string Trim(this string text, int numOfChars, string breakingPhrase = " ")
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (text.Length < numOfChars) return text;
            var finalNumOfChars = text.Substring(0, numOfChars).LastIndexOf(breakingPhrase);

            if (finalNumOfChars == -1 || numOfChars + 4 >= text.Length) finalNumOfChars = text.Length;
            return string.Concat(text.Substring(0, finalNumOfChars), " ...");
        }


        public static bool IsValidEmail(this string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);

            return (false);
        }

        public static bool IsValidEmailAddress(this string emailAddress)
        {
            if ((emailAddress == null) || (emailAddress.Trim().Length <= 0))
            {
                return false;
            }
            string[] ss = emailAddress.Split(new char[] { ';' });
            if (ss.Length <= 0)
            {
                return false;
            }
            foreach (string s_ in ss)
            {
                string s = s_.Trim();
                if (s.Length <= 0)
                {
                    return false;
                }
                int atPos = s.IndexOf("@");
                if ((atPos < 0) || (s.LastIndexOf(".") < (atPos + 1)))
                {
                    return false;
                }
                if (s.IndexOf("@", (int)(atPos + 1)) > atPos)
                {
                    return false;
                }
                if (s.Substring(atPos + 1, 1) == ".")
                {
                    return false;
                }
                if (s.Substring(s.Length - 2).IndexOf(".") > 0)
                {
                    return false;
                }
            }
            return true;
        }


        public static string Clean(this string text)
        {
            return Clean(text, false);
        }
        public static string Clean(this string text, bool removePunctuation)
        {
            return Clean(text, removePunctuation, true, false);
        }

        public static string Clean(this string text, bool removePunctuation, bool removeHtml, bool removeNumbers)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            char[] chars = new char[text.Length];
            int index = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (!char.IsControl(c) &&
                    !(removePunctuation && char.IsPunctuation(c)) &&
                    !(removeNumbers && char.IsNumber(c)))
                {
                    chars[index++] = c;
                }
            }
            var result = new String(chars, 0, index);
            result = RemoveLineEndings(result);
            if (removePunctuation)
                result = result.RemoveDiacritics();
            if (removeHtml)
            {    
                result = result.CleanHtml();
                //result = HttpUtility.HtmlEncode(result);
            }

            return result;
        }
        public static string RemoveLineEndings(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return value.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(lineSeparator, string.Empty).Replace(paragraphSeparator, string.Empty);
        }
        public static string CleanHtml(this string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";
            // remove html
            return Regex.Replace(html, "<[^>]+?>", "").Replace("\r\n", "").Replace("&nbsp;", " ");
            // remove empty lines
            //return Regex.Replace(text, "^$\r\n", "");
        }

        /// <summary>
        /// Capitalizes first character of every word
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string original)
        {
            if (string.IsNullOrEmpty(original)) return "";
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(original);
        }

        public static string CleanSql(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Replace("'", "''");
        }

        public static string RemoveDiacritics(this string stIn)
        {
            return stIn.RemoveArabicDiacritics(true);
        }

        public static string RemoveArabicDiacritics(this string stIn, bool arabic)
        {
            if (string.IsNullOrEmpty(stIn)) return "";
            string stFormD = "";
            if (arabic)
                stFormD = stIn.Normalize(NormalizationForm.FormKC);
            else
                stFormD = stIn.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            return sb.ToString().RemoveLineEndings().Normalize(NormalizationForm.FormC);
        }


        /// <summary>
        /// Extracts the link from an anchor element
        /// </summary>
        public static string ExtractUrlFromAnchor(this string htmlATag)
        {
            string link = "";
            string pattern = @"<a.*?href=[""'](?<link>.*?)[""'].*?>(?<name>.*?)</a>";

            if (Regex.IsMatch(htmlATag, pattern))
            {
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                link = r.Match(htmlATag).Result("${link}");
            }
            return link;
        }


        /// <summary>
        /// Extracts the text from an anchor element
        /// </summary>
        public static string ExtractTextFromAnchor(this string htmlATag)
        {
            string text = "";

            string pattern = @"<a.*?href=[""'](?<link>.*?)[""'].*?>(?<name>.*?)</a>";

            if (Regex.IsMatch(htmlATag, pattern))
            {
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                text = r.Match(htmlATag).Result("${name}");
            }
            return text;
        }

        public static bool ContainsAny(this string str, params string[] values)
        {
            if (!string.IsNullOrEmpty(str) && values.Length > 0)
            {
                return values.Any(value => str.Contains(value));
            }

            return false;
        }

        public static string[] SplitString(this string original, char separator)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(separator)
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
        
        public static bool IsImageFile(this string str)
        {
            var images = new[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };
            if (!string.IsNullOrEmpty(str))
            {
                return images.Any(value => !string.IsNullOrEmpty(value) && str.ToLower().EndsWith(value));
            }

            return false;
        }

        public static bool IsFlashFile(this string str)
        {
            return str.ToLower().EndsWith(".swf");           
        }

        public static bool IsVideoFile(this string str)
        {
            return str.IsVideoFile(".mp4", ".flv");
        }


        public static bool IsVideoFile(this string str, params string[] formats)
        {
            var images = new[] { ".mp4", ".flv" };
            if (!string.IsNullOrEmpty(str))
            {
                return images.Any(value => !string.IsNullOrEmpty(value) && str.ToLower().EndsWith(value)) ||
                       formats.Any(value => !string.IsNullOrEmpty(value) && str.ToLower().EndsWith(value));
            }

            return false;
        }

        public static bool IsAudioFile(this string str)
        {
            return str.ToLower().EndsWith(".mp3");           
            //var images = new[] { ".mp3" };
            //if (!string.IsNullOrEmpty(str))
            //{
            //    return images.Any(value => !string.IsNullOrEmpty(value) && str.ToLower().EndsWith(value));
            //}

            //return false;
        }
        public static bool IsJavascriptFile(this string str)
        {
            return str.ToLower().EndsWith(".js");           
        }
        public static bool IsCssFile(this string str)
        {
            return str.ToLower().EndsWith(".css");
        }
        public static bool IsExecutableFile(this string str)
        {
            var executables = new[] { ".exe", ".bat", ".dll", ".js", ".cmd", ".com", ".vbs", ".msi", ".msp", ".reg", ".wsf"};
            if (!string.IsNullOrEmpty(str))
            {
                return executables.Any(value => !string.IsNullOrEmpty(value) && str.ToLower().EndsWith(value));
            }

            return false;
        }
        public static void EnsureDirectoriesExist(this string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
        }


        public static byte[] AsBytes(this StringBuilder builder)
        {
            return System.Text.Encoding.UTF8.GetBytes(builder.ToString());
        }

        public static byte[] Clone(this byte[] data)
        {
            return data == null ? null : (byte[])data.Clone();
        }

        public static void NewLine(this StringBuilder builder)
        {
            builder.Append(Environment.NewLine);
        }

        #region Sql where
        public static StringBuilder And(this StringBuilder builder, string with)
        {
            builder.Append(" and ").Append(with);
            return builder;
        }

        public static StringBuilder Or(this StringBuilder builder, string with)
        {
            builder.Append(" or ").Append(with);
            return builder;

        }
        public static string And(this string builder, string with)
        {
            return builder + " and " + with;
        }

        public static string Or(this string builder, string with)
        {
            return builder + " or " + with;
        }

        #endregion

        public static string DecodeJson(this string str)
        {
            return str.Replace(@"""", "");
        }

        public static string PadLeft(this string str, int numOfTimes = 1, string paddingString = "")
        {
            if (string.IsNullOrEmpty(str)) return "";
            var builder = new StringBuilder(str.Length + (numOfTimes * paddingString.Length));
            for (int i = 0; i < numOfTimes; i++)
            {
                builder.Append(paddingString);
            }
            builder.Append(str);
            return builder.ToString();
        }
        public static string IsoLanguageName(this string lang)
        {
            switch (lang.ToLower())
            {
                case "ar":
                    return "Ar";
                case "en":
                    return "En";
            }
            return "Ar";
        }
        public static string IsoCultureName(this string lang)
        {
            var language = lang.ToLower();
            switch (language)
            {
                case "ar":
                    return "ar-lb";
                case "en":
                    return "en-us";
            }
            return "ar-lb";
        }

        //return the filetype to tell the browser. 
        //defaults to "application/octet-stream" if it cant find a match, as this works for all file types.
        public static string ContentType(this string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                case ".mp4":
                    return "video/mp4";
                default:
                    return "application/octet-stream";
            }
        }

        /// <summary>
        /// Is this path a web path i.e. starts with 'http://' or 'https://' ?
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true if it is a web path</returns>
        public static bool IsWebPath(this string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return path.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || 
                path.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase);
        }
    }

}
