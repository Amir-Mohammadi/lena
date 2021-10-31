using System;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Planning.StuffPriceDiscrepancy;
using System.Linq;
using lena.Models.Common;
using System.Linq.Dynamic;
using lena.Services.Common;
using lena.Domains.Enums.SortTypes;
using lena.Services.Core;
using System.Linq.Expressions;
// using lena.Services.Core.Foundation.Service.External;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Add
    public StuffPriceDiscrepancy AddStuffPriceDiscrepancy(
        TValue<int> purchaseOrderId,
        TValue<double> purchaseOrderPrice,
        TValue<byte> purchaseOrderCurrencyId,
        TValue<double> purchaseOrderQty,
        TValue<double?> currentStuffBasePrice,
        TValue<byte?> CurrentStuffBasePriceCurrencyId,
        TValue<string> description
    )
    {

      var stuffPriceDiscrepancy = repository.Create<StuffPriceDiscrepancy>();
      stuffPriceDiscrepancy.PurchaseOrderId = purchaseOrderId;
      stuffPriceDiscrepancy.PurchaseOrderPrice = purchaseOrderPrice;
      stuffPriceDiscrepancy.PurchaseOrderCurrencyId = purchaseOrderCurrencyId;
      stuffPriceDiscrepancy.PurchaseOrderQty = purchaseOrderQty;
      stuffPriceDiscrepancy.CurrentStuffBasePrice = currentStuffBasePrice;
      stuffPriceDiscrepancy.CurrentStuffBasePriceCurrencyId = CurrentStuffBasePriceCurrencyId;
      stuffPriceDiscrepancy.Description = description;
      stuffPriceDiscrepancy.DateTime = DateTime.UtcNow;
      stuffPriceDiscrepancy.Status = StuffPriceDiscrepancyStatus.NotAction;
      repository.Add(stuffPriceDiscrepancy);
      return stuffPriceDiscrepancy;
    }
    #endregion

    #region Get
    public StuffPriceDiscrepancy GetStuffPriceDiscrepancy(int id)
    {

      var stuffPriceDiscrepancy = GetStuffPriceDiscrepancies(e => e, id: id).FirstOrDefault();
      if (stuffPriceDiscrepancy == null)
        throw new StuffPriceDiscrepancyNotFoundException(id);
      return stuffPriceDiscrepancy;
    }

    public StuffPriceDiscrepancyResult ToStuffPriceDiscrepancyResult(StuffPriceDiscrepancy stuffPriceDiscrepancy)
    {
      return new StuffPriceDiscrepancyResult
      {
        Id = stuffPriceDiscrepancy.Id,
        RowVersion = stuffPriceDiscrepancy.RowVersion,
        PurchaseOrderId = stuffPriceDiscrepancy.PurchaseOrderId,
        PurchaseOrderPrice = stuffPriceDiscrepancy.PurchaseOrderPrice,
        PurchaseOrderCurrencyId = stuffPriceDiscrepancy.PurchaseOrderCurrencyId,
        PurchaseOrderQty = stuffPriceDiscrepancy.PurchaseOrderQty,
        CurrentStuffBasePrice = stuffPriceDiscrepancy.CurrentStuffBasePrice,
        CurrentStuffBasePriceCurrencyId = stuffPriceDiscrepancy.CurrentStuffBasePriceCurrencyId,
        Description = stuffPriceDiscrepancy.Description,
        DateTime = stuffPriceDiscrepancy.DateTime,
        ConfirmationDateTime = stuffPriceDiscrepancy.ConfirmationDateTime,
        ConfirmerUserId = stuffPriceDiscrepancy.ConfirmerUserId,
        ConfirmationDescription = stuffPriceDiscrepancy.ConfirmationDescription,
        Status = stuffPriceDiscrepancy.Status,
      };

    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetStuffPriceDiscrepancies<TResult>(
      Expression<Func<StuffPriceDiscrepancy, TResult>> selector,
      TValue<int> id = null,
      TValue<DateTime> fromDateTime = null,
      TValue<DateTime> toDateTime = null,
      TValue<StuffPriceDiscrepancyStatus> status = null,
      TValue<int> stuffId = null,
      TValue<int> stuffCategoryId = null,
      TValue<string> purchaseOrderCode = null,
      TValue<int> purchaseOrderCurrencyId = null,
      TValue<int> providerId = null,
      TValue<int> purchaseOrderEmployeeId = null
    )
    {


      var query = repository.GetQuery<StuffPriceDiscrepancy>();

      if (id != null)
        query = query.Where(d => d.Id == id);
      if (fromDateTime != null)
        query = query.Where(d => d.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(d => d.DateTime <= toDateTime);
      if (status != null)
        query = query.Where(d => d.Status == status);
      if (stuffId != null)
        query = query.Where(d => d.PurchaseOrder.StuffId == stuffId);
      if (stuffCategoryId != null)
        query = query.Where(d => d.PurchaseOrder.Stuff.StuffCategoryId == stuffCategoryId);
      if (purchaseOrderCode != null)
        query = query.Where(d => d.PurchaseOrder.Code == purchaseOrderCode);
      if (purchaseOrderCurrencyId != null)
        query = query.Where(d => d.PurchaseOrder.CurrencyId == purchaseOrderCurrencyId);
      if (providerId != null)
        query = query.Where(d => d.PurchaseOrder.ProviderId == providerId);
      if (purchaseOrderEmployeeId != null)
        query = query.Where(d => d.PurchaseOrder.User.Employee.Id == purchaseOrderEmployeeId);

      return query.Select(selector);
    }
    #endregion

    #region ToResult
    public IQueryable<StuffPriceDiscrepancyResult> ToStuffPriceDiscrepancyResultQuery(
        IQueryable<StuffPriceDiscrepancy> stuffPriceDiscrepancies
        )
    {
      var results =
          from stuffPriceDiscrepancy in stuffPriceDiscrepancies

          select new StuffPriceDiscrepancyResult
          {
            Id = stuffPriceDiscrepancy.Id,
            RowVersion = stuffPriceDiscrepancy.RowVersion,
            PurchaseOrderId = stuffPriceDiscrepancy.PurchaseOrderId,
            PurchaseOrderPrice = stuffPriceDiscrepancy.PurchaseOrderPrice,
            PurchaseOrderCurrencyId = stuffPriceDiscrepancy.PurchaseOrderCurrencyId,
            PurchaseOrderQty = stuffPriceDiscrepancy.PurchaseOrderQty,
            CurrentStuffBasePrice = stuffPriceDiscrepancy.CurrentStuffBasePrice,
            CurrentStuffBasePriceCurrencyId = stuffPriceDiscrepancy.CurrentStuffBasePriceCurrencyId,
            CurrentStuffBasePriceCurrencyTitle = stuffPriceDiscrepancy.Currency.Title,
            Description = stuffPriceDiscrepancy.Description,
            DateTime = stuffPriceDiscrepancy.DateTime,
            ConfirmationDateTime = stuffPriceDiscrepancy.ConfirmationDateTime,
            ConfirmerUserId = stuffPriceDiscrepancy.ConfirmerUserId,
            ConfirmerUserFullName = stuffPriceDiscrepancy.User.Employee.FirstName + " " + stuffPriceDiscrepancy.User.Employee.LastName,
            ConfirmationDescription = stuffPriceDiscrepancy.ConfirmationDescription,
            Status = stuffPriceDiscrepancy.Status,
            PurchaseOrderEmployeeFullName = stuffPriceDiscrepancy.PurchaseOrder.User.Employee.FirstName + " " + stuffPriceDiscrepancy.PurchaseOrder.User.Employee.LastName,
            StuffCategoryId = stuffPriceDiscrepancy.PurchaseOrder.Stuff.StuffCategoryId,
            StuffCategoryName = stuffPriceDiscrepancy.PurchaseOrder.Stuff.StuffCategory.Name,
            StuffNoun = stuffPriceDiscrepancy.PurchaseOrder.Stuff.Noun,
            StuffName = stuffPriceDiscrepancy.PurchaseOrder.Stuff.Name,
            StuffCode = stuffPriceDiscrepancy.PurchaseOrder.Stuff.Code,
            StuffId = stuffPriceDiscrepancy.PurchaseOrder.StuffId,
            ProviderName = stuffPriceDiscrepancy.PurchaseOrder.Provider.Name,
            UnitName = stuffPriceDiscrepancy.PurchaseOrder.Unit.Name,
            UnitId = stuffPriceDiscrepancy.PurchaseOrder.UnitId,
            PurchaseOrderCurrencyTitle = stuffPriceDiscrepancy.PurchaseOrder.Currency.Title,
            PurchaseOrderCode = stuffPriceDiscrepancy.PurchaseOrder.Code,
          };
      return results;
    }
    #endregion

    #region SearchResult
    public IQueryable<StuffPriceDiscrepancyResult> SearchStuffPriceDiscrepancyResultQuery(
              IQueryable<StuffPriceDiscrepancyResult> query,
              string searchText,
              AdvanceSearchItem[] advanceSearchItems,
              TValue<DateTime?> fromDateTime = null,
              TValue<DateTime?> toDateTime = null,
              TValue<StuffPriceDiscrepancyStatus?> status = null,
              TValue<int?> stuffId = null,
              TValue<string> purchaseOrderCode = null,
              TValue<int?> purchaseOrderCurrencyId = null,
              TValue<int?> stuffCategoryId = null
            )
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.StuffName.Contains(searchText) ||
                      item.StuffCode.Contains(searchText) ||
                      item.PurchaseOrderCode.Contains(searchText) ||
                      item.ProviderName.Contains(searchText) ||
                      item.PurchaseOrderCurrencyTitle.Contains(searchText)
                select item;
      }

      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);

      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);

      if (status != null)
        query = query.Where(i => i.Status == status);

      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);

      if (purchaseOrderCode != null)
        query = query.Where(i => i.PurchaseOrderCode == purchaseOrderCode);

      if (purchaseOrderCurrencyId != null)
        query = query.Where(i => i.PurchaseOrderCurrencyId == purchaseOrderCurrencyId);

      if (stuffCategoryId != null)
        query = query.Where(i => i.StuffCategoryId == stuffCategoryId);

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion

    #region SortResult
    public IOrderedQueryable<StuffPriceDiscrepancyResult> SortStuffPriceDiscrepancyResult(IQueryable<StuffPriceDiscrepancyResult> input, SortInput<StuffPriceDiscrepancySortType> options)
    {
      switch (options.SortType)
      {
        case StuffPriceDiscrepancySortType.PurchaseOrderCode:
          return input.OrderBy(r => r.PurchaseOrderCode, options.SortOrder);
        case StuffPriceDiscrepancySortType.StuffCode:
          return input.OrderBy(r => r.StuffCode, options.SortOrder);
        case StuffPriceDiscrepancySortType.StuffName:
          return input.OrderBy(r => r.StuffName, options.SortOrder);
        case StuffPriceDiscrepancySortType.StuffNoun:
          return input.OrderBy(r => r.StuffNoun, options.SortOrder);
        case StuffPriceDiscrepancySortType.StuffCategoryName:
          return input.OrderBy(r => r.StuffCategoryName, options.SortOrder);
        case StuffPriceDiscrepancySortType.PurchaseOrderPrice:
          return input.OrderBy(r => r.PurchaseOrderPrice, options.SortOrder);
        case StuffPriceDiscrepancySortType.PurchaseOrderCurrencyTitle:
          return input.OrderBy(r => r.PurchaseOrderCurrencyTitle, options.SortOrder);
        case StuffPriceDiscrepancySortType.CurrentStuffBasePrice:
          return input.OrderBy(r => r.CurrentStuffBasePrice, options.SortOrder);
        case StuffPriceDiscrepancySortType.CurrentStuffBasePriceCurrencyTitle:
          return input.OrderBy(r => r.CurrentStuffBasePriceCurrencyTitle, options.SortOrder);
        case StuffPriceDiscrepancySortType.PurchaseOrderQty:
          return input.OrderBy(r => r.PurchaseOrderQty, options.SortOrder);
        case StuffPriceDiscrepancySortType.UnitName:
          return input.OrderBy(r => r.UnitName, options.SortOrder);
        case StuffPriceDiscrepancySortType.Description:
          return input.OrderBy(r => r.Description, options.SortOrder);
        case StuffPriceDiscrepancySortType.Status:
          return input.OrderBy(r => r.Status, options.SortOrder);
        case StuffPriceDiscrepancySortType.PurchaseOrderEmployeeFullName:
          return input.OrderBy(r => r.PurchaseOrderEmployeeFullName, options.SortOrder);
        case StuffPriceDiscrepancySortType.DateTime:
          return input.OrderBy(r => r.DateTime, options.SortOrder);
        case StuffPriceDiscrepancySortType.ProviderName:
          return input.OrderBy(r => r.ProviderName, options.SortOrder);
        case StuffPriceDiscrepancySortType.ConfirmerUserFullName:
          return input.OrderBy(r => r.ConfirmerUserFullName, options.SortOrder);
        case StuffPriceDiscrepancySortType.ConfirmationDescription:
          return input.OrderBy(r => r.ConfirmationDescription, options.SortOrder);
        case StuffPriceDiscrepancySortType.ConfirmationDateTime:
          return input.OrderBy(r => r.ConfirmationDateTime, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(options.SortOrder), options.SortType, null);
      }
    }
    #endregion

    #region AcceptOrReject
    public void AcceptOrRejectStuffPriceDiscrepancy(AcceptOrRejectStuffPriceDiscrepancyInput[] inputs)
    {

      foreach (var input in inputs)
      {
        EditStuffPriceDiscrepancy(
                      rowVersion: input.RowVersion,
                      id: input.Id,
                      stuffPriceDiscrepancyStatus: input.IsConfirmed ? StuffPriceDiscrepancyStatus.Accepted : StuffPriceDiscrepancyStatus.Rejected,
                      confirmationDescription: input.ConfirmationDescription
                      );
      }
    }
    #endregion

    #region Edit
    public StuffPriceDiscrepancy EditStuffPriceDiscrepancy(
    byte[] rowVersion,
    int id,
    TValue<StuffPriceDiscrepancyStatus> stuffPriceDiscrepancyStatus = null,
    TValue<string> confirmationDescription = null
    )
    {

      var request = GetStuffPriceDiscrepancy(id: id);

      if (confirmationDescription != null)
        request.ConfirmationDescription = confirmationDescription;
      if (stuffPriceDiscrepancyStatus != null)
      {
        request.Status = stuffPriceDiscrepancyStatus;
        request.ConfirmationDateTime = DateTime.UtcNow;
        request.ConfirmerUserId = App.Providers.Security.CurrentLoginData.UserId;
      }

      repository.Update(entity: request, rowVersion: request.RowVersion);
      return request;
    }
    #endregion
  }
}