namespace R3MUS.Devpack.Core.HttpAbstraction
{
    public interface IHttpWebRequest
    {
        // expose the members you need
        string Method { get; set; }

        IHttpWebResponse GetResponse();
    }
}
