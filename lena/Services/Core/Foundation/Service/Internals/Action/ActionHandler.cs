// using System;
// using lena.Services.Common;
// //using lena.Services.Core.Foundation.Service.Internal.Action;
// using lena.Models.Common;
// using System.Diagnostics;
// using System.Linq;
// using lena.Services.Core.Exceptions;
// namespace lena.Services.Core.Foundation.Action
// {
//   public class ActionHandler<TReturn> : async Task<TReturn>, async Task, IAction, IAction<TReturn>
//   {
//     protected async TaskOfWork UnitOfWork { get; set; } = null;
//     protected Func<IRepository, TReturn> Task { private get; set; }
//     public ActionHandler(Func<IRepository, TReturn> task)
//     {
//       Task = task;
//     }
//     protected IAction<TReturn> With(IUnitOfWork unitOfWork)
//     {
//       UnitOfWork = unitOfWork;
//       return this;
//     }
//     TReturn IAction<TReturn>
//     {
//       return RunInternally();
//     }
//     private TReturn RunInternally()
//     {
//       var stackTrace = new StackTrace();
//       var repository = DALFactory.CreateRepository(UnitOfWork);
//       repository.OnAdd((e) =>
//       {
//         if (App.Providers.PersistentLogger != null)
//           App.Providers.PersistentLogger.Steps.Add(new Provider.Logger.PersistentLogStep(
//                     Provider.Logger.PersistentLogStepType.Info, new
//                     {
//                       ActionType = "Add",
//                       Entity = e.GetType().FullName,
//                       item = e
//                     }, 2)
//                 );
//       });
//       repository.OnUpdate((e) =>
//       {
//         if (App.Providers.PersistentLogger != null)
//           App.Providers.PersistentLogger.Steps.Add(new Provider.Logger.PersistentLogStep(
//                 Provider.Logger.PersistentLogStepType.Warn, new
//                 {
//                   ActionType = "Update",
//                   Entity = e.GetType().FullName,
//                   item = e
//                 }, 2)
//             );
//       });
//       repository.OnDelete((e) =>
//       {
//         App.Providers.PersistentLogger.Steps.Add(new Provider.Logger.PersistentLogStep(
//                 Provider.Logger.PersistentLogStepType.Warn, new
//                 {
//                   ActionType = "Delete",
//                   Entity = e.GetType().FullName,
//                   item = e
//                 }, 2)
//             );
//       });
//       repository.OnQuery((e) =>
//       {
//         App.Providers.PersistentLogger.Steps.Add(new Provider.Logger.PersistentLogStep(
//                 Provider.Logger.PersistentLogStepType.Log, new
//                 {
//                   ActionType = "Query",
//                   Entity = e.GetType().FullName,
//                   item = e
//                 }, 2)
//             );
//       });
//       var transactionName = "NO_REQUEST";
//       if (App.Providers.Request != null)
//       {
//         transactionName = App.Providers.Request.ClientAddress + ":" + App.Providers.Request.Url;
//       }
//       repository.UnitOfWork.BeginTransaction(transactionName);
//       TReturn result;
//       var frames = stackTrace.GetFrames();
//       //if (repository.UnitOfWork.IsLocalTransaction)
//       {
//         try
//         {
//           result = Task.Invoke(repository);
//           //repository.UnitOfWork.SaveChanges();
//           repository.UnitOfWork.CommitTransaction();
//         }
//         catch (DbUpdateConcurrencyException ex)
//         {
//           repository.UnitOfWork.RollBackTransaction();
//           throw new InternalDbUpdateConcurrencyException(ex.Message);
//           //After throw it go to next catch !!! and we use when
//         }
//         catch (Exception ex) when (!(ex is InternalDbUpdateConcurrencyException))
//         {
//           repository.UnitOfWork.RollBackTransaction();
//           throw ex;
//         }
//       }
//       //else
//       //{
//       //    result = Task.Invoke(repository);
//       //    //repository.UnitOfWork.SaveChanges();
//       //    repository.UnitOfWork.CommitTransaction();
//       //}
//       return result;
//     }
//     IAction<TReturn> async Task<TReturn>.With(IUnitOfWork unitOfWork)
//     {
//       return With(unitOfWork);
//   }
//   IAction async Task
//     {
//       UnitOfWork = null;
//       return this;
//   }
//   IAction<TReturn> async Task<TReturn>
//     {
//       return With(null);
// }
// IAction async Task.With(IUnitOfWork unitOfWork)
//     {
//   UnitOfWork = unitOfWork;
//   return this;
// }
// void IAction
// {
//   RunInternally();
// }
//   }
// }