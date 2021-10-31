using System.Reflection;
using Autofac;
using core.Data;
using core.Models;
namespace core.Autofac
{
  public static class AutofacConfigurationExtensions
  {
    public static void AddAutofacDependencyServices(this ContainerBuilder containerBuilder)
    {
      var currentAssembly = Assembly.GetCallingAssembly();
      var coreAssembly = Assembly.GetAssembly(typeof(IEntity));
      containerBuilder.RegisterAssemblyTypes(new[] { currentAssembly, coreAssembly })
                      .AssignableTo<IScopedDependency>()
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();
      containerBuilder.RegisterAssemblyTypes(new[] { currentAssembly, coreAssembly })
                      .AssignableTo<ITransientDependency>()
                      .AsImplementedInterfaces()
                      .InstancePerDependency();
      containerBuilder.RegisterAssemblyTypes(new[] { currentAssembly, coreAssembly })
                      .AssignableTo<ISingletonDependency>()
                      .AsImplementedInterfaces()
                      .SingleInstance();
      //TODO fix sss       
      // containerBuilder.RegisterGeneric(typeof(Repository))
      //                 .As(typeof(IRepository))
      //                 .InstancePerLifetimeScope();
    }
  }
}