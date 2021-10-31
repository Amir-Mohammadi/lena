using lena.Services.Core.Provider.DependencyProvider;
using lena.Domains.Enums;
using Stimulsoft.Base.Json;
using System;

namespace lena.Services.Core.Provider.Dependency
{
  public abstract class DependencyProvider : Provider
  {
    protected DependencyProvider()
    {
    }
    public abstract void RegisterType<T>(DependencyInjectType dependencyInjectType);
    public abstract void RegisterType<T, w>(DependencyInjectType dependencyInjectType);
    public abstract T GetService<T>();
  }
}
