using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.StockCheckingPerson;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region DeleteStockCheckingStuff
    internal void DeleteStockCheckingStuff(int stockCheckingId, int stuffId)
    {
      repository.Delete(GetStockCheckingStuff(stockCheckingId: stockCheckingId, stuffId: stuffId));
    }
    #endregion
    #region GetStockCheckingStuff
    internal StockCheckingStuff GetStockCheckingStuff(int stockCheckingId, int stuffId)
    {
      var data =
                GetStockCheckingStuffs(e => e, stockCheckingId: stockCheckingId, stuffId: stuffId)
                    .FirstOrDefault();
      if (data == null)
        throw new StockCheckingStuffNotFoundException();
      return data;
    }
    #endregion
    #region GetStockCheckingStuffs
    internal IQueryable<TResult> GetStockCheckingStuffs<TResult>(
        Expression<Func<StockCheckingStuff, TResult>> selector,
        TValue<int> stockCheckingId = null,
        TValue<int> stuffId = null)
    {
      var Stockcheckingstuff = repository.GetQuery<StockCheckingStuff>();
      if (stockCheckingId != null)
        Stockcheckingstuff = Stockcheckingstuff.Where(i => i.StockCheckingId == stockCheckingId);
      if (stuffId != null)
        Stockcheckingstuff = Stockcheckingstuff.Where(i => i.StuffId == stuffId);
      return Stockcheckingstuff.Select(selector);
    }
    #endregion
    #region AddStockCheckingStuff
    internal StockCheckingStuff AddStockCheckingStuff(int stockCheckingId, int stuffId)
    {
      var scs = repository.Create<StockCheckingStuff>();
      scs.StockCheckingId = stockCheckingId;
      scs.StuffId = stuffId;
      repository.Add(scs);
      return scs;
    }
    #endregion
  }
}