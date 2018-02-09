using Newtonsoft.Json;
using R3MUS.Devpack.Core.Enums;
using R3MUS.Devpack.Core.HttpAbstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace R3MUS.Devpack.Core
{
    public class Web
    {
        public static string BaseRequest(string uri, List<KeyValuePair<string, string>> headers = null)
        {
            var request = new HttpWebRequestFactory().Create(uri);
            if (headers != null)
            {
                headers.ForEach(h => {
                    if (h.Key.ToLower().Equals("authorization"))
                    {
                        request.Headers[System.Net.HttpRequestHeader.Authorization] = h.Value;
                    }
                    else
                    {
                        request.Headers.Add(h.Key, h.Value);
                    }
                });
            }

            var responseStream = request.GetResponse().GetResponseStream();
            using (var reader = new StreamReader(responseStream))
            {
                return reader.ReadToEnd();
            }
        }

        public static T Post<T>(string uri, Dictionary<string, string> headers, object postData)
        {
            string dataStr;

            if(postData.GetType() != typeof(string))
            {
                dataStr = JsonConvert.SerializeObject(postData);
            }
            else
            {
                dataStr = (string)postData;
            }

            var data = Encoding.ASCII.GetBytes(dataStr);
            string result;

            var request = new HttpWebRequestFactory().Create(uri);
            request.Method = Enum.GetName(typeof(Method), Method.POST);
            request.ContentHeaders = "application/json";
            request.ContentLength = data.Length;

            foreach (var header in headers)
            {
                request.AddHeader(header);
            }
            var nvc = (System.Collections.Specialized.NameValueCollection)request.Headers;
            var keys = nvc.AllKeys;
            foreach (var key in keys)
            {
                var values = nvc.GetValues(key);
                foreach (var value in values)
                {
                    System.Diagnostics.Debug.WriteLine(string.Concat(key, ": ", value));
                }
            }

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
            {
                result = reader.ReadToEnd();
            }
            
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
