using System;
using lena.Services.Core.Foundation.Service;
using lena.Services.Core.Provider.Dependency;
using lena.Services.Core.Provider.DependencyProvider;
using lena.Services.Core.Provider.Logger;
using lena.Services.Core.Provider.Notification;
using lena.Services.Core.Provider.RequestInfo;
using lena.Services.Core.Provider.Security;
using lena.Services.Core.Provider.Session;
using lena.Services.Core.Provider.Storage;
using lena.Services.Core.Provider.Transaction;
using lena.Domains.Enums;
namespace lena.Services.Core
{
  public class ProviderGroup
  {

    private static DependencyProvider dependencyProvider;
    public DependencyProvider Dependency => ProviderGroup.dependencyProvider;
    public SessionProvider Session => this.Dependency.GetService<SessionProvider>();
    public NotificationProvider Notification => this.Dependency.GetService<NotificationProvider>();
    public SecurityProvider Security => this.Dependency.GetService<SecurityProvider>();
    public Storage Storage => this.Dependency.GetService<Storage>();
    public LoggerProvider Logger => this.Dependency.GetService<LoggerProvider>();
    public RequestInfoProvider Request => this.Dependency.GetService<RequestInfoProvider>();
    public UncommitedTransactionAgent UncommitedTransactionAgent => this.Dependency.GetService<UncommitedTransactionAgent>();
    public PersistentLogger PersistentLogger => this.Dependency.GetService<PersistentLogger>();
    public Diagnostics Diagnostics => this.Dependency.GetService<Diagnostics>();

    internal static void RegisterModule(DependencyProvider dependencyProvider)
    {
      ProviderGroup.dependencyProvider = dependencyProvider;
      dependencyProvider.RegisterType<NotificationProvider>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<SecurityProvider>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<Storage>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<UncommitedTransactionAgent>(DependencyInjectType.Transient);
      dependencyProvider.RegisterType<PersistentLogger>(DependencyInjectType.Transient);
      dependencyProvider.RegisterType<Diagnostics>(DependencyInjectType.Transient);
    }
  }
}