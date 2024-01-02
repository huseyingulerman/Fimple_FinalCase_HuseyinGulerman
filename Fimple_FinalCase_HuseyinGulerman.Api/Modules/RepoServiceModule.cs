using Autofac;
using Fimple_FinalCase_HuseyinGulerman.Core.Repositories;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Fimple_FinalCase_HuseyinGulerman.Core.UnitOfWork;
using Fimple_FinalCase_HuseyinGulerman.Repository.Repositories;
using Fimple_FinalCase_HuseyinGulerman.Repository.UnitOfWork;
using Fimple_FinalCase_HuseyinGulerman.Repository;
using Fimple_FinalCase_HuseyinGulerman.Service.Mapping;
using Fimple_FinalCase_HuseyinGulerman.Service.Services;
using System.Reflection;

namespace Fimple_FinalCase_HuseyinGulerman.Api.Modules
{
    public class RepoServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<,,>)).As(typeof(IService<,,>)).InstancePerLifetimeScope();


            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}
