using System.Collections.Generic;
using System.IO;
using System.Net;

namespace R3MUS.Devpack.Core.HttpAbstraction
{
    public interface IHttpWebRequest
    {
        // expose the members you need
        string Method { get; set; }
        WebHeaderCollection Headers { get; }
        string ContentHeaders { get; set; }
        long ContentLength { get; set; }

        Stream GetRequestStream();
        IHttpWebResponse GetResponse();
        void AddHeader(KeyValuePair<string, string> header);
    }
}
