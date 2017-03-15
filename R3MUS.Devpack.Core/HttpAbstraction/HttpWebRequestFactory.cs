using System.Net;

namespace R3MUS.Devpack.Core.HttpAbstraction
{
    public class HttpWebRequestFactory : IHttpWebRequestFactory
    {
        public IHttpWebRequest Create(string uri)
        {
            return new HttpWebRequest((System.Net.HttpWebRequest)WebRequest.Create(uri));
        }
    }
}
