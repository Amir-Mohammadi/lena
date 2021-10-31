using System;
using System.CodeDom;
using System.Collections.Generic;
//using System.Data.Entity;
// using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
//using LinqLib.Sort;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting.BillOfMaterialPriceHistory;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    public BillOfMaterialPriceHistory GetBillOfMaterialPriceHistory(int id)
    {
      return GetBillOfMaterialPriceHistories(e => e, id: id).FirstOrDefault();
    }
    public IQueryable<BillOfMaterialPriceHistory> GetBillOfMaterialPriceHistories(
        TValue<int> id = null,
        TValue<int> stuffId = null,
        TValue<int?> version = null,
        TValue<int> currencyId = null,
        TValue<int> userId = null,
        TValue<DateTime> date = null)
    {
      return GetBillOfMaterialPriceHistories(e => e, id: id, stuffId: stuffId, version: version, currencyId: currencyId, userId: userId, date: date);
    }
    public IQueryable<TResult> GetBillOfMaterialPriceHistories<TResult>(
            Expression<Func<BillOfMaterialPriceHistory, TResult>> selector,
           TValue<int> id = null,
           TValue<int> stuffId = null,
           TValue<int?> version = null,
           TValue<int> currencyId = null,
           TValue<int> userId = null,
           TValue<DateTime> date = null)
    {
      var query = repository.GetQuery<BillOfMaterialPriceHistory>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (version != null)
        query = query.Where(i => i.Version == version);
      if (currencyId != null)
        query = query.Where(i => i.CurrencyId == currencyId);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (date != null)
        query = query.Where(i => i.DateTime.Date == date);
      return query.Select(selector);
    }
    public BillOfMaterialPriceHistory AddBillOfMaterialPriceHistory(
        int stuffId,
        int? version,
        byte currencyId,
        double totalPrice)
    {
      var entity = repository.Create<BillOfMaterialPriceHistory>();
      entity.StuffId = stuffId;
      entity.Version = version;
      entity.CurrencyId = currencyId;
      entity.TotalPrice = totalPrice;
      entity.UserId = App.Providers.Security.CurrentLoginData.UserId;
      entity.DateTime = DateTime.UtcNow;
      repository.Add(entity);
      return entity;
    }
    public void DeleteBillOfMaterialPriceHistory(BillOfMaterialPriceHistory entity, int id)
    {
      var e = entity == null ? GetBillOfMaterialPriceHistory(id) : entity;
      repository.Delete<BillOfMaterialPriceHistory>(e);
    }
    public Expression<Func<BillOfMaterialPriceHistory, BillOfMaterialPriceHistoryResult>> ToBillOfMaterialPriceHistoryResult =
               entity => new BillOfMaterialPriceHistoryResult
               {
                 Id = entity.Id,
                 StuffId = entity.StuffId,
                 StuffCode = entity.Stuff.Code,
                 StuffName = entity.Stuff.Name,
                 Version = entity.Version,
                 TotalPrice = entity.TotalPrice,
                 CurrencyId = entity.CurrencyId,
                 CurrencyTitle = entity.Currency.Title,
                 UserId = entity.UserId,
                 EmployeeId = entity.User.Employee.Id,
                 EmployeeFullName = entity.User.Employee.FirstName + " " + entity.User.Employee.LastName,
                 DateTime = entity.DateTime
               };
    #region Sort
    public IOrderedQueryable<BillOfMaterialPriceHistoryResult> SortBillOfMaterialPriceHistoryResult(
      IQueryable<BillOfMaterialPriceHistoryResult> query,
      SortInput<BillOfMaterialPriceHistorySortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case BillOfMaterialPriceHistorySortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case BillOfMaterialPriceHistorySortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case BillOfMaterialPriceHistorySortType.Version:
          return query.OrderBy(i => i.Version, sortInput.SortOrder);
        case BillOfMaterialPriceHistorySortType.TotalPrice:
          return query.OrderBy(i => i.TotalPrice, sortInput.SortOrder);
        case BillOfMaterialPriceHistorySortType.CurrencyTitle:
          return query.OrderBy(i => i.CurrencyTitle, sortInput.SortOrder);
        case BillOfMaterialPriceHistorySortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case BillOfMaterialPriceHistorySortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException($"SortBillOfMaterialPriceHistory sort type not implemented! [{nameof(BillOfMaterialPriceHistorySortType)}]");
      }
    }
    #endregion
    #region Search
    public IQueryable<BillOfMaterialPriceHistoryResult> SearchBillOfMaterialPriceHistoryResult(
         IQueryable<BillOfMaterialPriceHistoryResult> query,
         string searchText,
         AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.CurrencyTitle.Contains(searchText) ||
                item.StuffName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
  }
}