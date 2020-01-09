using Jux.Data.Redirect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Jux.Helpers
{
    public class UrlHelper
    {
        static bool isPlaylist = false;
        static bool isAlbum = false;
        static bool isSong = false;
        private static string GetUrlFromCopiedText(string CopiedString)
        {
            string txt = CopiedString;

            if(txt.ToLower().Contains("album"))
            {
                isAlbum = true;
            }
            else if (txt.ToLower().Contains("playlist"))
            {
                isPlaylist = true;
            }
            else
            {
                isSong = true;
            }

            string url = "";
            foreach (Match item in Regex.Matches(txt, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
            {
                url = item.Value;
            }

            if (IsUri(url))
            {
                return url;
            }
            else
            {
                return null;
            }

        }
        public static async Task<string> JooxUrl(string RawUrl)
        {
            var getText = RawUrl;
            var url = GetUrlFromCopiedText(getText);
            var finalUrl = "";

            if (isPlaylist && IsUri(url))
            {                    
                finalUrl = await RedirectUrl(url);
                return finalUrl;
            }
            else if (isAlbum && IsUri(url))
            {
                await Task.Run(() => {
                    finalUrl = GetFinalRedirect(url);
                });
                return finalUrl;
            }
            else
            {
                return url;
            }
        }
        private static bool IsUri(string url)
        {
            Uri uriResult;
            bool IsUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return IsUrl;
        }

        private async static Task<string> RedirectUrl(string Url)
        {
            var RedirectorModel = await GetUrls.Url(Url);
            var lastLink = RedirectorModel.Redirects.Select(link => link.RedirectUri).Last();

            return lastLink;
        }
        private static string GetFinalRedirect(string url, bool IsPlaylist =false)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            int maxRedirCount = 0;  // prevent infinite loops
            if (IsPlaylist)
            {
                maxRedirCount = 3;
            }
            string newUrl = url;
            do
            {
                HttpWebRequest req = null;
                HttpWebResponse resp = null;
                try
                {
                    req = (HttpWebRequest)HttpWebRequest.Create(url);
                    req.Method = "HEAD";
                    req.AllowAutoRedirect = false;
                    resp = (HttpWebResponse)req.GetResponse();
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            return newUrl;
                        case HttpStatusCode.Redirect:
                        case HttpStatusCode.MovedPermanently:
                        case HttpStatusCode.RedirectKeepVerb:
                        case HttpStatusCode.RedirectMethod:
                            newUrl = resp.Headers["Location"];
                            if (newUrl == null)
                                return url;

                            if (newUrl.IndexOf("://", System.StringComparison.Ordinal) == -1)
                            {
                                // Doesn't have a URL Schema, meaning it's a relative or absolute URL
                                Uri u = new Uri(new Uri(url), newUrl);
                                newUrl = u.ToString();
                            }
                            break;
                        default:
                            return newUrl;
                    }
                    url = newUrl;
                }
                catch (WebException)
                {
                    // Return the last known good URL
                    return newUrl;
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    if (resp != null)
                        resp.Close();
                }
            } while (maxRedirCount-- > 0);

            return newUrl;
        }
    }
}
