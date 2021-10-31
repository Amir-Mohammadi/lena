using lena.Services.Core.Foundation.Service;
using lena.Services.Internals.Production;
using lena.Services.Internals.Accounting;
using lena.Services.Internals.ApplicationSettings;
using lena.Services.Internals.ApplictaionBase;
using lena.Services.Internals.Guard;
using lena.Services.Internals.Notification;
using lena.Services.Internals.Planning;
using lena.Services.Internals.PrinterManagement;
using lena.Services.Internals.ProjectManagement;
using lena.Services.Internals.Supplies;
using lena.Services.Internals.SaleManagement;
using lena.Services.Internals.ScrumManagement;
using lena.Services.Internals.UserManagement;
using lena.Services.Internals.WarehouseManagement;
using lena.Services.Internals.Reports;
using lena.Services.Internals.QualityControl;
using lena.Services.Internals.Terminal;
using lena.Services.Internals.ReportManagement;
using lena.Services.Internals.Logger;
using lena.Services.Internals.Application;
using lena.Services.Internals.QualityGuarantee;
using lena.Services.Internals.EmployeeAttendance;
using lena.Services.Internals.QualityAssurance;
using lena.Services.Core;
using lena.Services.Core.Provider.DependencyProvider;
using System;
using lena.Services.Core.Provider.Dependency;
using lena.Services.Internals.EmployeeCartable;
namespace lena.Services
{
  public class InternalServiceGroup
  {
    public UserManagement UserManagement => App.Providers.Dependency.GetService<UserManagement>();
    public ApplicationSetting ApplicationSetting => App.Providers.Dependency.GetService<ApplicationSetting>();
    public SaleManagement SaleManagement => App.Providers.Dependency.GetService<SaleManagement>();
    public PrinterManagement PrinterManagment => App.Providers.Dependency.GetService<PrinterManagement>();
    public WarehouseManagement WarehouseManagement => App.Providers.Dependency.GetService<WarehouseManagement>();
    public ApplicationBase ApplicationBase => App.Providers.Dependency.GetService<ApplicationBase>();
    public ApplicationManagement ApplicationManagment => App.Providers.Dependency.GetService<ApplicationManagement>();
    public ScrumManagement ScrumManagement => App.Providers.Dependency.GetService<ScrumManagement>();
    public ProjectManagement ProjectManagement => App.Providers.Dependency.GetService<ProjectManagement>();
    public ReportManagement ReportManagement => App.Providers.Dependency.GetService<ReportManagement>();
    public Planning Planning => App.Providers.Dependency.GetService<Planning>();
    // public EntityConfirmation.Confirmation Confirmation => App.Providers.Dependency.GetService<EntityConfirmation.Confirmation>();
    public QualityAssurance QualityAssurance => App.Providers.Dependency.GetService<QualityAssurance>();
    public QualityControl QualityControl => App.Providers.Dependency.GetService<QualityControl>();
    public Supplies Supplies => App.Providers.Dependency.GetService<Supplies>();
    public Guard Guard => App.Providers.Dependency.GetService<Guard>();
    public Accounting Accounting => App.Providers.Dependency.GetService<Accounting>();
    public Production Production => App.Providers.Dependency.GetService<Production>();
    public Notification Notification => App.Providers.Dependency.GetService<Notification>();
    public TicketManagement TicketManagement => App.Providers.Dependency.GetService<TicketManagement>();
    public TerminalManagement TerminalManagement => App.Providers.Dependency.GetService<TerminalManagement>();
    public Logger Logger => App.Providers.Dependency.GetService<Logger>();
    public QualityGuarantee QualityGuarantee => App.Providers.Dependency.GetService<QualityGuarantee>();
    public EmployeeAttendance EmployeeAttendance => App.Providers.Dependency.GetService<EmployeeAttendance>();
    public EmployeeCartable EmployeeCartable => App.Providers.Dependency.GetService<EmployeeCartable>();
    internal static void RegisterModule(DependencyProvider dependencyProvider)
    {
      dependencyProvider.RegisterType<UserManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<ApplicationSetting>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<SaleManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<PrinterManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<WarehouseManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<ApplicationBase>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<ApplicationManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<ScrumManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<ProjectManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<ReportManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<Planning>(DependencyInjectType.Singleton);
      // dependencyProvider.RegisterType<Internal.EntityConfirmation.Confirmation>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<QualityAssurance>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<QualityControl>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<Supplies>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<Guard>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<Accounting>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<Production>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<Notification>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<TicketManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<TerminalManagement>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<Logger>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<QualityGuarantee>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<EmployeeAttendance>(DependencyInjectType.Singleton);
      dependencyProvider.RegisterType<EmployeeCartable>(DependencyInjectType.Singleton);
    }
  }
}