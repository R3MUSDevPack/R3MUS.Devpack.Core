using AutoMapper;

namespace R3MUS.Devpack.Core.AutoMapper
{
    public class CoreMapper : ICoreMapper
    {
        public IMapper MappingEngine()
        {
            return Mapper.Instance;
        }

        public T Map<T>(object model)
        {
            return Mapper.Map<T>(model);
        }
    }
}
