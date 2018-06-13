using AutoMapper;

namespace R3MUS.Devpack.Core.AutoMapper
{
    public interface ICoreMapper
    {
        T Map<T>(object model);
        IMapper MappingEngine();
    }
}