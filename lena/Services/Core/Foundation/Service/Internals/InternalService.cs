// using System;
// ////using lena.Services.Core.Foundation.Action;
// using lena.Services.Core.Foundation.Service;
// //using lena.Services.Core.Foundation.Service.Internal.Action;
// //using Parlar.DAL;
// //using Parlar.DAL.Repositories;
// namespace lena.Services.Core.Foundation
// {
//   public class InternalService<T> where T : class
//   {
//     public static async Task<TA> Do<TA>(Func<IRepository, TA> expression)
//     {
//       return new ActionHandler<TA>(expression);
//     }
//     public static async Task Do(Action<IRepository> expression)
//     {
//       return new ActionHandler<bool>((re) =>
//       {
//         expression.Invoke(re);
//         return true;
//       });
//     }
//   }
// }