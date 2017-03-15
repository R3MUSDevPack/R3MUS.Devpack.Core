using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace R3MUS.Devpack.Core.HttpAbstraction
{
    public class HttpWebRequest : IHttpWebRequest
    {
        private readonly System.Net.HttpWebRequest _request;

        public HttpWebRequest(System.Net.HttpWebRequest request)
        {
            _request = request;
            _request.UserAgent = Assembly.GetExecutingAssembly().FullName;
        }

        public string Method
        {
            get { return _request.Method; }
            set { _request.Method = value; }
        }

        public IHttpWebResponse GetResponse()
        {
            return new HttpWebResponse((System.Net.HttpWebResponse)_request.GetResponse());
        }
    }

    public class HttpWebResponse : IHttpWebResponse
    {
        private WebResponse _response;

        public HttpWebResponse(System.Net.HttpWebResponse response)
        {
            _response = response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_response != null)
                {
                    ((IDisposable)_response).Dispose();
                    _response = null;
                }
            }
        }

        public virtual Stream GetResponseStream()
        {
            return _response.GetResponseStream();
        }
    }
}
