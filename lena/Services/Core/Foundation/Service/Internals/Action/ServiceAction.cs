// using System;
// using lena.Services.Common;
// //using Parlar.DAL;
// //using Parlar.DAL.UnitOfWorks;
// using lena.Models.Common;
// namespace lena.Services.Core.Foundation.Action
// {
//   public abstract class InternalServiceActionHandler<TReturn>
//     where TReturn : Result
//   {
//     protected InternalServiceActionHandler(Func<Exception, TReturn> onFail)
//     {
//       this.OnFailVal = onFail;
//     }
//     public InternalServiceGroup Modules => App.Internals;
//     internal Func<InternalServiceGroup, async TaskOfWork, TReturn> Task { private get; set; }
//     protected Func<Exception, TReturn> OnFailVal { get; set; }
//     protected TReturn RunExpression()
//     {
//       using (var unitOfWork = DALFactory.CreateUnitOfWork())
//       {
//         try
//         {
//           var transactionName = "NO_REQUEST";
//           var requestProvider = App.Providers.Request;
//           if (requestProvider != null)
//           {
//             transactionName = requestProvider.Url + ":" + requestProvider.Url;
//           }
//           unitOfWork.BeginTransaction(transactionName);
//           var result = Task.Invoke(Modules, unitOfWork);
//           //unitOfWork.SaveChanges();
//           unitOfWork.CommitTransaction();
//           return result;
//         }
//         catch (Exception e)
//         {
//           unitOfWork.RollBackTransaction();
//           return OnFailVal.Invoke(e);
//         }
//       }
//     }
//     protected void OnFailExpression(Func<Exception, TReturn> expression)
//     {
//       OnFailVal = expression;
//     }
//   }
//   public class ServiceActionHandler<TReturn>
//           where TReturn : Result
//   {
//     public ServiceActionHandler(Func<ExternalServiceGroup, TReturn> task)
//     {
//       Task = task;
//     }
//     public ServiceActionHandler(Func<ExternalServiceGroup, TReturn> task, Func<Exception, TReturn> onFailVal)
//     {
//       Task = task;
//       OnFailVal = onFailVal;
//     }
//     protected ServiceActionHandler(Func<Exception, TReturn> onFail)
//     {
//       OnFailVal = onFail;
//     }
//     public ExternalServiceGroup Modules => App.Api;
//     protected Func<ExternalServiceGroup, TReturn> Task { get; set; }
//     protected Func<Exception, TReturn> OnFailVal { get; set; }
//     protected void OnFailExpression(Func<Exception, TReturn> expression)
//     {
//       OnFailVal = expression;
//     }
//     public TReturn RunExpression()
//     {
//       try
//       {
//         var result = Task.Invoke(Modules);
//         return result;
//       }
//       catch (Exception e)
//       {
//         return OnFailVal.Invoke(e);
//       }
//     }
//   }
//   public class ServiceResultHandler<TR> : ServiceActionHandler<Result<TR>>, IServiceAction<Result<TR>>
//   {
//     public ServiceResultHandler(Func<ExternalServiceGroup, Result<TR>> task) : base(exception => new Result<TR>() { Success = false, Message = exception.ToResponse() })
//     {
//       Task = task;
//     }
//     public Result<TR> Run()
//     {
//       return RunExpression();
//     }
//     public void OnFail(Func<Exception, Result<TR>> expression)
//     {
//       OnFailExpression(expression);
//     }
//   }
// }
