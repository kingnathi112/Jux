using System.Collections.Generic;

namespace Jux.Models.RedirectUrlModel
{
    public class RedirectorModel
    {
        public string RequestedURL { get; set; }
        public string NumberOfRedirects { get; set; }
        public List<Redirect> Redirects { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}
