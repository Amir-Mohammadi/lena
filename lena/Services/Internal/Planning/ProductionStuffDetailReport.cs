using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.ProductionStuffDetailReport;
using System;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic;



using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Gets
    public IQueryable<ProductionStuffDetailReportResult> GetProductionStuffDetailReports(
        int? stuffId,
        int? stuffCategoryId,
        DateTime? fromDate,
        DateTime? toDate)
    {


      var productionStuffDetails = App.Internals.Production.GetProductionStuffDetails(
                stuffId: stuffId,
                fromDate: fromDate,
                toDate: toDate,
                billOfMaterialDetailType: BillOfMaterialDetailType.Material);


      var stuffs = App.Internals.SaleManagement.GetStuffs(selector: e => new { e.Id, e.Name, e.Code, e.Noun, e.StuffCategory, e.UnitTypeId },
               stuffCategoryId: stuffCategoryId);

      var units = App.Internals.ApplicationBase.GetUnits(selector: e => new { e.Name, e.UnitTypeId },
                isMainUnit: true);

      var newShoppings = App.Internals.WarehouseManagement.GetNewShoppings(selector: e => new { e.StuffId, e.DateTime },
                stuffId: stuffId);





      var productionStuffDetailQuerys = from productionStuffDetail in productionStuffDetails
                                        group productionStuffDetail by productionStuffDetail.StuffId into g
                                        select new
                                        {
                                          StuffId = g.Key,
                                          Qty = g.Sum(i => i.Qty * i.Unit.ConversionRatio),
                                        };


      var newShoppingStuffs = from newShooping in newShoppings
                              group newShooping by newShooping.StuffId into g
                              select new
                              {
                                StuffId = g.Key,
                                LastStoreReceiptDate = g.Max(i => i.DateTime)
                              };


      var result = from productionStuffDetailQuery in productionStuffDetailQuerys
                   join stuff in stuffs on productionStuffDetailQuery.StuffId equals stuff.Id
                   join unit in units on stuff.UnitTypeId equals unit.UnitTypeId
                   join newShoppingStuff in newShoppingStuffs on productionStuffDetailQuery.StuffId equals newShoppingStuff.StuffId into sr
                   from temp in sr.DefaultIfEmpty()
                   select new ProductionStuffDetailReportResult
                   {
                     StuffId = productionStuffDetailQuery.StuffId,
                     StuffName = stuff.Name,
                     StuffCode = stuff.Code,
                     StuffNoun = stuff.Noun,
                     UnitName = unit.Name,
                     StuffCategoryName = stuff.StuffCategory.Name,
                     Amount = productionStuffDetailQuery.Qty,
                     LastStoreReceiptDate = temp.LastStoreReceiptDate
                   };



      return result;

    }

    #endregion
    #region Sort
    public IOrderedQueryable<ProductionStuffDetailReportResult> SortProductionStuffDetailReportResult(
        IQueryable<ProductionStuffDetailReportResult> query,
        SortInput<ProductionStuffDetailReportSortType> sort)

    {
      switch (sort.SortType)
      {
        case ProductionStuffDetailReportSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ProductionStuffDetailReportSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ProductionStuffDetailReportSortType.StuffNoun:
          return query.OrderBy(a => a.StuffNoun, sort.SortOrder);
        case ProductionStuffDetailReportSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case ProductionStuffDetailReportSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case ProductionStuffDetailReportSortType.StuffCategoryName:
          return query.OrderBy(a => a.StuffCategoryName, sort.SortOrder);
        case ProductionStuffDetailReportSortType.Date:
          return query.OrderBy(a => a.LastStoreReceiptDate, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ProductionStuffDetailReportResult> SearchProductionStuffDetailReportResult(
        IQueryable<ProductionStuffDetailReportResult> query,
        string searchText,
         AdvanceSearchItem[] advanceSearchItems)

    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.StuffCode.Contains(searchText) ||
                 item.StuffName.Contains(searchText) ||
                 item.StuffNoun.Contains(searchText) ||
                 item.StuffCategoryName.Contains(searchText)
                select item;

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }

    #endregion
  }
}