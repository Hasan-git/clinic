using System.Data.Entity;
using Clinic.Common.Core.Caching;
using Clinic.Core.Domain.Repositories;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.Helpers;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Clinic.Infrastructure
{
    public class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAppMicroCache>().To<NativeMicroCache>();
            Bind<DbContext>().To<ClinicContext>();
            Bind<RepositoryFactories>().To<RepositoryFactories>().InSingletonScope();
            Bind<IRepositoryProvider>().To<RepositoryProvider>();
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            //Bind<IMediaItemRepository>().To<MediaItemRepository>();
        }
    }
}
