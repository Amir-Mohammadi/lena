// using lena.Services.Core.Foundation.Service;
// using lena.Services.Externals.Accounting;
// using lena.Services.Externals.ApplicationBase;
// using lena.Services.Externals.Guard;
// using lena.Services.Externals.Notification;
// using lena.Services.Externals.Planning;
// using lena.Services.Externals.QualityControl;
// using lena.Services.Externals.Printers;
// using lena.Services.Externals.Production;
// using lena.Services.Externals.ProjectManagement;
// using lena.Services.Externals.SaleManagement;
// using lena.Services.Externals.ScrumManagement;
// using lena.Services.Externals.Supplies;
// using lena.Services.Externals.UserManagement;
// using lena.Services.Externals.WarehouseManagement;
// using lena.Services.Externals.ApplicationSettings;
// using lena.Services.Externals.Terminals;
// using lena.Services.Externals.Reports;
// using lena.Services.Externals.EntityConfirmation;
// using lena.Services.Externals.Storage;
// using lena.Services.Externals.Application;
// using lena.Services.Externals.QualityAssurance;
// using lena.Services.Externals.QualityGuarantee;
// using lena.Services.Core;
// using lena.Services.Core.Provider.DependencyProvider;
// using lena.Services.Externals.Logger;
// using System;
// using lena.Services.Core.Provider.Dependency;
// using lena.Services.Externals.EmployeeAttendance;
// using lena.Services.Externals.EmployeeCartable;
// namespace lena.Services
// {
//   public class ExternalServiceGroup
//   {
//     public UserManagement UserManagement => App.Providers.Dependency.GetService<UserManagement>();
//     public ApplicationManagement ApplicationManagment => App.Providers.Dependency.GetService<ApplicationManagement>();
//     public ApplicationBase ApplicationBase => App.Providers.Dependency.GetService<ApplicationBase>();
//     public ApplicationSettings ApplicationSetting => App.Providers.Dependency.GetService<ApplicationSettings>();
//     public SaleManagement SaleManagement => App.Providers.Dependency.GetService<SaleManagement>();
//     public Planning Planning => App.Providers.Dependency.GetService<Planning>();
//     public QualityAssurance QualityAssurance => App.Providers.Dependency.GetService<QualityAssurance>();
//     public QualityControl QualityControl => App.Providers.Dependency.GetService<QualityControl>();
//     public Supplies Supplies => App.Providers.Dependency.GetService<Supplies>();
//     public Confirmation Confirmatios => App.Providers.Dependency.GetService<Confirmation>();
//     public PrinterManagment PrinterManagment => App.Providers.Dependency.GetService<PrinterManagment>();
//     public WarehouseManagement WarehouseManagement => App.Providers.Dependency.GetService<WarehouseManagement>();
//     public ProjectManagement ProjectManagement => App.Providers.Dependency.GetService<ProjectManagement>();
//     public ReportManagement ReportManagement => App.Providers.Dependency.GetService<ReportManagement>();
//     public ScrumManagement ScrumManagement => App.Providers.Dependency.GetService<ScrumManagement>();
//     public Guard Guard => App.Providers.Dependency.GetService<Guard>();
//     public Accounting Accounting => App.Providers.Dependency.GetService<Accounting>();
//     public Production Production => App.Providers.Dependency.GetService<Production>();
//     public Notification Notification => App.Providers.Dependency.GetService<Notification>();
//     public Terminal Terminal => App.Providers.Dependency.GetService<Terminal>();
//     public TicketManagement TicketManagement => App.Providers.Dependency.GetService<TicketManagement>();
//     public Logger Logger => App.Providers.Dependency.GetService<Logger>();
//     public Storage Storage => App.Providers.Dependency.GetService<External.Storage.Storage>();
//     public QualityGuarantee QualityGuarantee => App.Providers.Dependency.GetService<QualityGuarantee>();
//     public EmployeeAttendance EmployeeAttendance => App.Providers.Dependency.GetService<EmployeeAttendance>();
//     public EmployeeCartable EmployeeCartable => App.Providers.Dependency.GetService<EmployeeCartable>();
//     internal static void RegisterModule(DependencyProvider dependencyProvider)
//     {
//       dependencyProvider.RegisterType<UserManagement>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<ApplicationManagement>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<ApplicationBase>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<ApplicationSettings>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<SaleManagement>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Planning>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<QualityAssurance>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<QualityControl>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Supplies>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Confirmation>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<PrinterManagment>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<WarehouseManagement>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<ProjectManagement>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<ReportManagement>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<ScrumManagement>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Guard>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Accounting>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Production>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Notification>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Terminal>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<TicketManagement>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Logger>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<Storage>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<QualityGuarantee>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<EmployeeAttendance>(DependencyInjectType.Singleton);
//       dependencyProvider.RegisterType<EmployeeCartable>(DependencyInjectType.Singleton);
//     }
//   }
// }