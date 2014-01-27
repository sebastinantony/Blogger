using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http.Routing;

namespace Blogger.Models
{
    public class ArchivesSet
    {
        public string Month;
        public int Year;
        public int Count;
        public IQueryable<Post> PostList;
    }

    public class BloggerConstants
    {
        public static string LocationFinder = "http://api.hostip.info/get_html.php?ip=";        
    }

    public static class UrlEncoder
    {
        public static string ToFriendlyUrl(string urlToEncode)
        {
            urlToEncode = (urlToEncode ?? "").Trim().ToLower();

            StringBuilder url = new StringBuilder();

            foreach (char ch in urlToEncode)
            {
                switch (ch)
                {
                    case ' ':
                        url.Append('-');
                        break;
                    case '&':
                        url.Append("and");
                        break;
                    case '\'':
                        break;
                    default:
                        if ((ch >= '0' && ch <= '9') ||
                            (ch >= 'a' && ch <= 'z'))
                        {
                            url.Append(ch);
                        }
                        else
                        {
                            url.Append('-');
                        }
                        break;
                }
            }

            return url.ToString();
        }


    }

    public static class Location
    {
        public static string GetContactDetails(string url)
        {
            HttpWebRequest request = null;
            request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string result = reader.ReadToEnd();
            return result;
        }
    }
}