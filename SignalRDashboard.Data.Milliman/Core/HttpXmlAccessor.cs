using System.Xml;
using System.Xml.Resolvers;

namespace SignalRDashboard.Data.Milliman.Core
{
    using System;
    using System.IO;
    using System.Net;
    using System.Xml.Linq;

    public class HttpXmlAccessor : IXmlAccessor
    {
        public static XNamespace Xhtml = XNamespace.Get("http://www.w3.org/1999/xhtml");

        public string BaseUrl { get; set; }
        public string UrlPrefix { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public HttpXmlAccessor(string baseUrl, string urlPrefix, string username, string password)
        {
            BaseUrl = baseUrl;
            UrlPrefix = urlPrefix;
            Username = username;
            Password = password;
        }

        public XElement GetXml(string path)
        {
            Uri requestUri = GetRequestUri(path);
            var request = CreateWebRequest(requestUri);

            try
            {
                using (var response = request.GetResponse())
                {
                    var httpResponse = (HttpWebResponse)response;

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var settings = new XmlReaderSettings
                        {
                            DtdProcessing = DtdProcessing.Parse,
                            XmlResolver = new XmlPreloadedResolver(XmlKnownDtds.Xhtml10)
                        };
                        return XDocument.Load(XmlReader.Create(reader, settings)).Root;
                    }
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                if (response != null && ex.Status == WebExceptionStatus.ProtocolError && response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                throw new XmlAccessException("Error occurred attempting to access URI : " + requestUri, ex);
            }
        }

        public XElement GetXml()
        {
            return GetXml(string.Empty);
        }

        private Uri GetRequestUri(string path)
        {
            if (!string.IsNullOrEmpty(UrlPrefix) && !string.IsNullOrEmpty(path))
                return new Uri(new Uri(BaseUrl, UriKind.Absolute), UrlPrefix + path);

            if (string.IsNullOrEmpty(UrlPrefix) && !string.IsNullOrEmpty(path))
                return new Uri(new Uri(BaseUrl, UriKind.Absolute), path);

            if (string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(UrlPrefix))
                return new Uri(new Uri(BaseUrl, UriKind.Absolute), UrlPrefix);

            if (string.IsNullOrEmpty(UrlPrefix) && string.IsNullOrEmpty(path))
                return new Uri(BaseUrl, UriKind.Absolute);

            throw new InvalidOperationException("Cannot formulate request URI");
        }

        internal HttpWebRequest CreateWebRequest(Uri requestUri)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);

            if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Password))
            {
                httpWebRequest.Credentials = new NetworkCredential(Username, Password);
                httpWebRequest.Method = WebRequestMethods.Http.Get;
            }

            return httpWebRequest;
        }
    }
}