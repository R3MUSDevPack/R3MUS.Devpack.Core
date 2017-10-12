using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace R3MUS.Devpack.Core.HttpAbstraction
{
    public class HttpWebRequest : IHttpWebRequest
    {
        private readonly System.Net.HttpWebRequest _request;

        public HttpWebRequest(System.Net.HttpWebRequest request)
        {
            _request = request;
            //_request.UserAgent = Assembly.GetExecutingAssembly().FullName;
            //_request.Accept = "application/json";
        }

        public string Host
        {
            get { return _request.Host; }
            set { _request.Host = value; }
        }

        public long ContentLength
        {
            get { return _request.ContentLength; }
            set { _request.ContentLength = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return _request.Headers; }
        }

        public string ContentHeaders
        {
            get { return _request.ContentType; }
            set { _request.ContentType = value; }
        }

        public string Method
        {
            get { return _request.Method; }
            set { _request.Method = value; }
        }

        public Stream GetRequestStream()
        {
            return _request.GetRequestStream();
        }
								
        public IHttpWebResponse GetResponse()
        {
            return new HttpWebResponse((System.Net.HttpWebResponse)_request.GetResponse());
        }
		public IHttpWebResponse GetResponse(byte[] bytes)
        {
            var reqStream = _request.GetRequestStream();
            reqStream.Write(bytes, 0, bytes.Length);
            reqStream.Close();

            return new HttpWebResponse((System.Net.HttpWebResponse)_request.GetResponse());
        }
        public void AddHeader(KeyValuePair<string, string> header)
        {
            if (header.Key == "Content-Type")
            {
                _request.ContentType = header.Value;
            }
            else if(header.Key == "Accept")
            {
                _request.Accept = header.Value;
            }
            else
            {
                _request.Headers[header.Key] = header.Value;
            }
        }
    }

    public class HttpWebResponse : IHttpWebResponse
    {
        private System.Net.HttpWebResponse _response;

        public HttpStatusCode StatusCode { get { return _response.StatusCode; } }

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
