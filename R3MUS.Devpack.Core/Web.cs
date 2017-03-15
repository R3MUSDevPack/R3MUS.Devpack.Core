using R3MUS.Devpack.Core.HttpAbstraction;
using System.IO;

namespace R3MUS.Devpack.Core
{
    public class Web
    {
        public static string BaseRequest(string uri)
        {
            var request = new HttpWebRequestFactory().Create(uri);
            var responseStream = request.GetResponse().GetResponseStream();
            using (var reader = new StreamReader(responseStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
