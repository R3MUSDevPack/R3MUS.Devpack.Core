using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MUS.Devpack.Core.HttpAbstraction
{
    public interface IHttpWebRequestFactory
    {
        IHttpWebRequest Create(string uri);
    }
}
