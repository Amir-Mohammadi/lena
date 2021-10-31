using lena.Models.Common;
using System;
using System.Linq;
using lena.Domains.Enums;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Models.Accounting.FinancialDocument;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Search
    public IQueryable<ProviderFinancialDocumentResult> SearchProviderFinancialDocument(
        IQueryable<ProviderFinancialDocumentResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.PlanCode.Contains(searchText) ||
                item.ProviderName.Contains(searchText)
                select item;

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProviderFinancialDocumentResult> SortProviderFinancialDocumentResult(
        IQueryable<ProviderFinancialDocumentResult> query,
        SortInput<ProviderFinancialDocumentSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProviderFinancialDocumentSortType.ProviderCode:
          return query.OrderBy(a => a.ProviderCode, sort.SortOrder);
        case ProviderFinancialDocumentSortType.ProviderName:
          return query.OrderBy(a => a.ProviderName, sort.SortOrder);
        case ProviderFinancialDocumentSortType.PlanCode:
          return query.OrderBy(a => a.PlanCode, sort.SortOrder);
        case ProviderFinancialDocumentSortType.CostSum:
          return query.OrderBy(a => a.CostSum, sort.SortOrder);
        case ProviderFinancialDocumentSortType.CurrencyTitle:
          return query.OrderBy(a => a.CurrencyTitle, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToResult
    public IQueryable<ProviderFinancialDocumentResult> GetProviderCosts(
        TValue<int> providerId = null,
        TValue<string> planCode = null)
    {

      var purchaseOserCosts = GetPurchaseOrderCosts(
                    selector: e => new
                    {
                      Amount = e.Amount,
                      CurrencyId = e.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId,
                      CurrencyTitle = e.FinancialDocumentCost.FinancialDocument.FinancialAccount.Currency.Title,
                      ProviderId = e.PurchaseOrder.ProviderId,
                      ProviderCode = e.PurchaseOrder.Provider.Code,
                      ProviderName = e.PurchaseOrder.Provider.Name,
                      PurchaseOrderDetails = e.PurchaseOrder.PurchaseOrderDetails
                    },
                    providerId: providerId,
                    isDelete: false);

      var purchaseOrderDetailsCosts =
                from cost in purchaseOserCosts
                let sumOfQty = cost.PurchaseOrderDetails.Sum(i => i.Qty)
                from purchaseOrderDetial in cost.PurchaseOrderDetails
                let ratio = purchaseOrderDetial.Qty / sumOfQty
                select new
                {
                  Amount = cost.Amount * ratio,
                  CurrencyId = cost.CurrencyId,
                  CurrencyTitle = cost.CurrencyTitle,
                  ProviderId = cost.ProviderId,
                  ProviderCode = cost.ProviderCode,
                  ProviderName = cost.ProviderName,
                  PlanCode = purchaseOrderDetial.PurchaseRequest.PlanCode.Code
                };

      var cargoCosts = GetCargoCosts(
                    selector: e => new
                    {
                      Amount = e.Amount,
                      CurrencyId = e.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId,
                      CurrencyTitle = e.FinancialDocumentCost.FinancialDocument.FinancialAccount.Currency.Title,
                      ProviderId = e.CargoItem.PurchaseOrder.ProviderId,
                      ProviderCode = e.CargoItem.PurchaseOrder.Provider.Code,
                      ProviderName = e.CargoItem.PurchaseOrder.Provider.Name,
                      PurchaseOrderDetails = e.CargoItem.PurchaseOrder.PurchaseOrderDetails
                    },
                    providerId: providerId,
                    isDelete: false);

      var cargoDetailsCosts =
                from cost in cargoCosts
                let sumOfQty = cost.PurchaseOrderDetails.Sum(i => i.Qty)
                from purchaseOrderDetial in cost.PurchaseOrderDetails
                let ratio = purchaseOrderDetial.Qty / sumOfQty
                select new
                {
                  Amount = cost.Amount * ratio,
                  CurrencyId = cost.CurrencyId,
                  CurrencyTitle = cost.CurrencyTitle,
                  ProviderId = cost.ProviderId,
                  ProviderCode = cost.ProviderCode,
                  ProviderName = cost.ProviderName,
                  PlanCode = purchaseOrderDetial.PurchaseRequest.PlanCode.Code
                };

      var ladingCosts = GetLadingCosts(
                    selector: e => new
                    {
                      Amount = e.Amount,
                      CurrencyId = e.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId,
                      CurrencyTitle = e.FinancialDocumentCost.FinancialDocument.FinancialAccount.Currency.Title,
                      ProviderId = e.LadingItem.CargoItem.PurchaseOrder.ProviderId,
                      ProviderCode = e.LadingItem.CargoItem.PurchaseOrder.Provider.Code,
                      ProviderName = e.LadingItem.CargoItem.PurchaseOrder.Provider.Name,
                      PurchaseOrderDetails = e.LadingItem.CargoItem.PurchaseOrder.PurchaseOrderDetails
                    },
                    providerId: providerId,
                    isDelete: false);

      var ladingDetailsCosts =
                from cost in ladingCosts
                let sumOfQty = cost.PurchaseOrderDetails.Sum(i => i.Qty)
                from purchaseOrderDetial in cost.PurchaseOrderDetails
                let ratio = purchaseOrderDetial.Qty / sumOfQty
                select new
                {
                  Amount = cost.Amount * ratio,
                  CurrencyId = cost.CurrencyId,
                  CurrencyTitle = cost.CurrencyTitle,
                  ProviderId = cost.ProviderId,
                  ProviderCode = cost.ProviderCode,
                  ProviderName = cost.ProviderName,
                  PlanCode = purchaseOrderDetial.PurchaseRequest.PlanCode.Code
                };

      var allCosts = purchaseOrderDetailsCosts.Union(cargoDetailsCosts).Union(ladingDetailsCosts);

      var groupedCosts = from cost in allCosts
                         group cost by new
                         {
                           cost.CurrencyId,
                           cost.CurrencyTitle,
                           cost.ProviderId,
                           cost.ProviderCode,
                           cost.ProviderName,
                           cost.PlanCode
                         }
                into grp
                         select new ProviderFinancialDocumentResult
                         {
                           CostSum = grp.Sum(g => g.Amount),
                           CurrencyId = grp.Key.CurrencyId,
                           CurrencyTitle = grp.Key.CurrencyTitle,
                           ProviderId = grp.Key.ProviderId,
                           ProviderCode = grp.Key.ProviderCode,
                           ProviderName = grp.Key.ProviderName,
                           PlanCode = grp.Key.PlanCode
                         };

      if (planCode != null)
        groupedCosts = groupedCosts.Where(i => i.PlanCode == planCode);

      return groupedCosts;
    }
    #endregion
  }
}
