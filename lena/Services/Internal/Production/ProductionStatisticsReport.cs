
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Production.ProductionStatisticsReport;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Internals.Planning;
using Microsoft.EntityFrameworkCore;
//using Parlar.DAL.Repositories;
using lena.Services.Core;
//using System.Data.Entity;
using Stimulsoft.Report.Dialogs;
//using System.Data.Entity.SqlServer;
using System.Collections.Generic;
// using System.Activities.Statements;
using lena.Domains.Enums.SortTypes;
using lena.Models.Production.ProductionOperation;
using lena.Models.Supplies.PurchaseOrder;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region GetProductionStatisticsCharts
    public IQueryable<ProductionStatisticsChartResult> GetProductionStatisticsCharts(
        int stuffId,
        int productionLineId,
        DateTime fromDateTime,
        DateTime? toDateTime,
        int? operationId)
    {
      var saleManagement = App.Internals.SaleManagement;
      var planning = App.Internals.Planning;
      var productionOperations = GetProductionOperations(
                         selector: e => new
                         {
                           e.Operation.IsCorrective,
                           e.OperationId,
                           e.Time,
                           e.ProductionOperatorId,
                           Qty = (e.Production.StuffSerial.InitQty - e.Production.StuffSerial.PartitionedQty) * e.Production.StuffSerial.InitUnit.ConversionRatio,
                           BillOfMaterialUnitConversionRatio = e.Production.StuffSerial.BillOfMaterial.Unit.ConversionRatio
                         },
                         fromDateTime: fromDateTime,
                         toDateTime: toDateTime,
                         operationId: operationId,
                         productionLineId: productionLineId,
                         stuffId: stuffId,
                         notHasStatuses: new[] { ProductionOperationStatus.Faild, ProductionOperationStatus.QualityControlFaild });
      var groupedTotalOperationsQuery = from productionOperation in productionOperations
                                        where productionOperation.IsCorrective == false
                                        group productionOperation by productionOperation.OperationId into g
                                        select new
                                        {
                                          OperationId = g.Key,
                                          OperationCount = g.Sum(i => i.Qty / i.BillOfMaterialUnitConversionRatio),
                                          Time = g.Average(i => i.Time),
                                          LatestProductionOperatorId = g.Max(i => i.ProductionOperatorId)
                                        };
      var operations = planning.GetOperations(e => e);
      if (toDateTime == null || toDateTime > DateTime.UtcNow)
        toDateTime = DateTime.UtcNow;
      var operationDurations = App.Internals.Production.CalculateTimeForProdcutionLineOperation(
                productionLineId: productionLineId,
                fromDateTime: fromDateTime,
                toDateTime: toDateTime.Value,
                stuffId: stuffId,
                operationId: operationId);
      var faildStatus = ProductionOperationStatus.Faild | ProductionOperationStatus.QualityControlFaild;
      var accumulationProductionOperations = from po in repository.GetQuery<ProductionOperation>()
                                             where po.DateTime >= fromDateTime
                                                   && po.DateTime <= toDateTime
                                                   && po.Production.StuffSerialStuffId == stuffId
                                                   && po.Production.Status == ProductionStatus.InProduction
                                                   && (po.Status & faildStatus) == 0
                                                   && po.Production.ProductionOrder.WorkPlanStep.ProductionLineId == productionLineId
                                             select new
                                             {
                                               po.OperationId,
                                               po.Id,
                                               po.Production.StuffSerialStuffId,
                                               po.Production.StuffSerialCode
                                             };
      var groupedProductionOperations = from po in accumulationProductionOperations
                                        group po by new
                                        {
                                          po.StuffSerialStuffId,
                                          po.StuffSerialCode
                                        } into gItems
                                        select new
                                        {
                                          StuffSerialStuffId = gItems.Key.StuffSerialStuffId,
                                          StuffSerialCode = gItems.Key.StuffSerialCode,
                                          ProductionOperationId = gItems.Max(r => r.Id)
                                        };
      var groupedSerialCountOfOperationsQuery = (from gpo in groupedProductionOperations
                                                 join po in accumulationProductionOperations on gpo.ProductionOperationId equals po.Id
                                                 group po by po.OperationId into gItems
                                                 select new
                                                 {
                                                   OperationId = gItems.Key,
                                                   Accumulation = gItems.Count()
                                                 });
      var productionOperatorIds = groupedTotalOperationsQuery.Select(i => i.LatestProductionOperatorId).ToArray();
      var productionOperatorsInfos = from item in repository.GetQuery<ProductionOperatorMachineEmployee>()
                                     where productionOperatorIds.Contains(item.ProductionOperatorId)
                                     group item by item.ProductionOperatorId into gItems
                                     select new
                                     {
                                       ProductionOperatorId = gItems.Key,
                                       OperatorMachineCount = gItems.Select(m => m.EmployeeId).Distinct().Count()
                                     };
      var groupsResult = from groupedTotalOperationQuery in groupedTotalOperationsQuery
                         join poi in productionOperatorsInfos on groupedTotalOperationQuery.LatestProductionOperatorId equals poi.ProductionOperatorId
                         join po in repository.GetQuery<ProductionOperator>() on poi.ProductionOperatorId equals po.Id
                         join groupedSerialCountOfOperationQuery in groupedSerialCountOfOperationsQuery
                               on groupedTotalOperationQuery.OperationId equals groupedSerialCountOfOperationQuery.OperationId into groupedResultLeft
                         from item in groupedResultLeft.DefaultIfEmpty()
                         select new
                         {
                           OperationId = groupedTotalOperationQuery.OperationId,
                           TotalOperationCount = groupedTotalOperationQuery.OperationCount,
                           OperatorMachineCount = poi.OperatorMachineCount,
                           Index = po.OperationSequence.Index,
                           Time = groupedTotalOperationQuery.Time == 0 ? 1 : groupedTotalOperationQuery.Time,
                           Accumulation = ((int?)item.Accumulation) ?? 0
                         };
      var query = from gResult in groupsResult
                  join operation in operations on gResult.OperationId equals operation.Id
                  join tod in operationDurations on gResult.OperationId equals tod.OperationId into tods
                  from od in tods.DefaultIfEmpty()
                  orderby gResult.Index
                  let employeeTotalTime = od.Duration ?? 0d
                  let operationTargetCount = employeeTotalTime / gResult.Time
                  let efficiency = employeeTotalTime > 0 ? gResult.TotalOperationCount / operationTargetCount : 0d
                  select new ProductionStatisticsChartResult()
                  {
                    OperationSequenceIndex = gResult.Index,
                    OperationDefaultTime = gResult.Time,
                    OperationId = gResult.OperationId,
                    OperationCode = operation.Code,
                    OperationTitle = operation.Title,
                    Accumulation = gResult.Accumulation,
                    OperationTotalCount = gResult.TotalOperationCount,
                    EmployeeTotalTime = employeeTotalTime,
                    OperationTargetCount = Math.Round(operationTargetCount, 0),
                    OperatorMachineCount = gResult.OperatorMachineCount,
                    Efficiency = Math.Round(efficiency, 2)
                  };
      return query;
    }
    #endregion
    #region SortProductionStatisticsChartResult
    public IOrderedQueryable<ProductionStatisticsChartResult> SortProductionStatisticsChartResult(
        IQueryable<ProductionStatisticsChartResult> query,
        SortInput<ProductionStatisticsChartSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionStatisticsChartSortType.OperationSequenceIndex: return query.OrderBy(a => a.OperationSequenceIndex, sort.SortOrder);
        case ProductionStatisticsChartSortType.OperationDefaultTime: return query.OrderBy(a => a.OperationDefaultTime, sort.SortOrder);
        case ProductionStatisticsChartSortType.OperationCode: return query.OrderBy(a => a.OperationCode, sort.SortOrder);
        case ProductionStatisticsChartSortType.OperationTitle: return query.OrderBy(a => a.OperationTitle, sort.SortOrder);
        case ProductionStatisticsChartSortType.Accumulation: return query.OrderBy(a => a.Accumulation, sort.SortOrder);
        case ProductionStatisticsChartSortType.OperationTotalCount: return query.OrderBy(a => a.OperationTotalCount, sort.SortOrder);
        case ProductionStatisticsChartSortType.OperationTargetCount: return query.OrderBy(a => a.OperationTargetCount, sort.SortOrder);
        case ProductionStatisticsChartSortType.OperatorMachineCount: return query.OrderBy(a => a.OperatorMachineCount, sort.SortOrder);
        case ProductionStatisticsChartSortType.EmployeeTotalTime: return query.OrderBy(a => a.EmployeeTotalTime, sort.SortOrder);
        case ProductionStatisticsChartSortType.Efficiency: return query.OrderBy(a => a.Efficiency, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region SearchProductionStatisticsChartResult
    public IQueryable<ProductionStatisticsChartResult> SearchProductionStatisticsChartResult(
        IQueryable<ProductionStatisticsChartResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
      )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.OperationCode.Contains(searchText) ||
                 item.OperationTitle.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Gets
    public IQueryable<ProductionStatisticsReportResult> GetProductionStatisticsReports(
        TValue<int> stuffId = null,
        TValue<int> operationId = null,
        TValue<int> productionLineId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<string> stuffSerial = null,
        TValue<string> fromStuffSerial = null,
        TValue<string> toStuffSerial = null,
        TValue<bool> groupByProductionTerminalId = null,
        TValue<bool> groupBySerial = null,
        TValue<bool> groupByDeteTime = null)
    {
      var saleManagement = App.Internals.SaleManagement;
      var planning = App.Internals.Planning;
      var productionOperations = GetProductionOperations(e => e,
                    fromDateTime: fromDateTime,
                    toDateTime: toDateTime,
                    operationId: operationId,
                    productionLineId: productionLineId,
                    stuffId: stuffId,
                    notHasStatuses: new[] { ProductionOperationStatus.Faild });
      var stuffs = saleManagement.GetStuffs(e => e);
      var operations = planning.GetOperations(e => e);
      var productionTerminals = GetProductionTerminals(e => e);
      var productionLines = App.Internals.Planning.GetProductionLines();
      var stuffSerials = App.Internals.WarehouseManagement.GetStuffSerials(selector: e => e,
                    stuffId: stuffId,
                    serial: stuffSerial);
      var RawgroupQuery = from productionOperation in productionOperations
                          group productionOperation by new
                          {
                            productionOperation.OperationId,
                            productionOperation.Production.ProductionOrder.WorkPlanStep.ProductionLineId,
                            productionOperation.Production.StuffSerialStuffId,
                            productionOperation.Production.StuffSerialCode,
                            productionOperation.ProductionTerminalId
                          } into g
                          select new RawProductionStatisticsReports()
                          {
                            ProductionLineId = g.Key.ProductionLineId,
                            ProductionTerminalId = g.Key.ProductionTerminalId,
                            OperationId = g.Key.OperationId,
                            StuffId = g.Key.StuffSerialStuffId,
                            stuffSerial = g.FirstOrDefault().Production.StuffSerial,
                            StuffSerialCode = g.Key.StuffSerialCode,
                            DateTime = g.FirstOrDefault().DateTime,
                            Qty = g.Sum(i => i.Qty.HasValue ? i.Qty.Value : 0)
                          };
      #region Define Key Selector
      Expression<Func<RawProductionStatisticsReports, ProductionStatisticsReportsGroupKey>> groupKeySelector =
      i => new ProductionStatisticsReportsGroupKey
      {
        StuffId = i.StuffId,
        ProductionLineId = i.ProductionLineId,
        OperationId = i.OperationId,
        StuffSerialCode = null,
        DateTime = null,
        ProductionTerminalId = null
      };
      if (groupBySerial && groupByDeteTime && groupByProductionTerminalId)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          OperationId = i.OperationId,
          StuffSerialCode = i.StuffSerialCode,
          DateTime = i.DateTime,
          ProductionTerminalId = i.ProductionTerminalId
        };
      if (groupBySerial && groupByDeteTime && !groupByProductionTerminalId)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          OperationId = i.OperationId,
          StuffSerialCode = i.StuffSerialCode,
          DateTime = i.DateTime,
          ProductionTerminalId = null
        };
      if (groupBySerial && !groupByDeteTime && groupByProductionTerminalId)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          OperationId = i.OperationId,
          StuffSerialCode = i.StuffSerialCode,
          DateTime = null,
          ProductionTerminalId = i.ProductionTerminalId
        };
      if (!groupBySerial && groupByDeteTime && groupByProductionTerminalId)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          OperationId = i.OperationId,
          StuffSerialCode = null,
          DateTime = i.DateTime,
          ProductionTerminalId = i.ProductionTerminalId
        };
      if (groupBySerial && !groupByDeteTime && !groupByProductionTerminalId)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          OperationId = i.OperationId,
          StuffSerialCode = i.StuffSerialCode,
          DateTime = null,
          ProductionTerminalId = null
        };
      if (!groupBySerial && !groupByDeteTime && groupByProductionTerminalId)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          OperationId = i.OperationId,
          StuffSerialCode = null,
          DateTime = null,
          ProductionTerminalId = i.ProductionTerminalId
        };
      if (!groupBySerial && groupByDeteTime && !groupByProductionTerminalId)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          OperationId = i.OperationId,
          StuffSerialCode = null,
          DateTime = i.DateTime,
          ProductionTerminalId = null
        };
      #endregion
      var FgroupQuery = RawgroupQuery.GroupBy(groupKeySelector)
      .Select(groupItems => new
      {
        ProductionLineId = groupItems.Key.ProductionLineId,
        ProductionTerminalId = groupItems.Key.ProductionTerminalId,
        OperationId = groupItems.Key.OperationId,
        StuffId = groupItems.Key.StuffId,
        StuffSerialCode = groupItems.Key.StuffSerialCode,
        DateTime = groupItems.Key.DateTime,
        OperationCount = groupItems.Sum(i => i.Qty),
        Qty = groupItems.Sum(i => (i.stuffSerial.InitQty - i.stuffSerial.PartitionedQty) * i.stuffSerial.InitUnit.ConversionRatio),
      });
      var query = from item in FgroupQuery
                  join tpt in productionTerminals on item.ProductionTerminalId equals tpt.Id into tempProductionTerminals
                  from productionTerminal in tempProductionTerminals.DefaultIfEmpty()
                  join productionLine in productionLines on item.ProductionLineId equals productionLine.Id
                  join operation in operations on item.OperationId equals operation.Id
                  join stuff in stuffs on item.StuffId equals stuff.Id
                  join tss in stuffSerials on new { StuffId = item.StuffId, StuffSerialCode = item.StuffSerialCode }
                        equals new { StuffId = (int?)tss.StuffId, StuffSerialCode = (long?)tss.Code }
                            into tStuffSerials
                  from stuffSerialItem in tStuffSerials.DefaultIfEmpty()
                  from unit in stuff.UnitType.Units
                  where unit.IsMainUnit == true
                  select new ProductionStatisticsReportResult()
                  {
                    ProductionLineId = item.ProductionLineId,
                    ProductionLineName = productionLine.Name,
                    ProductionTerminalDescription = productionTerminal.Description,
                    StuffId = stuff.Id,
                    StuffName = stuff.Name,
                    StuffNoun = stuff.Noun,
                    StuffCode = stuff.Code,
                    OperationCode = operation.Code,
                    OperationTitle = operation.Title,
                    OperationCount = item.OperationCount,
                    Qty = item.Qty,
                    StuffSerialCode = item.StuffSerialCode,
                    StuffSerial = stuffSerialItem.Serial,
                    DateTime = item.DateTime
                  };
      if (!string.IsNullOrWhiteSpace(fromStuffSerial))
      {
        var fromSerial1 = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(fromStuffSerial);
        query = query.Where(i => i.StuffSerial.CompareTo(fromSerial1) >= 0);
      }
      if (!string.IsNullOrWhiteSpace(toStuffSerial))
      {
        var toSerial1 = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(toStuffSerial);
        query = query.Where(i => i.StuffSerial.CompareTo(toSerial1) <= 0);
      }
      return query;
    }
    #endregion
    #region GetProductionLineEfficiency
    public IQueryable<ProductionLineEfficiencyReportsResult> GetProductionLineEfficiency(
        int? stuffId,
        int productionLineId,
        DateTime fromDateTime,
        DateTime toDateTime)
    {
      if (toDateTime == null)
        toDateTime = DateTime.UtcNow;
      var saleManagement = App.Internals.SaleManagement;
      var planning = App.Internals.Planning;
      var productionOperations = GetProductionOperations(e => new
      {
        e.Operation.IsCorrective,
        e.OperationId,
        e.Time,
        e.ProductionOperatorId,
        ProductionLineId = e.Production.ProductionOrder.WorkPlanStep.ProductionLineId,
        StuffId = e.Production.StuffSerialStuffId,
        Qty = (e.Production.StuffSerial.InitQty - e.Production.StuffSerial.PartitionedQty) * e.Production.StuffSerial.InitUnit.ConversionRatio,
        BillOfMaterialUnitConversionRatio = e.Production.StuffSerial.BillOfMaterial.Unit.ConversionRatio,
        e.ProductionId,
      },
                    fromDateTime: fromDateTime,
                    toDateTime: toDateTime,
                    productionLineId: productionLineId,
                    stuffId: stuffId,
                    notHasStatuses: new[] { ProductionOperationStatus.Faild, ProductionOperationStatus.QualityControlFaild });
      int totalOperatorMachineCount = 0;
      if (stuffId != null)
      {
        var groupedTotalOperationsQuery = from productionOperation in productionOperations
                                          where productionOperation.IsCorrective == false
                                          group productionOperation by productionOperation.OperationId into g
                                          select new
                                          {
                                            OperationId = g.Key,
                                            OperationCount = g.Sum(i => i.Qty / i.BillOfMaterialUnitConversionRatio),
                                            Time = g.Average(i => i.Time),
                                            LatestProductionOperatorId = g.Max(i => i.ProductionOperatorId)
                                          };
        var productionOperatorIds = groupedTotalOperationsQuery.Select(i => i.LatestProductionOperatorId).ToArray();
        var productionOperatorsInfos = from item in repository.GetQuery<ProductionOperatorMachineEmployee>()
                                       where productionOperatorIds.Contains(item.ProductionOperatorId)
                                       group item by item.ProductionOperatorId into gItems
                                       select new
                                       {
                                         ProductionOperatorId = gItems.Key,
                                         OperatorMachineCount = gItems.Select(m => m.EmployeeId).Distinct().Count()
                                       };
        totalOperatorMachineCount = productionOperatorsInfos.Sum(i => i.OperatorMachineCount);
      }
      var RawgroupQuery = from productionOperation in productionOperations
                          where productionOperation.IsCorrective == false
                          select new RawProductionLineEfficiencyReports()
                          {
                            Qty = productionOperation.Qty / productionOperation.BillOfMaterialUnitConversionRatio,
                            ProductionOperationTime = productionOperation.Time,
                            ProductionLineId = productionOperation.ProductionLineId,
                            OperationId = productionOperation.OperationId,
                            StuffId = productionOperation.StuffId
                          };
      if (toDateTime > DateTime.UtcNow)
        toDateTime = DateTime.UtcNow;
      var calculateTimeForProdcutionLine = App.Internals.Production.CalculateTimeForProdcutionLine(
                productionLineId: productionLineId,
                fromDateTime: fromDateTime,
                toDateTime: toDateTime,
                stuffId: stuffId);
      float totalSequenceTimes = 0.0F;
      var productionId = productionOperations.Max(i => (int?)i.ProductionId);
      if (productionId != null)
      {
        var productionInfo = GetProductions(i => new { i.Id, i.ProductionOrderId, i.ProductionOrder.WorkPlanStepId }, id: productionId)
              .FirstOrDefault();
        totalSequenceTimes = planning.GetOperationSequences(i => i.DefaultTime, workPlanStepId: productionInfo.WorkPlanStepId)
              .Sum();
      }
      #region Define Key Selector
      Expression<Func<RawProductionLineEfficiencyReports, ProductionStatisticsReportsGroupKey>> groupKeySelector = null;
      if (stuffId == null && productionLineId != null)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          ProductionLineId = i.ProductionLineId
        };
      if (stuffId != null && productionLineId == null)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId
        };
      if (stuffId != null && productionLineId != null)
        groupKeySelector = i => new ProductionStatisticsReportsGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId
        };
      #endregion
      var groupedQuery = RawgroupQuery.GroupBy(groupKeySelector);
      IQueryable<ProductionStatisticsGroupResult> result = null;
      if (stuffId != null && productionLineId != null)
      {
        result = groupedQuery.Select(i => new ProductionStatisticsGroupResult()
        {
          ProductionLineId = i.Key.ProductionLineId,
          StuffId = i.Key.StuffId,
          TotalOperationsSequenceTime = totalSequenceTimes,
          TotalProductionOperationTime = i.Sum(x => x.Qty * x.ProductionOperationTime),
          TotalOperation = i.Count(),
        });
      }
      if (stuffId != null && productionLineId == null)
      {
        result = groupedQuery.Select(i => new ProductionStatisticsGroupResult()
        {
          StuffId = i.Key.StuffId,
          TotalOperationsSequenceTime = totalSequenceTimes,
          TotalProductionOperationTime = i.Sum(x => x.Qty * x.ProductionOperationTime),
          TotalOperation = i.Count(),
        });
      }
      if (stuffId == null && productionLineId != null)
      {
        result = groupedQuery.Select(i => new ProductionStatisticsGroupResult()
        {
          ProductionLineId = i.Key.ProductionLineId,
          TotalOperationsSequenceTime = totalSequenceTimes,
          TotalProductionOperationTime = i.Sum(x => x.Qty * x.ProductionOperationTime),
          TotalOperation = i.Count(),
        });
      }
      var stuffs = saleManagement.GetStuffs(e => e);
      var productionLines = App.Internals.Planning.GetProductionLines();
      if (stuffId == null && productionLineId != null)
      {
        return (from item in result
                join productionLine in productionLines on item.ProductionLineId equals productionLine.Id
                select new ProductionLineEfficiencyReportsResult()
                {
                  ProductionLineId = item.ProductionLineId,
                  ProductionLineName = productionLine.Name,
                  TotalProductionOperationTime = item.TotalProductionOperationTime,
                  TotalOperationsSequenceTime = item.TotalOperationsSequenceTime,
                  TotalOperation = item.TotalOperation,
                  //زمان حضور
                  AttendanceTime = calculateTimeForProdcutionLine.TotalSeconds,
                  //تعداد پرسنل حاضر
                  TotalOperatorMachineCount = totalOperatorMachineCount
                });
      }
      else if (stuffId != null && productionLineId == null)
      {
        return (from item in result
                join stuff in stuffs on item.StuffId equals stuff.Id
                select new ProductionLineEfficiencyReportsResult()
                {
                  TotalProductionOperationTime = item.TotalProductionOperationTime,
                  TotalOperationsSequenceTime = item.TotalOperationsSequenceTime,
                  TotalOperation = item.TotalOperation,
                  StuffId = stuff.Id,
                  StuffName = stuff.Name,
                  StuffNoun = stuff.Noun,
                  StuffCode = stuff.Code,
                  //زمان حضور
                  AttendanceTime = calculateTimeForProdcutionLine.TotalSeconds,
                  //تعداد پرسنل حاضر
                  TotalOperatorMachineCount = totalOperatorMachineCount
                });
      }
      return (from item in result
              join productionLine in productionLines on item.ProductionLineId equals productionLine.Id
              join stuff in stuffs on item.StuffId equals stuff.Id
              select new ProductionLineEfficiencyReportsResult()
              {
                ProductionLineId = item.ProductionLineId,
                ProductionLineName = productionLine.Name,
                TotalProductionOperationTime = item.TotalProductionOperationTime,
                TotalOperationsSequenceTime = item.TotalOperationsSequenceTime,
                TotalOperation = item.TotalOperation,
                StuffId = stuff.Id,
                StuffName = stuff.Name,
                StuffNoun = stuff.Noun,
                StuffCode = stuff.Code,
                //زمان حضور
                AttendanceTime = calculateTimeForProdcutionLine.TotalSeconds,
                //تعداد پرسنل حاضر
                TotalOperatorMachineCount = totalOperatorMachineCount
              });
    }
    public IQueryable<ProductionEmployeeEfficiencyResult> GetProductionEmployeeEfficiencys(
        DateTime fromDateTime,
        DateTime toDateTime,
        TValue<int> employeeId = null
        )
    {
      string employeeCode = null;
      if (employeeId != null)
      {
        var employee = App.Internals.UserManagement.GetEmployee(employeeId.Value);
        employeeCode = employee.Code;
      }
      #region GetIntervalQuery
      //لیست بازه های ثبت بارکد
      var intervalsQuery = GetProductionLineEmployeeIntervals(e => new
      {
        Id = e.Id,
        EmployeeId = e.EmployeeId,
        EmployeeCode = e.Employee.Code,
        FirstSerialTime = e.EntranceDateTime,
        LastSerialTime = e.ExitDateTime,
        LineWorkTime = EF.Functions.DiffMinutes((e.EntranceDateTime < fromDateTime ? fromDateTime : e.EntranceDateTime), (e.ExitDateTime > toDateTime ? toDateTime : e.ExitDateTime))
      },
      employeeId: employeeId,
      fromDateTime: fromDateTime,
      toDateTime: toDateTime);
      var lineTotalWorkTimeQeury = from item in intervalsQuery
                                   group item by item.EmployeeId into gItems
                                   select
                                         new
                                         {
                                           EmployeeId = gItems.Key,
                                           TotalTime = gItems.Sum(i => i.LineWorkTime)
                                         };
      #endregion
      #region GetWorkTimeQuery
      var employeeWorkDetail = App.Internals.EmployeeAttendance.GetEmployeeWorkDetail(fromDateTime: fromDateTime, toDateTime: toDateTime, employeeCode: employeeCode);
      var employeeWorkTime = from interval in employeeWorkDetail
                             group interval by interval.EmployeeCode into g
                             select new
                             {
                               EmployeeCode = g.Key,
                               TotalTime = g.Sum(i => i.WorkTime)
                             };
      #endregion
      #region GetProductionOperationTime
      var productionOperations = GetProductionOperations(selector: e => e,
        fromDateTime: fromDateTime,
        toDateTime: toDateTime);
      var productionOperationEmployeeGroupCount = from poe in repository.GetQuery<ProductionOperationEmployee>()
                                                  group poe by poe.ProductionOperationEmployeeGroupId into g
                                                  select new
                                                  {
                                                    ProductionOperationEmployeeGroupId = g.Key,
                                                    EmployeesCount = g.Count()
                                                  };
      var operationTimeQuery = from po in productionOperations
                               from poe in po.ProductionOperationEmployeeGroup.ProductionOperationEmployees
                               join poeCount in productionOperationEmployeeGroupCount on po.ProductionOperationEmployeeGroupId equals poeCount.ProductionOperationEmployeeGroupId
                               let qty = po.Qty * po.Production.StuffSerial.InitUnit.ConversionRatio /
                                     po.Production.StuffSerial.BillOfMaterial.Unit.ConversionRatio / po.Production.StuffSerial.BillOfMaterial.Value
                               let time = poeCount.EmployeesCount == 0 ? po.Time : po.Time / poeCount.EmployeesCount
                               let defaultTime = po.ProductionOperator.DefaultTime
                               select new
                               {
                                 EmployeeId = poe.EmployeeId,
                                 EmployeeCode = poe.Employee.Code,
                                 DefaultTime = poeCount.EmployeesCount == 0 ? defaultTime : defaultTime / poeCount.EmployeesCount,
                                 Time = time,
                                 DateTime = po.DateTime,
                                 Qty = qty.Value
                               };
      var employeeOperationTime = from operation in operationTimeQuery
                                  group operation by new { operation.EmployeeCode, operation.EmployeeId } into g
                                  select new
                                  {
                                    EmployeeId = g.Key.EmployeeId,
                                    EmployeeCode = g.Key.EmployeeCode,
                                    TotalTime = g.Sum(i => i.Qty * i.Time) /*in second*/ / 60.0,
                                  };
      #endregion
      var timeQuery = from operationTime in employeeOperationTime
                      join workTime in employeeWorkTime on operationTime.EmployeeCode equals workTime.EmployeeCode
                      join lineTime in lineTotalWorkTimeQeury on operationTime.EmployeeId equals lineTime.EmployeeId
                      select new
                      {
                        EmployeeCode = operationTime.EmployeeCode,
                        EmployeeId = operationTime.EmployeeId,
                        TotalLineWorkTime = lineTime.TotalTime,
                        TotalWorkTime = workTime.TotalTime,
                        TotalOperationTime = operationTime.TotalTime,
                      };
      var employeesQuery = App.Internals.UserManagement.GetEmployees(e => e);
      if (employeeId != null)
        operationTimeQuery = operationTimeQuery.Where(i => i.EmployeeId == employeeId);
      var query = from item in timeQuery
                  join employee in employeesQuery on item.EmployeeId equals employee.Id
                  select new ProductionEmployeeEfficiencyResult
                  {
                    EmployeeCode = employee.Code,
                    EmployeeFullName = employee.FirstName + " " + employee.LastName,
                    TotalOperationTime = item.TotalOperationTime / 60.0,
                    TotalPresenceTimeInCompany = item.TotalWorkTime / 60.0,
                    TotalPresenceTimeInProductionLine = item.TotalLineWorkTime / 60.0,
                    TotalEfficiency = item.TotalWorkTime == 0 ? 0 : (item.TotalOperationTime / item.TotalWorkTime) * 100,
                    EffectiveEfficiency = item.TotalLineWorkTime == 0 ? 0 : (item.TotalOperationTime / item.TotalLineWorkTime) * 100
                  };
      return query;
    }
    public IQueryable<ProductionEmployeeEfficiencyResult> GetProductionEmployeeEfficiencyWithDetail(
       DateTime fromDateTime,
       DateTime toDateTime,
       TValue<int> employeeId = null
       )
    {
      string employeeCode = null;
      if (employeeId != null)
      {
        var employee = App.Internals.UserManagement.GetEmployee(employeeId.Value);
        employeeCode = employee.Code;
      }
      var employeesQuery = App.Internals.UserManagement.GetEmployees(e => e);
      //لیست بازه های ثبت بارکد
      IQueryable<ProductionEmployeeIntervalGroupResult> employeeIntervalQuery = GetProductionLineEmployeeIntervals(e => new ProductionEmployeeIntervalGroupResult()
      {
        WorkId = e.Id,
        EmployeeId = e.EmployeeId,
        EmployeeCode = e.Employee.Code,
        EmployeeEnterTime = null,
        EmployeeExitTime = null,
        FirstSerialTime = e.EntranceDateTime,
        LastSerialTime = e.ExitDateTime,
        WorkTime = 0,
        WorkTimeInLine = 0,
      },
      employeeId: employeeId,
      fromDateTime: fromDateTime,
      toDateTime: toDateTime
      );
      var employeeWorkDetail = App.Internals.EmployeeAttendance
           .GetEmployeeWorkDetail(fromDateTime: fromDateTime, toDateTime: toDateTime, employeeCode: employeeCode);
      var dayDiffs = (toDateTime - fromDateTime).Days;
      employeeIntervalQuery = (from work in employeeWorkDetail
                               join interval in employeeIntervalQuery on work.EmployeeCode equals interval.EmployeeCode into left1
                               from interval in left1.DefaultIfEmpty()
                               where !((interval.FirstSerialTime <= work.FirstEnterTime && interval.LastSerialTime <= work.FirstEnterTime) || work.FirstEnterTime >= interval.FirstSerialTime)
                               group new { work.FirstEnterTime, work.LastExitTime, interval.FirstSerialTime, interval.LastSerialTime } by new { work.Id, interval.EmployeeId, interval.EmployeeCode } into g1
                               let workTimeInLine = g1.Sum(t => EF.Functions.DateDiffMinute((t.FirstSerialTime < fromDateTime ? fromDateTime : t.FirstSerialTime), (t.LastSerialTime > toDateTime ? toDateTime : t.LastSerialTime)) ?? 0)
                               let workTime = EF.Functions.DateDiffMinute(g1.Min(t => t.FirstEnterTime), g1.Max(t => t.LastExitTime)) ?? 0
                               let WorkTimeHour = workTime / 60.0
                               //به علت اینکه تفکیک روز کمی مشکل ساز هست
                               //همچنین محاسبه ضریب هم همیشه درست نیست مثلا 16 ساعت  کارکرد مربوط به یک روز باشه یا چند روز
                               //اگر چند روز باشه باید تایم استراحت چند روز کسر بشه در غیر این صورت یک روز
                               //که قابل تشخیص نیست
                               let lastWorkTime = workTime - (dayDiffs == 1 ? (WorkTimeHour <= 8 ? 20 : (WorkTimeHour >= 8 && WorkTimeHour <= 10 ? 60 : 80)) : 0)
                               select new ProductionEmployeeIntervalGroupResult()
                               {
                                 WorkId = g1.Key.Id,
                                 EmployeeId = g1.Key.EmployeeId,
                                 EmployeeCode = g1.Key.EmployeeCode,
                                 EmployeeEnterTime = g1.Min(t => t.FirstEnterTime),
                                 EmployeeExitTime = g1.Max(t => t.LastExitTime),
                                 FirstSerialTime = g1.Min(t => t.FirstSerialTime),
                                 LastSerialTime = g1.Max(t => t.LastSerialTime),
                                 WorkTime = lastWorkTime < 0 ? 0 : lastWorkTime,
                                 WorkTimeInLine = workTimeInLine,
                               }
                         );
      //var ll = employeeIntervalQuery.ToList();
      var productionOperations = GetProductionOperations(selector: e => e,
       fromDateTime: fromDateTime,
       toDateTime: toDateTime);
      var productionOperationEmployeeGroupCount = from poe in repository.GetQuery<ProductionOperationEmployee>()
                                                  group poe by poe.ProductionOperationEmployeeGroupId into g
                                                  select new
                                                  {
                                                    ProductionOperationEmployeeGroupId = g.Key,
                                                    EmployeesCount = g.Count()
                                                  };
      var operationTimeQuery = from po in productionOperations
                               from poe in po.ProductionOperationEmployeeGroup.ProductionOperationEmployees
                               join poeCount in productionOperationEmployeeGroupCount on po.ProductionOperationEmployeeGroupId equals poeCount.ProductionOperationEmployeeGroupId
                               let qty = po.Qty * po.Production.StuffSerial.InitUnit.ConversionRatio /
                                     po.Production.StuffSerial.BillOfMaterial.Unit.ConversionRatio / po.Production.StuffSerial.BillOfMaterial.Value
                               let time = poeCount.EmployeesCount == 0 ? po.Time : po.Time / poeCount.EmployeesCount
                               let defaultTime = po.ProductionOperator.DefaultTime
                               select new
                               {
                                 EmployeeId = poe.EmployeeId,
                                 EmployeeCode = poe.Employee.Code,
                                 DefaultTime = poeCount.EmployeesCount == 0 ? (long?)defaultTime : (long?)defaultTime / poeCount.EmployeesCount,
                                 Time = (long?)time,
                                 DateTime = po.DateTime,
                                 Qty = qty.Value
                               };
      if (employeeId != null)
        operationTimeQuery = operationTimeQuery.Where(i => i.EmployeeId == employeeId);
      var defaultTimeQuery = (from interval in employeeIntervalQuery
                              from operation in operationTimeQuery
                              where operation.DateTime >= interval.EmployeeEnterTime && operation.DateTime <= interval.EmployeeExitTime && interval.EmployeeCode == operation.EmployeeCode
                              group operation by new { interval.WorkId, interval.EmployeeCode } into g
                              select new
                              {
                                WorkId = g.Key.WorkId,
                                EmployeeCode = g.Key.EmployeeCode,
                                TotalQty = g.Sum(i => i.Qty),
                                TotalTime = g.Sum(i => i.Time),
                                TotalDefaultTime = g.Sum(i => i.Qty * (i.DefaultTime ?? 0)) /*in second*/ / 60.0,
                              });
      var query = (from interval in employeeIntervalQuery
                   join employee in employeesQuery on interval.EmployeeId equals employee.Id
                   join time in defaultTimeQuery on interval.EmployeeCode equals time.EmployeeCode into timeLeft
                   from time in timeLeft.DefaultIfEmpty()
                   select new ProductionEmployeeEfficiencyResult
                   {
                     WorkId = interval.WorkId,
                     EmployeeCode = interval.EmployeeCode,
                     EmployeeFullName = employee.FirstName + " " + employee.LastName,
                     EmployeeEnterTime = interval.EmployeeEnterTime,
                     EmployeeExitTime = interval.EmployeeExitTime,
                     FirstRegisteredSerial = interval.FirstSerialTime,
                     LastRegisteredSerial = interval.LastSerialTime,
                     TotalPresenceTimeInCompany = interval.WorkTime / 60.0,
                     TotalPresenceTimeInProductionLine = interval.WorkTimeInLine / 60.0,
                     TotalOperationTime = time.TotalTime,
                     TotalEfficiency = (interval.WorkTime == 0 ? 0 : time.TotalDefaultTime / interval.WorkTime) * 100,
                     EffectiveEfficiency = (interval.WorkTimeInLine == 0 ? 0 : time.TotalDefaultTime / interval.WorkTimeInLine) * 100
                   }
            );
      return query;
    }
    #endregion
    #region GetProductionLinesEfficiency
    public IQueryable<ProductionLineEfficiencyReportsResult> GetProductionLinesEfficiency(
        int[] productionLineIds,
        DateTime fromDateTime,
        DateTime toDateTime,
        int normalBoardTime)
    {
      var ProductionLineEfficiencyReportsList = new List<ProductionLineEfficiencyReportsResult>();
      var productionLines = App.Internals.Planning.GetProductionLines();
      var productionSteps = App.Internals.Planning.GetProductionLineProductionSteps();
      for (int i = 0; i < productionLineIds.Length; i++)
      {
        var productionLineEfficiencyReportsResult = GetProductionLineEfficiency(
                  stuffId: null,
                  productionLineId: productionLineIds[i],
                  fromDateTime: fromDateTime,
                  toDateTime: toDateTime);
        ProductionLineEfficiencyReportsList.AddRange(productionLineEfficiencyReportsResult.ToList());
      }
      var query = (from item in ProductionLineEfficiencyReportsList
                   join productionLine in productionLines on item.ProductionLineId equals productionLine.Id
                   join productionStep in productionSteps on item.ProductionLineId equals productionStep.ProductionLineId
                   select new
                   {
                     ProductionLineId = item.ProductionLineId,
                     productionStepId = productionStep.ProductionStepId,
                     productionStepName = productionStep.ProductionStep.Name,
                     ProductionLineProductivityImpactFactor = productionLine.ProductivityImpactFactor,
                     productionStepProductivityImpactFactor = productionStep.ProductionStep.ProductivityImpactFactor,
                     ProductionLineName = productionLine.Name,
                     TotalProductionOperationTime = item.TotalProductionOperationTime,
                     TotalOperationsSequenceTime = item.TotalOperationsSequenceTime,
                     TotalOperation = item.TotalOperation,
                     //زمان حضور
                     AttendanceTime = item.AttendanceTime,
                     // بهره وری با زمان برد نرمال
                     EfficiencyWithNormalBoardTime = ((item.AttendanceTime == null
                             ? 0
                             : item.AttendanceTime / normalBoardTime)) == 0
                             ? 0
                             :
                            ((item.TotalProductionOperationTime / normalBoardTime) / ((item.AttendanceTime == null
                            ? 0
                            : item.AttendanceTime / normalBoardTime))) * 100
                   }).AsQueryable();
      var sumEfficiencyWithNormalBoardTime = 0d;
      var sumProductionStepProductivityImpactFactor = 0d;
      var productionStepGroup = from item in query
                                group item by item.productionStepId into g
                                select new
                                {
                                  // میانگین بهره وری یک خط تولید بر اساس ضریب
                                  AvgproductionStepEfficiency = g.Sum(i => i.EfficiencyWithNormalBoardTime * i.ProductionLineProductivityImpactFactor)
                                          / g.Sum(i => i.ProductionLineProductivityImpactFactor),
                                  productionStepId = g.Key,
                                  productionStepProductivityImpactFactor = g.FirstOrDefault().productionStepProductivityImpactFactor
                                };
      if (ProductionLineEfficiencyReportsList.Any())
      {
        // مجموع بهره وری خطوط یک مرحله تولید براساس ضریب
        sumEfficiencyWithNormalBoardTime = productionStepGroup.Sum(i => i.AvgproductionStepEfficiency * i.productionStepProductivityImpactFactor);
        // مجموع ضرایب
        sumProductionStepProductivityImpactFactor = productionStepGroup.Sum(i => i.productionStepProductivityImpactFactor);
      }
      return from item in query
             select new ProductionLineEfficiencyReportsResult()
             {
               ProductionLineId = item.ProductionLineId,
               ProductionLineProductivityImpactFactor = item.ProductionLineProductivityImpactFactor,
               productionStepProductivityImpactFactor = item.productionStepProductivityImpactFactor,
               ProductionLineName = item.ProductionLineName,
               productionStepName = item.productionStepName,
               TotalProductionOperationTime = item.TotalProductionOperationTime,
               TotalOperationsSequenceTime = item.TotalOperationsSequenceTime,
               TotalOperation = item.TotalOperation,
               //زمان حضور
               AttendanceTime = item.AttendanceTime,
               // بهره وری با زمان برد نرمال
               EfficiencyWithNormalBoardTime = item.EfficiencyWithNormalBoardTime,
               // میانگین بهره وری بر اساس ضرایب خط
               AvgEfficiencyWithNormalBoardTime = sumEfficiencyWithNormalBoardTime / sumProductionStepProductivityImpactFactor,
             };
    }
    #endregion
    #region GetProductionPercentageIndex
    public IQueryable<ProductionPercentageIndexResult> GetProductionPercentageIndex(
        int[] productionLineIds,
        DateTime fromDateTime,
        DateTime toDateTime)
    {
      var saleManagement = App.Internals.SaleManagement;
      var planning = App.Internals.Planning;
      var productionSchedules = planning.GetProductionSchedules(
               selector: e => e,
               fromDateTime: fromDateTime,
               toDateTime: toDateTime,
               productionLineIds: productionLineIds,
               isDelete: false);
      var productionScheduleResults = productionSchedules
            .Where(i => i.CalendarEvent.Duration < EF.Functions.DateDiffSecond(i.CalendarEvent.DateTime, toDateTime));
      var normalBoardTime = App.Internals.ApplicationSetting.GetNormalBoardTime();
      var operationSequences = planning.GetOperationSequences(e => new
      {
        e.WorkPlanStepId,
        e.DefaultTime
      });
      var operationSequenceGroups = from productionSchedule in productionScheduleResults
                                    join operationSequence in operationSequences on productionSchedule.WorkPlanStepId equals operationSequence.WorkPlanStepId
                                    group new { operationSequence } by productionSchedule.Id into g
                                    select new
                                    {
                                      ProductionScheduleId = g.Key,
                                      OperationSequenceTime = g.Sum(i => i.operationSequence.DefaultTime)
                                    };
      var ProductionScheduleGroups = from productionSchedule in productionScheduleResults
                                     join productionScheduleOrder in productionScheduleResults.SelectMany(i => i.ProductionOrders) on productionSchedule.Id equals productionScheduleOrder.ProductionScheduleId
                                     group new { productionScheduleOrder } by productionSchedule.Id into g
                                     select new
                                     {
                                       ProductionScheduleId = g.Key,
                                       Qty = g.Sum(i => i.productionScheduleOrder.Qty),
                                       InProductionQty = g.Sum(i => i.productionScheduleOrder.ProductionOrderSummary.InProductionQty),
                                       ProducedQty = g.Sum(i => i.productionScheduleOrder.ProductionOrderSummary.ProducedQty),
                                     };
      var query = from ProductionScheduleGroup in ProductionScheduleGroups
                  join operationSequence in operationSequenceGroups on ProductionScheduleGroup.ProductionScheduleId equals operationSequence.ProductionScheduleId
                  join productionScheduleResult in productionScheduleResults on ProductionScheduleGroup.ProductionScheduleId equals productionScheduleResult.Id
                  select new
                  {
                    FromDateTime = productionScheduleResult.CalendarEvent.DateTime,
                    ToDateTime = productionScheduleResult.CalendarEvent.DateTime.AddSeconds(productionScheduleResult.CalendarEvent.Duration),
                    SemiProductStuffCode = productionScheduleResult.ProductionPlanDetail.BillOfMaterial.Stuff.Code,
                    SemiProductStuffName = productionScheduleResult.ProductionPlanDetail.BillOfMaterial.Stuff.Name,
                    ProductionLineId = productionScheduleResult.WorkPlanStep.ProductionLineId,
                    ProductionLineName = productionScheduleResult.WorkPlanStep.ProductionLine.Name,
                    HardnessCoefficient = (double)operationSequence.OperationSequenceTime / normalBoardTime,
                    PlanQty = ProductionScheduleGroup.Qty,
                    InProductionQty = ProductionScheduleGroup.InProductionQty,
                    ProducedQty = ProductionScheduleGroup.ProducedQty,
                    PlanQtyNormalize = ProductionScheduleGroup.Qty * ((double)operationSequence.OperationSequenceTime / normalBoardTime),
                    InProductionQtyNormalize = ProductionScheduleGroup.InProductionQty * ((double)operationSequence.OperationSequenceTime / normalBoardTime),
                    ProducedQtyNormalize = ProductionScheduleGroup.ProducedQty * ((double)operationSequence.OperationSequenceTime / normalBoardTime),
                    PercentageProduction = ((ProductionScheduleGroup.ProducedQty * ((double)operationSequence.OperationSequenceTime / normalBoardTime))
                            / (ProductionScheduleGroup.Qty * ((double)operationSequence.OperationSequenceTime / normalBoardTime)))
                            * 100
                  };
      var avgPercentageProduction = 0d;
      if (query.Any())
      {
        avgPercentageProduction = query.Sum(i => i.PercentageProduction) / query.Count();
      }
      var result = from item in query
                   select new ProductionPercentageIndexResult()
                   {
                     FromDateTime = item.FromDateTime,
                     ToDateTime = item.ToDateTime,
                     SemiProductStuffCode = item.SemiProductStuffCode,
                     SemiProductStuffName = item.SemiProductStuffName,
                     ProductionLineId = item.ProductionLineId,
                     ProductionLineName = item.ProductionLineName,
                     HardnessCoefficient = item.HardnessCoefficient,
                     PlanQty = item.PlanQty,
                     InProductionQty = item.InProductionQty,
                     ProducedQty = item.ProducedQty,
                     PlanQtyNormalize = item.PlanQtyNormalize,
                     InProductionQtyNormalize = item.InProductionQtyNormalize,
                     ProducedQtyNormalize = item.ProducedQtyNormalize,
                     PercentageProduction = item.PercentageProduction,
                     AvgPercentageProduction = avgPercentageProduction
                   };
      return result;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProductionStatisticsReportResult> SortProductionStatisticsReportResult(
        IQueryable<ProductionStatisticsReportResult> query,
        SortInput<ProductionStatisticsReportSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionStatisticsReportSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ProductionStatisticsReportSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ProductionStatisticsReportSortType.StuffNoun:
          return query.OrderBy(a => a.StuffNoun, sort.SortOrder);
        case ProductionStatisticsReportSortType.OperationCode:
          return query.OrderBy(a => a.OperationCode, sort.SortOrder);
        case ProductionStatisticsReportSortType.OperationTitle:
          return query.OrderBy(a => a.OperationTitle, sort.SortOrder);
        case ProductionStatisticsReportSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case ProductionStatisticsReportSortType.OperationCount:
          return query.OrderBy(a => a.OperationCount, sort.SortOrder);
        case ProductionStatisticsReportSortType.ProductionLineName:
          return query.OrderBy(a => a.ProductionLineName, sort.SortOrder);
        case ProductionStatisticsReportSortType.ProductionTerminalDescription:
          return query.OrderBy(a => a.ProductionTerminalDescription, sort.SortOrder);
        case ProductionStatisticsReportSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<ProductionPercentageIndexResult> SortProductionPercentageIndex(
      IQueryable<ProductionPercentageIndexResult> query,
      SortInput<ProductionPercentageIndexSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionPercentageIndexSortType.SemiProductStuffCode: return query.OrderBy(a => a.SemiProductStuffCode, sort.SortOrder);
        case ProductionPercentageIndexSortType.SemiProductStuffName: return query.OrderBy(a => a.SemiProductStuffName, sort.SortOrder);
        case ProductionPercentageIndexSortType.PercentageProduction: return query.OrderBy(a => a.PercentageProduction, sort.SortOrder);
        case ProductionPercentageIndexSortType.ProductionLineName: return query.OrderBy(a => a.ProductionLineName, sort.SortOrder);
        case ProductionPercentageIndexSortType.FromDateTime: return query.OrderBy(a => a.FromDateTime, sort.SortOrder);
        case ProductionPercentageIndexSortType.ToDateTime: return query.OrderBy(a => a.ToDateTime, sort.SortOrder);
        case ProductionPercentageIndexSortType.HardnessCoefficient: return query.OrderBy(a => a.HardnessCoefficient, sort.SortOrder);
        case ProductionPercentageIndexSortType.PlanQty: return query.OrderBy(a => a.PlanQty, sort.SortOrder);
        case ProductionPercentageIndexSortType.InProductionQty: return query.OrderBy(a => a.InProductionQty, sort.SortOrder);
        case ProductionPercentageIndexSortType.ProducedQty: return query.OrderBy(a => a.ProducedQty, sort.SortOrder);
        case ProductionPercentageIndexSortType.PlanQtyNormalize: return query.OrderBy(a => a.PlanQtyNormalize, sort.SortOrder);
        case ProductionPercentageIndexSortType.InProductionQtyNormalize: return query.OrderBy(a => a.InProductionQtyNormalize, sort.SortOrder);
        case ProductionPercentageIndexSortType.ProducedQtyNormalize: return query.OrderBy(a => a.ProducedQtyNormalize, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<ProductionEmployeeEfficiencyResult> SortProductionEmployeeEfficiencyResult(
        IQueryable<ProductionEmployeeEfficiencyResult> query,
        SortInput<ProductionEmployeeEfficiencySortType> sort)
    {
      switch (sort.SortType)
      {
        //case ProductionEmployeeEfficiencySortType.DateTime:
        //    return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.EmployeeCode:
          return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.ProductionLineName:
          return query.OrderBy(a => a.ProductionLineName, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.EmployeeEnterTime:
          return query.OrderBy(a => a.EmployeeEnterTime, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.EmployeeExitTime:
          return query.OrderBy(a => a.EmployeeExitTime, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.FirstRegisteredSerial:
          return query.OrderBy(a => a.FirstRegisteredSerial, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.LastRegisteredSerial:
          return query.OrderBy(a => a.LastRegisteredSerial, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.TotalPresenceTimeInCompany:
          return query.OrderBy(a => a.TotalPresenceTimeInCompany, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.TotalPresenceTimeInProductionLine:
          return query.OrderBy(a => a.TotalPresenceTimeInProductionLine, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.TotalOperationTime:
          return query.OrderBy(a => a.TotalOperationTime, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.TotalEfficiency:
          return query.OrderBy(a => a.TotalEfficiency, sort.SortOrder);
        case ProductionEmployeeEfficiencySortType.EffectiveEfficiency:
          return query.OrderBy(a => a.EffectiveEfficiency, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException($"Sort argument of {nameof(ProductionEmployeeEfficiencyResult)} not suppurted.");
      }
    }
    #endregion
    #region Search
    public IQueryable<ProductionStatisticsReportResult> SearchProductionStatisticsReportResult(
        IQueryable<ProductionStatisticsReportResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
      )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.StuffName.Contains(searchText) ||
                 item.StuffNoun.Contains(searchText) ||
                 item.StuffCode.Contains(searchText) ||
                 item.OperationCode.Contains(searchText) ||
                 item.OperationTitle.Contains(searchText) ||
                 item.ProductionLineName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IQueryable<ProductionPercentageIndexResult> SearchProductionPercentageIndex(
       IQueryable<ProductionPercentageIndexResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems
     )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.SemiProductStuffCode.ToString().Contains(searchText) ||
                 item.SemiProductStuffName.ToString().Contains(searchText) ||
                 item.PercentageProduction.ToString().Contains(searchText) ||
                 item.ProductionLineName.ToString().Contains(searchText) ||
                 item.FromDateTime.ToString().Contains(searchText) ||
                 item.ToDateTime.ToString().Contains(searchText) ||
                 item.HardnessCoefficient.ToString().Contains(searchText) ||
                 item.PlanQty.ToString().Contains(searchText) ||
                 item.InProductionQty.ToString().Contains(searchText) ||
                 item.ProducedQty.ToString().Contains(searchText) ||
                 item.PlanQtyNormalize.ToString().Contains(searchText) ||
                 item.InProductionQtyNormalize.ToString().Contains(searchText) ||
                 item.ProducedQtyNormalize.ToString().Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IQueryable<ProductionEmployeeEfficiencyResult> SearchProductionEmployeeEfficiencyResult(
        IQueryable<ProductionEmployeeEfficiencyResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
      )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.EmployeeFullName.Contains(searchText) ||
                 item.EmployeeCode.Contains(searchText) ||
                 item.ProductionLineName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
  }
}