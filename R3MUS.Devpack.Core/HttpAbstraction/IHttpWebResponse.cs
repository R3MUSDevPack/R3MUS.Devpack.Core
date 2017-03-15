using System;
using System.IO;

namespace R3MUS.Devpack.Core.HttpAbstraction
{
    public interface IHttpWebResponse : IDisposable
    {
        // expose the members you need
        Stream GetResponseStream();
    }
}
