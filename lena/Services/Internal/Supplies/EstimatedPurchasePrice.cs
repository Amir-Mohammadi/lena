using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region AddProcess
    /// <summary>
    /// Remove old estimate purchase price and add new estimate purchase price
    /// </summary>
    /// <param name="currencyId"></param>
    /// <param name="price"></param>
    /// <param name="stuffId"></param>
    /// <param name="purchaseOrderId"></param>
    /// <returns></returns>
    public EstimatedPurchasePrice AddEstimatedPurchasePriceProcess(
        byte currencyId,
        double price,
        int stuffId,
        int purchaseOrderId)
    {

      var oldEstimatedPurchasePrices = GetEstimatedPurchasePrices(
                    selector: e => e,
                    stuffId: stuffId,
                    isCurrent: true,
                    purchaseOrderId: purchaseOrderId);
      foreach (var oldEstimatedPurchasePrice in oldEstimatedPurchasePrices)
      {
        App.Internals.Supplies.ArchiveStuffPriceProcess(stuffPrice: oldEstimatedPurchasePrice,
                      rowVersion: oldEstimatedPurchasePrice.RowVersion);
      }
      return AddEstimatedPurchasePrice(
                    currencyId: currencyId,
                    price: price,
                    stuffId: stuffId,
                    purchaseOrderId: purchaseOrderId);
    }

    #endregion
    #region Add
    public EstimatedPurchasePrice AddEstimatedPurchasePrice(
        byte currencyId,
        double price,
        int stuffId,
        int purchaseOrderId)
    {

      var estimatedPurchasePrice = repository.Create<EstimatedPurchasePrice>();
      estimatedPurchasePrice.PurchaseOrderId = purchaseOrderId;
      App.Internals.Supplies.AddStuffPrice(
                    stuffPrice: estimatedPurchasePrice,
                    currencyId: currencyId,
                    price: price,
                    stuffId: stuffId,
                    type: StuffPriceType.EstimatedPurchasePrice,
                    description: null);
      return estimatedPurchasePrice;
    }
    #endregion
    #region Edit
    public EstimatedPurchasePrice EditEstimatedPurchasePrice(
        int id,
        byte[] rowVersion,
        TValue<byte> currencyId = null,
        TValue<double> price = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> purchaseOrderId = null)
    {

      var estimatedPurchasePrice = GetEstimatedPurchasePrice(id: id);
      return EditEstimatedPurchasePrice(
                    estimatedPurchasePrice: estimatedPurchasePrice,
                    rowVersion: rowVersion,
                    currencyId: currencyId,
                    price: price,
                    stuffId: stuffId,
                    status: status,
                    purchaseOrderId: purchaseOrderId);
    }

    public EstimatedPurchasePrice EditEstimatedPurchasePrice(
        EstimatedPurchasePrice estimatedPurchasePrice,
        byte[] rowVersion,
        TValue<byte> currencyId = null,
        TValue<double> price = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> purchaseOrderId = null)
    {

      if (purchaseOrderId != null)
        estimatedPurchasePrice.PurchaseOrderId = purchaseOrderId;
      App.Internals.Supplies.EditStuffPrice(
                    stuffPrice: estimatedPurchasePrice,
                    rowVersion: rowVersion,
                    currencyId: currencyId,
                    price: price,
                    stuffId: stuffId,
                    status: status);
      return estimatedPurchasePrice;
    }

    #endregion
    #region EditProcess
    public EstimatedPurchasePrice EditEstimatedPurchasePriceProcess(
        int id,
        byte[] rowVersion,
        byte currencyId,
        int price)
    {

      ArchiveStuffPriceProcess(id: id,
                    rowVersion: rowVersion);

      var estimatedPurchasePrice = GetEstimatedPurchasePrice(id: id);
      return AddEstimatedPurchasePrice(
                    currencyId: currencyId,
                    price: price,
                    stuffId: estimatedPurchasePrice.StuffId,
                    purchaseOrderId: estimatedPurchasePrice.PurchaseOrderId);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetEstimatedPurchasePrices<TResult>(
        Expression<Func<EstimatedPurchasePrice, TResult>> selector,
        TValue<int> id = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> currencyId = null,
        TValue<int> userId = null,
        TValue<int> purchaseOrderId = null,
        TValue<bool> isCurrent = null)
    {

      var baseQuery = App.Internals.Supplies.GetStuffPrices(
                    selector: e => e,
                    id: id,
                    stuffId: stuffId,
                    status: status,
                    currencyId: currencyId,
                    userId: userId,
                    priceType: StuffPriceType.EstimatedPurchasePrice,
                    isCurrent: isCurrent,
                    isDelete: false);
      var query = baseQuery.OfType<EstimatedPurchasePrice>();
      if (purchaseOrderId != null)
        query = query.Where(i => i.PurchaseOrderId == purchaseOrderId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public EstimatedPurchasePrice GetEstimatedPurchasePrice(int id) => GetEstimatedPurchasePrice(selector: e => e, id: id);
    public TResult GetEstimatedPurchasePrice<TResult>(Expression<Func<EstimatedPurchasePrice, TResult>> selector, int id)
    {

      var estimatedPurchasePrice = GetEstimatedPurchasePrices(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (estimatedPurchasePrice == null)
        throw new EstimatedPurchasePriceNotFoundException(id);
      return estimatedPurchasePrice;
    }
    #endregion
    #region Sort
    //public IOrderedQueryable<EstimatedPurchasePriceResult> SortEstimatedPurchasePriceResult(IQueryable<EstimatedPurchasePriceResult> input,
    //    SortInput<EstimatedPurchasePriceSortType> options)
    //{
    //    switch (options.SortType)
    //    {
    //        case EstimatedPurchasePriceSortType.CurrencyTitle:
    //            return input.OrderBy(i => i.CurrencyTitle, options.SortOrder);
    //        case EstimatedPurchasePriceSortType.DateTime:
    //            return input.OrderBy(i => i.DateTime, options.SortOrder);
    //        case EstimatedPurchasePriceSortType.EmployeeFullName:
    //            return input.OrderBy(i => i.EmployeeFullName, options.SortOrder);
    //        case EstimatedPurchasePriceSortType.IsArchive:
    //            return input.OrderBy(i => i.IsArchive, options.SortOrder);
    //        case EstimatedPurchasePriceSortType.Price:
    //            return input.OrderBy(i => i.Price, options.SortOrder);
    //        case EstimatedPurchasePriceSortType.StuffCode:
    //            return input.OrderBy(i => i.StuffCode, options.SortOrder);
    //        case EstimatedPurchasePriceSortType.StuffName:
    //            return input.OrderBy(i => i.StuffName, options.SortOrder);
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}
    #endregion
    #region ToResult
    //public Expression<Func<EstimatedPurchasePrice, EstimatedPurchasePriceResult>> ToEstimatedPurchasePriceResult =>
    //    estimatedPurchasePrice => new EstimatedPurchasePriceResult
    //    {
    //        Id = estimatedPurchasePrice.Id,
    //        CurrencyId = estimatedPurchasePrice.CurrencyId,
    //        CurrencyTitle = estimatedPurchasePrice.Currency.Title,
    //        DateTime = estimatedPurchasePrice.DateTime,
    //        EmployeeFullName = estimatedPurchasePrice.User.Employee.FirstName + " " + estimatedPurchasePrice.User.Employee.LastName,
    //        EmployeeId = estimatedPurchasePrice.User.Employee.Id,
    //        IsArchive = estimatedPurchasePrice.IsArchive,
    //        Price = estimatedPurchasePrice.Price,
    //        StuffId = estimatedPurchasePrice.StuffId,
    //        StuffCode = estimatedPurchasePrice.Stuff.Code,
    //        StuffName = estimatedPurchasePrice.Stuff.Name,
    //        RowVersion = estimatedPurchasePrice.RowVersion
    //    };
    #endregion
    #region Search
    //public IQueryable<EstimatedPurchasePriceResult> SearchEstimatedPurchasePriceResultQuery(
    //    IQueryable<EstimatedPurchasePriceResult> query,
    //    AdvanceSearchItem[] advanceSearchItems,
    //    string searchText)
    //{
    //    if (!string.IsNullOrWhiteSpace(searchText))
    //        query = from estimatedPurchasePrice in query
    //                where estimatedPurchasePrice.CurrencyTitle.Contains(searchText) ||
    //                      estimatedPurchasePrice.EmployeeFullName.Contains(searchText) ||
    //                      estimatedPurchasePrice.StuffCode.Contains(searchText) ||
    //                      estimatedPurchasePrice.StuffName.Contains(searchText)
    //                select estimatedPurchasePrice;
    //    if (advanceSearchItems.Any())
    //        query = query.Where(advanceSearchItems);
    //    return query;
    //}
    #endregion

    #region DeleteProcess
    public void DeleteEstimatedPurchasePrice(EstimatedPurchasePrice estimatedPurchasePrice)
    {



      App.Internals.ApplicationBase.EditBaseEntity(
                baseEntity: estimatedPurchasePrice,
                rowVersion: estimatedPurchasePrice.RowVersion,
                isDelete: true);

    }
    public void DeleteEstimatedPurchasePrice(int id, byte[] rowVersion)
    {

      var estimatedPurchasePrice = GetEstimatedPurchasePrice(id: id);

      App.Internals.ApplicationBase.EditBaseEntity(
                baseEntity: estimatedPurchasePrice,
                rowVersion: rowVersion,
                isDelete: true);

    }
    #endregion


  }
}
