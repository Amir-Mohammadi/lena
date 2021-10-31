
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Production.ProductionEmployeeCostReport;
using System;
using System.Linq;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {

    #region Gets
    public IQueryable<ProductionEmployeeCostReportResult> GetProductionEmployeeCostReports(
        double employeeCostPerHour,
        TValue<int> stuffId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null)
    {


      var stuffs = App.Internals.SaleManagement.GetStuffs(e => e, id: stuffId);
      #region ProductionOperation
      var productionOperations = GetProductionOperations(e => e,
          stuffId: stuffId,
          fromDateTime: fromDateTime,
          toDateTime: toDateTime)
      .OrderByDescending(v => v.DateTime);


      #endregion


      #region Deviation coefficient  //ضریب انحراف

      #region Produced Time  // کارکرد نفر ساعت مفید
      var producedProductionOperations = productionOperations.Where(i => i.Production.Status == ProductionStatus.Produced && i.Production.StuffSerial.Status == StuffSerialStatus.Completed);
      var producedTime = producedProductionOperations.Any() ? producedProductionOperations.Sum(m => m.Time) : 0;
      #endregion

      #region Not Produced Time  // کارکرد نفر ساعت نیمه تمام
      var notProducedProductionOperations = productionOperations.Where(i => i.Production.Status != ProductionStatus.Produced && i.Production.StuffSerial.Status != StuffSerialStatus.Completed);
      var notProducedTime = notProducedProductionOperations.Any() ? notProducedProductionOperations.Sum(m => m.Time) : 0;
      #endregion

      #region Out Of Range Time //  کارکرد خارج از بازه 

      var productionIds = producedProductionOperations.Select((m) => m.ProductionId).Distinct().ToArray();
      //var outOfRangeProducedProductionOperation = GetProductionOperations(
      //                                       e => e,
      //                                       productionIds: productionIds,
      //                                       toDateTime: fromDateTime)
      //                                    
      //;

      // aghaye ghaffari dian

      var outRangeProductionOperations = GetProductionOperations(e => e,
              stuffId: stuffId,
              toDateTime: fromDateTime)


          .OrderByDescending(v => v.DateTime);

      var outOfRangeProducedProductionOperation = from producedProductionOperation in producedProductionOperations
                                                  join outRangeProductionOperation in outRangeProductionOperations on producedProductionOperation.ProductionId equals outRangeProductionOperation.ProductionId
                                                  select new
                                                  {
                                                    StuffId = producedProductionOperation.Production.StuffSerialStuffId,
                                                    ProductionId = outRangeProductionOperation.ProductionId,
                                                    Time = outRangeProductionOperation.Time
                                                  };


      var outOfRangePoProducedTime = outOfRangeProducedProductionOperation.Any() ? outOfRangeProducedProductionOperation.Sum(m => m.Time) : 0;
      #endregion

      #region Presence Time //زمان حضور

      var productionOperationEmployees = (from po in productionOperations
                                          from poe in po.ProductionOperationEmployeeGroup.ProductionOperationEmployees
                                          select poe.Employee.Code).Distinct();


      //var outOfRangeProductionOperationEmployees = (from item in outOfRangeProducedProductionOperation
      //                                                  /// اینارو بعد از تغییر کد غ اضافه کردم
      //                                              join outRangeProductionOperation in outRangeProductionOperations
      //                                              on item.StuffId equals outRangeProductionOperation.Production.StuffSerialStuffId
      //                                              from poe in outRangeProductionOperation.ProductionOperationEmployeeGroup.ProductionOperationEmployees
      //                                                  ///
      //                                              select poe.Employee.Code).Distinct();


      //var employeeCodes = productionOperationEmployees.Union(outOfRangeProductionOperationEmployees).ToArray();


      var workQuery = App.Internals.EmployeeAttendance.GetEmployeeWorkDetail(
                      employeeCodes: productionOperationEmployees.ToArray(),
                      fromDateTime: fromDateTime,
                      toDateTime: toDateTime);

      var presenceTime = workQuery.Any() ? (double?)workQuery.Sum(m => m.WorkTime) * 60 : 0;

      #endregion

      // ------------------  Deviation Coefficient = Presence Time / ( Produced Time + Out Of Range Time) - Not Produced Time  ------------------
      var deviationCoefficient = (double)(presenceTime / ((producedTime + outOfRangePoProducedTime) - notProducedTime));

      #endregion

      #region Main Query // گزارش هزینه پرسنل

      // Produced Qty

      var getProducedQtyResult = from productionOperation in producedProductionOperations
                                 group productionOperation by new
                                 {
                                   StuffId = productionOperation.Production.StuffSerialStuffId,
                                   ProductionId = productionOperation.ProductionId
                                 } into g
                                 select new
                                 {
                                   StuffId = g.Key.StuffId,
                                   ProductionId = g.Key.ProductionId
                                 };



      var getProducedQtyQuery = from productionOperation in getProducedQtyResult
                                group productionOperation by new
                                {
                                  StuffId = productionOperation.StuffId
                                } into g
                                select new
                                {
                                  StuffId = g.Key.StuffId,
                                  ProducedQty = g.Count()

                                };


      var producedGroupByRes = from productionOperation in producedProductionOperations
                               group productionOperation by new
                               {
                                 StuffId = productionOperation.Production.StuffSerialStuffId
                               } into g
                               select new ProductionEmployeeCostReportUnionResult
                               {

                                 StuffId = g.Key.StuffId,
                                 ProducedQty = 0,
                                 SumProducedPoTime = ((g.Sum(m => m.Time)) / 60) / 60,
                                 SumProducedOutOfRangePoTime = null,

                               };


      var outOfRangePoProducedGroupByRes = from productionOperation in outOfRangeProducedProductionOperation
                                           group productionOperation by new
                                           {
                                             StuffId = productionOperation.StuffId
                                           } into g
                                           select new ProductionEmployeeCostReportUnionResult
                                           {
                                             StuffId = g.Key.StuffId,
                                             ProducedQty = 0,
                                             SumProducedPoTime = null,
                                             SumProducedOutOfRangePoTime = ((g.Sum(m => m.Time)) / 60) / 60,

                                           };


      var employeeFunctionHourUnion = (from producedGroupByObj in producedGroupByRes
                                         // ---- Produced Qty -----
                                       join producedQtyObj in getProducedQtyQuery
                                       on producedGroupByObj.StuffId equals producedQtyObj.StuffId
                                       // ----
                                       join outOfRangePoProducedGroupByObj in outOfRangePoProducedGroupByRes
                                       on producedGroupByObj.StuffId equals outOfRangePoProducedGroupByObj.StuffId
                                       select new ProductionEmployeeCostReportUnionResult
                                       {
                                         StuffId = producedGroupByObj.StuffId,
                                         ProducedQty = producedQtyObj.ProducedQty,
                                         SumProducedPoTime = producedGroupByObj.SumProducedPoTime,
                                         SumProducedOutOfRangePoTime = outOfRangePoProducedGroupByObj.SumProducedOutOfRangePoTime

                                       }).Distinct();


      var joinResult = from unionObj in employeeFunctionHourUnion
                       join stuff in stuffs
                             on unionObj.StuffId equals stuff.Id
                       select new ProductionEmployeeCostReportResult
                       {
                         StuffId = stuff.Id,
                         StuffCode = stuff.Code,
                         StuffName = stuff.Name,
                         ProducedQty = unionObj.ProducedQty,
                         EmployeeFunctionHour = Math.Round((double)(unionObj.SumProducedPoTime + unionObj.SumProducedOutOfRangePoTime), 4),
                         ActualFunction = Math.Round((double)((unionObj.SumProducedPoTime + unionObj.SumProducedOutOfRangePoTime) * deviationCoefficient), 3),
                         TotalAmount = Math.Round((double)(((unionObj.SumProducedPoTime + unionObj.SumProducedOutOfRangePoTime) * deviationCoefficient) * employeeCostPerHour), 4),
                         StuffBasePrice = Math.Round((double)((double)((((unionObj.SumProducedPoTime + unionObj.SumProducedOutOfRangePoTime) * deviationCoefficient) * employeeCostPerHour)) / unionObj.ProducedQty), 4) // /ProducedQty

                       };

      #endregion


      return joinResult;
    }

    #endregion

    #region Sort
    public IOrderedQueryable<ProductionEmployeeCostReportResult> SortProductionEmployeeCostReportResult(
        IQueryable<ProductionEmployeeCostReportResult> query,
        SortInput<ProductionEmployeeCostReportSortType> sort)

    {
      switch (sort.SortType)
      {
        case ProductionEmployeeCostReportSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);
        case ProductionEmployeeCostReportSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ProductionEmployeeCostReportSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ProductionEmployeeCostReportSortType.ProducedQty:
          return query.OrderBy(a => a.ProducedQty, sort.SortOrder);
        case ProductionEmployeeCostReportSortType.EmployeeFunctionHour:
          return query.OrderBy(a => a.EmployeeFunctionHour, sort.SortOrder);
        case ProductionEmployeeCostReportSortType.ActualFunction:
          return query.OrderBy(a => a.ActualFunction, sort.SortOrder);
        case ProductionEmployeeCostReportSortType.TotalAmount:
          return query.OrderBy(a => a.TotalAmount, sort.SortOrder);
        case ProductionEmployeeCostReportSortType.StuffBasePrice:
          return query.OrderBy(a => a.StuffBasePrice, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion

    #region Search
    public IQueryable<ProductionEmployeeCostReportResult> SearchProductionEmployeeCostReportResult(
        IQueryable<ProductionEmployeeCostReportResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
      )
    {
      if (!string.IsNullOrEmpty(searchText))
        //query = from item in query
        //         item.StuffCode.Contains(searchText) ||
        //         item.OperationCode.Contains(searchText) ||
        //         item.OperationTitle.Contains(searchText) ||
        //         item.ProductionLineName.Contains(searchText)
        //        select item;
        if (advanceSearchItems.Any())
          query = query.Where(advanceSearchItems);
      return query;
    }


    #endregion
  }
}
