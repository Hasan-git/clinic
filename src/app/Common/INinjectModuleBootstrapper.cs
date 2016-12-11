using System.Collections.Generic;
using Ninject.Modules;

namespace Clinic.Common
{
    public interface INinjectModuleBootstrapper
    {
        IList<INinjectModule> GetModules();
    }
}
