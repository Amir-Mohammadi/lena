// using System;
// using lena.Services.Common;
// //using Parlar.DAL;
// //using Parlar.DAL.UnitOfWorks;
// using lena.Domains.Enums;
// using lena.Models.ApplicationBase.Logger;
// using lena.Models.UserManagement.User;
// using System.Diagnostics;
// using lena.Services.Core.Provider.Logger;
// using lena.Models.PersistLogger;
// using lena.Services.Core.Foundation.Service.Internal;
// using System.Runtime.ExceptionServices;
// using System.Threading.Tasks;
// namespace lena.Services.Core.Foundation.Service.External
// {
//   public abstract class BllActionFilter<TInput, TOutput>
//   {
//     protected abstract void OnProcessStarted(Object input);
//     protected abstract TOutput OnProcessing(TInput input);
//     protected abstract void OnProcessFinished(Object input, object output);
//     protected abstract TOutput OnException(Exception exception);
//     protected TOutput RunInternally(TInput input)
//     {
//       try
//       {
//         if (App.Providers.Storage.LoggerEnabled)
//         {
//           #region Log REQUEST
//           //try
//           //{
//           //    App.Providers.Logger.Info("REQUEST");
//           //}
//           //catch (Exception) { }
//           #endregion
//         }
//         OnProcessStarted(input);
//         var output = OnProcessing(input);
//         OnProcessFinished(input, output);
//         if (App.Providers.Storage.LoggerEnabled)
//         {
//           #region Log DONE
//           try
//           {
//             App.Providers.Logger.Info("DONE");
//           }
//           catch (Exception) { }
//           #endregion
//         }
//         App.Providers.Request.Response = output;
//         App.Providers.Request.IsFailed = false;
//         return output;
//       }
//       catch (Exception exception)
//       {
//         App.Providers.Request.IsFailed = true;
//         if (App.Providers.Storage.LoggerEnabled)
//         {
//           #region Log Error
//           try
//           {
//             //var stackTrace = App.Providers.Diagnostics.Print();  
//             var stackTrace = exception.StackTrace;
//             stackTrace = stackTrace == "" ? exception.StackTrace : stackTrace;
//             App.Providers.Logger.Error(stackTrace, exception.ToResponse());
//           }
//           catch (Exception) { }
//           #endregion
//         }
//         #region handle exceptions
//         if (exception is InternalServiceException)
//         {
//           ((InternalServiceException)exception).Init();
//           return OnException(exception);
//         }
//         else
//         {
//           var unknownException = new UnknownInternalServiceException(exception.Message, exception);
//           unknownException.Init();
//           return OnException(unknownException);
//         }
//         #endregion
//       }
//     }
//   }
//   public abstract class ServiceActionFilter<TInput, TOutput> : BllActionFilter<TInput, TOutput>
//   {
//     private Func<TInput, InternalServiceGroup, async TaskOfWork, TOutput> task;
//     public Action<object> ProcessStarted { get; set; }
//     public Action<object, object> ProcessFinished { get; set; }
//     protected bool IsAtomic = true;
//     protected void ActivateAtomic()
//     {
//       IsAtomic = true;
//     }
//     protected void ActivateNoneAtomic()
//     {
//       IsAtomic = false;
//     }
//     protected ServiceActionFilter(Func<TInput, InternalServiceGroup, async TaskOfWork, TOutput> task)
//     {
//       this.task = task;
//     }
//     protected ServiceActionFilter(Func<InternalServiceGroup, async TaskOfWork, TOutput> task)
//     {
//       this.task = (input, group, arg3) => task.Invoke(group, arg3);
//     }
//     protected override void OnProcessStarted(object input)
//     {
//       ProcessStarted.Invoke(input);
//     }
//     protected override TOutput OnProcessing(TInput input)
//     {
//       using (var unitOfWork = DALFactory.CreateUnitOfWork())
//       {
//         try
//         {
//           unitOfWork.IsAtomic = IsAtomic;
//           var transactionName = "NO_REQUEST";
//           if (App.Providers.Request != null)
//           {
//             transactionName = App.Providers.Request.ClientAddress + ":" + App.Providers.Request.Url;
//           }
//           unitOfWork.BeginTransaction(transactionName);
//           App.Providers.UncommitedTransactionAgent.Use(unitOfWork);
//           var res = task.Invoke(input, App.Internals, unitOfWork);
//           if (App.Providers.UncommitedTransactionAgent.Any)
//           {
//             App.Providers.PersistentLogger.Steps.Add(new PersistentLogStep(PersistentLogStepType.Log,
//                 new
//                 {
//                   Transactions = App.Providers.UncommitedTransactionAgent.GetReport()
//                 }
//                 , 20));
//           }
//           App.Providers.UncommitedTransactionAgent.CheckTransactionBatch();
//           unitOfWork.CommitTransaction();
//           return res;
//         }
//         catch (Exception ex)
//         {
//           //todo check and fix it koohgard
//           try
//           {
//             unitOfWork.RollBackTransaction();
//           }
//           catch
//           {
//           }
//           ExceptionDispatchInfo.Capture(ex).Throw();
//           throw; // only for prevent return error
//         }
//       }
//     }
//     protected override void OnProcessFinished(object input, object output)
//     {
//       ProcessFinished.Invoke(input, output);
//     }
//   }
// }