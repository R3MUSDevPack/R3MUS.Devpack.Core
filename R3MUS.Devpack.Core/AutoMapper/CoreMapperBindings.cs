using AutoMapper;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MUS.Devpack.Core.AutoMapper
{
    public class CoreMapperBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ICoreMapper>().To<CoreMapper>();
            Rebind<IMapper>().ToMethod(ctxt => ctxt.Kernel.Get<ICoreMapper>().MappingEngine());
        }
    }
}
