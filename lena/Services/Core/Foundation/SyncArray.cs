using System;
using System.Collections.Generic;
using System.Linq;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
//using Parlar.DAL;
//using Parlar.DAL.UnitOfWorks;
namespace lena.Services.Core.Foundation
{
    public class SyncArray<T>
    {
        public T[] Added { get; set; }
        public T[] Removed { get; set; }
        public void Sync(Action<T> add, Action<T> remove)
        {
            foreach (var a in Added)
            {
                add.Invoke(a);
            }
            foreach (var d in Removed)
            {
                remove.Invoke(d);
            }
        }
        public void Sync<TA>(Func<T, TA> add, Func<T> remove)
        {
            foreach (var a in Added)
            {
                add.Invoke(a);
            }
            foreach (var d in Removed)
            {
                remove.Invoke(d);
            }
        }
    }
}