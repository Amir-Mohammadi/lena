using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models;
using lena.Models.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Gets
    public IQueryable<IndicatorProviderNumberResult> GetIndicatorProviderNumbers(
        TValue<ProviderType> providerType = null,
        TValue<int> stuffId = null,
        TValue<int?> responsibleEmployeeId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null
        )
    {

      var supplies = App.Internals.Supplies;
      var purchaseOrders = supplies.GetPurchaseOrders(selector: e => e, providerType: providerType, isDelete: false, stuffId: stuffId, fromDate: fromDate, toDate: toDate); ; var purchaseOrderDetails = supplies.GetPurchaseOrderDetails(selector: e => e);
      var purchaseRequests = supplies.GetPurchaseRequests(selector: e => e, responsibleEmployeeId: responsibleEmployeeId);

      var purchaseOrderResults = from purchaseOrder in purchaseOrders
                                 select new
                                 {
                                   ResponsibleEmployeeId = (int?)null,
                                   ResponsibleEmployeeName = (string)null,
                                   StuffId = purchaseOrder.StuffId,
                                   StuffName = purchaseOrder.Stuff.Name,
                                   StuffCode = purchaseOrder.Stuff.Code,
                                   ProviderId = purchaseOrder.ProviderId,
                                   ProviderName = purchaseOrder.Provider.Name,
                                   Qty = purchaseOrder.Qty
                                 };
      if (responsibleEmployeeId != null)
      {
        purchaseOrderResults = from purchaseOrder in purchaseOrders
                               join purchaseOrderDetail in purchaseOrderDetails on purchaseOrder.Id
                                     equals purchaseOrderDetail.PurchaseOrderId
                               join purchaseRequest in purchaseRequests on purchaseOrderDetail.PurchaseRequestId
                                     equals purchaseRequest.Id
                               select new
                               {
                                 ResponsibleEmployeeId = purchaseRequest.ResponsibleEmployeeId,
                                 ResponsibleEmployeeName = purchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseRequest.ResponsibleEmployee.LastName,
                                 StuffId = purchaseOrder.StuffId,
                                 StuffName = purchaseOrder.Stuff.Name,
                                 StuffCode = purchaseOrder.Stuff.Code,
                                 ProviderId = purchaseOrder.ProviderId,
                                 ProviderName = purchaseOrder.Provider.Name,
                                 Qty = purchaseOrderDetail.Qty
                               };
      }
      var totalQtys = from purchaseOrderResult in purchaseOrderResults
                      group purchaseOrderResult by new
                      {
                        purchaseOrderResult.StuffId,
                      }
            into grp
                      select new
                      {
                        StuffId = grp.Key.StuffId,
                        RequestQty = grp.Sum(x => x.Qty),
                      };
      var providerGroupByQuerys = from purchaseOrderResult in purchaseOrderResults
                                  group purchaseOrderResult by new
                                  {
                                    purchaseOrderResult.StuffId,
                                    purchaseOrderResult.ProviderId,
                                    purchaseOrderResult.ResponsibleEmployeeId,
                                    purchaseOrderResult.ResponsibleEmployeeName,
                                    purchaseOrderResult.StuffName,
                                    purchaseOrderResult.StuffCode,
                                    purchaseOrderResult.ProviderName,


                                  }
                 into grp
                                  select new
                                  {
                                    StuffId = grp.Key.StuffId,
                                    ProviderId = grp.Key.ProviderId,
                                    ResponsibleEmployeeId = grp.Key.ResponsibleEmployeeId,
                                    ResponsibleEmployeeName = grp.Key.ResponsibleEmployeeName,
                                    StuffName = grp.Key.StuffName,
                                    StuffCode = grp.Key.StuffCode,
                                    ProviderName = grp.Key.ProviderName,
                                    Qty = grp.Sum(x => x.Qty),
                                    ProviderBuyCount = grp.Count()
                                  };

      var resultQuery = from providerGroupByQuery in providerGroupByQuerys
                        join totalQty in totalQtys on providerGroupByQuery.StuffId
                                  equals totalQty.StuffId
                        select new IndicatorProviderNumberResult
                        {
                          StuffId = providerGroupByQuery.StuffId,
                          StuffName = providerGroupByQuery.StuffName,
                          StuffCode = providerGroupByQuery.StuffCode,
                          ProviderName = providerGroupByQuery.ProviderName,
                          ProviderId = providerGroupByQuery.ProviderId,
                          ResponsibleEmployeeId = providerGroupByQuery.ResponsibleEmployeeId,
                          ResponsibleEmployeeName = providerGroupByQuery.ResponsibleEmployeeName,
                          Qty = providerGroupByQuery.Qty,
                          RequestQty = totalQty.RequestQty,
                          ProviderBuyCount = providerGroupByQuery.ProviderBuyCount,
                          QtyPercentage = (providerGroupByQuery.Qty / totalQty.RequestQty) * 100
                        };
      return resultQuery;
    }
    #endregion

    #region Search
    public IQueryable<IndicatorProviderNumberResult> SearchIndicatorProviderNumber(IQueryable<IndicatorProviderNumberResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.ProviderName.Contains(searchText) ||
            item.StuffName.Contains(searchText) ||
            item.StuffCode.Contains(searchText)
            );
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<IndicatorProviderNumberResult> SortIndicatorProviderNumberResult(IQueryable<IndicatorProviderNumberResult> query,
        SortInput<IndicatorProviderNumberSortType> sort)
    {
      switch (sort.SortType)
      {
        case IndicatorProviderNumberSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);
        case IndicatorProviderNumberSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case IndicatorProviderNumberSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case IndicatorProviderNumberSortType.ProviderName:
          return query.OrderBy(a => a.ProviderName, sort.SortOrder);
        case IndicatorProviderNumberSortType.ProviderId:
          return query.OrderBy(a => a.ProviderId, sort.SortOrder);
        case IndicatorProviderNumberSortType.QtyPercentage:
          return query.OrderBy(a => a.QtyPercentage, sort.SortOrder);
        case IndicatorProviderNumberSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case IndicatorProviderNumberSortType.RequestQty:
          return query.OrderBy(a => a.RequestQty, sort.SortOrder);
        case IndicatorProviderNumberSortType.ProviderBuyCount:
          return query.OrderBy(a => a.ProviderBuyCount, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

  }

}
