using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Infrastructure;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.StuffFraction;
using lena.Models.WarehouseManagement.StuffFractionByProject;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    internal IQueryable<StuffFractionResult> GetStuffFractions(
        int? productStuffId,
        int? productStuffVersion,
        int? stuffId,
        DateTime dateTime,
        DateTime fromDateTime,
        DateTime toDateTime,
        bool includeMainProductionPlan,
        bool includeTemporaryProductionPlan,
        bool? includeStuffFaultyPercentage,
        bool? includeSerialBuffer,
        int? plannerUserId,
        int[] warehouseIds
        )
    {

      var warehouse = App.Internals.WarehouseManagement;
      var utcDate = DateTime.Now.ToUniversalTime();
      if (dateTime < utcDate)
        dateTime = utcDate;
      string warehouseIdsStr = null;
      if (warehouseIds != null && warehouseIds.Length != 0)
        warehouseIdsStr = string.Join(",", warehouseIds);
      #region Linq
      //#region transaction Query
      //var baseTransactions = warehouse.GetBaseTransactions(
      //        stuffId: stuffId,
      //        selector: warehouse.ToBaseTransactionMinResult)
      //    
      //;
      //var transactionPlans = warehouse.GetTransactionPlans(
      //        selector: e => new { e.Id, e.IsDelete },
      //        stuffId: stuffId)
      //    
      //;
      //var transactions = from baseTransaction in baseTransactions
      //                   from transactionPlan in transactionPlans.Where(i => baseTransaction.Id == i.Id).DefaultIfEmpty()
      //                   let isDelete = (bool?)transactionPlan.IsDelete
      //                   where isDelete != true && baseTransaction.TransactionLevel != TransactionLevel.Waste
      //                   select baseTransaction;
      //var importConsumPlanId = Models.StaticData.StaticTransactionTypes.ImportConsumPlan.Id;
      //var transactionQuery = from transaction in transactions
      //                           //فقط در تراکنش های از نوع برنامه مصرف  ضریب معیوبی کالا در مقدار مصرف اعمال  می شود
      //                       let faultyPercentage = transaction.TransactionTypeId == importConsumPlanId ? transaction.StuffFaultyPercentage : 0
      //                       select new
      //                       {
      //                           StuffId = transaction.StuffId,
      //                           EffectDateTime = transaction.EffectDateTime,
      //                           TransactionLevel = transaction.TransactionLevel,
      //                           TransactionTypeFactor = transaction.TransactionTypeFactor,
      //                           //فقط در تراکنش های از نوع برنامه مصرف  ضریب معیوبی کالا در مقدار مصرف اعمال  می شود
      //                           Value = Math.Floor(transaction.Value * (100 + faultyPercentage) / 100)
      //                       };


      //#endregion
      //#region DateGroupQuery

      //var dateGroupQuery = from transaction in transactionQuery
      //                     group transaction by new
      //                     {
      //                         StuffId = transaction.StuffId,
      //                         EffectDateTime = transaction.EffectDateTime,
      //                         TransactionLevel = transaction.TransactionLevel
      //                     }
      //    into gItems
      //                     select new
      //                     {
      //                         StuffId = gItems.Key.StuffId,
      //                         EffectDateTime = gItems.Key.EffectDateTime,
      //                         TransactionLevel = gItems.Key.TransactionLevel,
      //                         Value = gItems.Sum(i => i.Value)
      //                     };
      //#endregion
      //#region RunningTotalQuery
      ////var runningTotalQuery = from currentItem in dateGroupQuery
      ////                        join preItem in dateGroupQuery on
      ////                        new { currentItem.StuffId, TransactionLevel = currentItem.TransactionLevel } equals
      ////                        new { preItem.StuffId, TransactionLevel = preItem.TransactionLevel }
      ////                        where preItem.EffectDateTime <= currentItem.EffectDateTime
      ////                        group preItem by currentItem into gItems
      ////                        select new
      ////                        {
      ////                            StuffId = gItems.Key.StuffId,
      ////                            EffectDateTime = gItems.Key.EffectDateTime,
      ////                            TransactionLevel = gItems.Key.TransactionLevel,
      ////                            Value = gItems.Key.Value,
      ////                            Total = gItems.Sum(i => i.Value)
      ////                        };
      //#endregion
      //#region Inventory Query



      //var inventoryBaseQuery = from transaction in dateGroupQuery
      //                         group transaction by transaction.StuffId
      //into gItems
      //                         select new
      //                         {
      //                             StuffId = gItems.Key,
      //                             AvailableAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime < dateTime && t.TransactionLevel == TransactionLevel.Available)
      //                             .Sum(i => i.Value) ?? 0,
      //                             BlockedAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime < dateTime && t.TransactionLevel == TransactionLevel.Blocked)
      //                             .Sum(i => i.Value) ?? 0,
      //                             QualityControlAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime < dateTime &&
      //                            t.TransactionLevel == TransactionLevel.QualityControl)
      //                             .Sum(i => i.Value) ?? 0,
      //                             WasteAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime < dateTime && t.TransactionLevel == TransactionLevel.Waste)
      //                             .Sum(i => i.Value) ?? 0,
      //                             PlanAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime < dateTime && t.TransactionLevel == TransactionLevel.Plan)
      //                             .Sum(i => i.Value) ?? 0,
      //                             TotalAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime < dateTime && t.TransactionLevel != TransactionLevel.Waste)
      //                             .Sum(i => i.Value) ?? 0,
      //                             BeforePeriodPlanPlusAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime >= dateTime &&
      //                            t.EffectDateTime < fromDateTime &&
      //                            t.TransactionLevel == TransactionLevel.Plan &&
      //                            t.Value > 0)
      //                             //t.TransactionTypeFactor == TransactionTypeFactorType.Plus)
      //                             .Sum(i => i.Value) ?? 0,
      //                             BeforePeriodPlanMinusAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime >= dateTime &&
      //                            t.EffectDateTime < fromDateTime &&
      //                            t.TransactionLevel == TransactionLevel.Plan &&
      //                            t.Value < 0)
      //                             //t.TransactionTypeFactor == TransactionTypeFactorType.Minus)
      //                             .Sum(i => i.Value) ?? 0,
      //                             PeriodPlanPlusAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime >= fromDateTime &&
      //                            t.EffectDateTime < toDateTime &&
      //                            t.TransactionLevel == TransactionLevel.Plan &&
      //                            t.Value > 0)
      //                             //t.TransactionTypeFactor == TransactionTypeFactorType.Plus)
      //                             .Sum(i => i.Value) ?? 0,
      //                             PeriodPlanMinusAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime >= fromDateTime &&
      //                            t.EffectDateTime < toDateTime &&
      //                            t.TransactionLevel == TransactionLevel.Plan &&
      //                            t.Value < 0)
      //                             //t.TransactionTypeFactor == TransactionTypeFactorType.Minus)
      //                             .Sum(i => i.Value) ?? 0,
      //                             AfterPeriodPlanPlusAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime >= toDateTime &&
      //                            t.TransactionLevel == TransactionLevel.Plan &&
      //                            t.Value > 0)
      //                             //t.TransactionTypeFactor == TransactionTypeFactorType.Plus)
      //                             .Sum(i => i.Value) ?? 0,
      //                             AfterPeriodPlanMinusAmount = (double?)gItems
      //                             .Where(t => t.EffectDateTime >= toDateTime &&
      //                            t.TransactionLevel == TransactionLevel.Plan &&
      //                            t.Value < 0)
      //                             //t.TransactionTypeFactor == TransactionTypeFactorType.Minus)
      //                             .Sum(i => i.Value) ?? 0,
      //                             //todo fix it running total
      //                             HasFraction = false,//gItems.Any(t => t.EffectDateTime < toDateTime && t.Total < 0)
      //                         };
      //#endregion
      //#region Get Units
      //var units = App.Internals.ApplicationBase.GetUnits(
      //        selector: i => new { i.Id, i.Name, i.UnitTypeId },
      //        isMainUnit: true,
      //        isActive: true)
      //    
      //;
      //#endregion
      //#region GetStuffs
      //var stuffs = App.Internals.SaleManagement.GetStuffs(
      //        selector: e => e,
      //        id: stuffId)
      //    
      //;
      //#endregion
      //#region BaseQuery 

      //var baseQuery = from stuff in stuffs
      //                join unit in units on stuff.UnitTypeId equals unit.UnitTypeId
      //                select new
      //                {
      //                    StuffId = stuff.Id,
      //                    StuffName = stuff.Name,
      //                    StuffNoun = stuff.Noun,
      //                    StuffTitle = stuff.Title,
      //                    StuffCode = stuff.Code,
      //                    StuffFaultyPercentage = stuff.FaultyPercentage,
      //                    StuffStockSafety = stuff.StockSafety,
      //                    UnitId = unit.Id,
      //                    UnitName = unit.Name
      //                };
      //#endregion
      //#region InventoryQuery
      //var inventoryQuery = from inventory in inventoryBaseQuery
      //                     join stuffInfo in baseQuery on inventory.StuffId equals stuffInfo.StuffId
      //                     let remainedAmount = inventory.TotalAmount - stuffInfo.StuffStockSafety
      //                     let beforePeriodPlanAmount = remainedAmount +
      //                         inventory.BeforePeriodPlanPlusAmount +
      //                         inventory.BeforePeriodPlanMinusAmount
      //                     let periodPlanAmount = beforePeriodPlanAmount +
      //                         inventory.PeriodPlanPlusAmount +
      //                         inventory.PeriodPlanMinusAmount
      //                     let afterPeriodPlanAmount = periodPlanAmount +
      //                         inventory.AfterPeriodPlanPlusAmount +
      //                         inventory.AfterPeriodPlanMinusAmount

      //                     select new
      //                     {
      //                         StuffName = stuffInfo.StuffName,
      //                         StuffNoun = stuffInfo.StuffNoun,
      //                         StuffTitle = stuffInfo.StuffTitle,
      //                         StuffCode = stuffInfo.StuffCode,
      //                         StuffFaultyPercentage = stuffInfo.StuffFaultyPercentage,
      //                         StuffStockSafety = stuffInfo.StuffStockSafety,
      //                         UnitId = stuffInfo.UnitId,
      //                         UnitName = stuffInfo.UnitName,
      //                         StuffId = inventory.StuffId,
      //                         AvailableAmount = inventory.AvailableAmount,
      //                         BlockedAmount = inventory.BlockedAmount,
      //                         QualityControlAmount = inventory.QualityControlAmount,
      //                         WasteAmount = inventory.WasteAmount,
      //                         PlanAmount = inventory.PlanAmount,
      //                         TotalAmount = inventory.TotalAmount,
      //                         RemainedAmount = remainedAmount,
      //                         BeforePeriodPlanPlusAmount = inventory.BeforePeriodPlanPlusAmount,
      //                         BeforePeriodPlanMinusAmount = inventory.BeforePeriodPlanMinusAmount,
      //                         BeforePeriodPlanAmount = beforePeriodPlanAmount,
      //                         PeriodPlanPlusAmount = inventory.PeriodPlanPlusAmount,
      //                         PeriodPlanMinusAmount = inventory.PeriodPlanMinusAmount,
      //                         PeriodPlanAmount = periodPlanAmount,
      //                         AfterPeriodPlanPlusAmount = inventory.AfterPeriodPlanPlusAmount,
      //                         AfterPeriodPlanMinusAmount = inventory.AfterPeriodPlanMinusAmount,
      //                         AfterPeriodPlanAmount = afterPeriodPlanAmount,
      //                         HasFraction = inventory.HasFraction
      //                     };

      //#endregion
      //#region result 
      //var resultTransactions = from inventory in inventoryQuery
      //                         select new StuffFractionResult()
      //                         {
      //                             StuffId = inventory.StuffId,
      //                             StuffName = inventory.StuffName,
      //                             StuffNoun = inventory.StuffNoun,
      //                             StuffTitle = inventory.StuffTitle,
      //                             StuffCode = inventory.StuffCode,
      //                             StuffFaultyPercentage = inventory.StuffFaultyPercentage,
      //                             StuffStockSafety = inventory.StuffStockSafety,
      //                             UnitId = inventory.UnitId,
      //                             UnitName = inventory.UnitName,
      //                             AvailableAmount = inventory.AvailableAmount,
      //                             BlockedAmount = inventory.BlockedAmount,
      //                             QualityControlAmount = inventory.QualityControlAmount,
      //                             WasteAmount = inventory.WasteAmount,
      //                             PlanAmount = inventory.PlanAmount,
      //                             TotalAmount = inventory.TotalAmount,
      //                             RemainedAmount = inventory.RemainedAmount,
      //                             BeforePeriodPlanPlusAmount = inventory.BeforePeriodPlanPlusAmount,
      //                             BeforePeriodPlanMinusAmount = inventory.BeforePeriodPlanMinusAmount,
      //                             BeforePeriodPlanAmount = inventory.BeforePeriodPlanAmount,
      //                             PeriodPlanPlusAmount = inventory.PeriodPlanPlusAmount,
      //                             PeriodPlanMinusAmount = inventory.PeriodPlanMinusAmount,
      //                             PeriodPlanAmount = inventory.PeriodPlanAmount,
      //                             AfterPeriodPlanPlusAmount = inventory.AfterPeriodPlanPlusAmount,
      //                             AfterPeriodPlanMinusAmount = inventory.AfterPeriodPlanMinusAmount,
      //                             AfterPeriodPlanAmount = inventory.AfterPeriodPlanAmount,
      //                             Status = (inventory.PeriodPlanAmount < 0 || inventory.BeforePeriodPlanAmount < 0) ? InventoryPlanStatus.Critical :
      //                                    (inventory.RemainedAmount - inventory.PlanAmount + inventory.BeforePeriodPlanMinusAmount + inventory.PeriodPlanMinusAmount) >= 0 ? InventoryPlanStatus.Normal :
      //                                     (inventory.HasFraction || inventory.RemainedAmount < 0) ? InventoryPlanStatus.Dangerous :
      //                                     InventoryPlanStatus.Caution
      //                         };
      //var result = resultTransactions;
      //#endregion
      #endregion
      var parameters = new List<SqlParameter>();
      parameters.Add(new SqlParameter() { ParameterName = "@productStuffId", Value = (object)productStuffId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@productStuffVersion", Value = (object)productStuffVersion ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@stuffId", Value = (object)stuffId ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@dateTime", Value = dateTime });
      parameters.Add(new SqlParameter() { ParameterName = "@fromDateTime", Value = fromDateTime });
      parameters.Add(new SqlParameter() { ParameterName = "@toDateTime", Value = toDateTime });
      parameters.Add(new SqlParameter() { ParameterName = "@includeSerialBuffer", Value = includeSerialBuffer ?? true });
      parameters.Add(new SqlParameter() { ParameterName = "@includeMainProductionPlan", Value = includeMainProductionPlan });
      parameters.Add(new SqlParameter() { ParameterName = "@includeTemporaryProductionPlan", Value = includeTemporaryProductionPlan });
      parameters.Add(new SqlParameter() { ParameterName = "@includeStuffFaultyPercentage", Value = includeStuffFaultyPercentage });
      parameters.Add(new SqlParameter() { ParameterName = "@plannerUserId", Value = plannerUserId });
      parameters.Add(new SqlParameter() { ParameterName = "@warehouseIds", Value = warehouseIdsStr });


      var result = repository.CreateQuery<StuffFractionResult>("EXEC dbo.usp_GetStuffFractions @dateTime, @fromDateTime, @toDateTime, @stuffId, @productStuffId, @productStuffVersion, @includeSerialBuffer, @includeMainProductionPlan, @includeTemporaryProductionPlan, @includeStuffFaultyPercentage , @plannerUserId , @warehouseIds",
                parameters.ToArray<DbParameter>());
      return result;
    }


    internal IEnumerable<ExpandoObject> GetStuffFractionsByProject(
        StuffFractionStuffInput[] stuffs,
        int[] warehouseIds
        )
    {


      string warehouseIdsStr = null;

      if (warehouseIds != null && warehouseIds.Length != 0)
        warehouseIdsStr = string.Join(",", warehouseIds);

      StringBuilder strBuilder = new StringBuilder();
      var productFlag = 0;
      foreach (var item in stuffs)
      {
        if (productFlag != 0)
          strBuilder.Append("|");

        strBuilder.Append(item.StuffCode + ",");
        strBuilder.Append(item.ProjectCode + ",");
        strBuilder.Append(item.Qty);
        var semiFlag = 0;
        foreach (var semiItem in item.SemiProductsInfo)
        {
          if (semiItem.Qty >= 0)
          {
            if (semiFlag != 0)
              strBuilder.Append("/");
            else
              strBuilder.Append(",");
            strBuilder.Append(semiItem.StuffCode + ":");
            strBuilder.Append(item.Qty);

            semiFlag = 1;
          }
        }
        productFlag = 1;
      }

      var projectsFormattedStr = strBuilder.ToString();

      var parameters = new List<SqlParameter>();
      parameters.Add(new SqlParameter() { ParameterName = "@inventoryWarehouseIds", Value = (object)warehouseIdsStr ?? DBNull.Value });
      parameters.Add(new SqlParameter() { ParameterName = "@projects", Value = (object)projectsFormattedStr ?? DBNull.Value });



      var result = repository.CreateQueryWithDynamicResult("EXEC dbo.[usp_CalculateProjectBasedFractions] @inventoryWarehouseIds , @projects", parameters.ToArray<DbParameter>());

      return result.ToList();
    }

    public IQueryable<StuffFractionResult> SearchStuffFractionResult(
        IQueryable<StuffFractionResult> query,
        InventoryPlanStatus[] statuses)
    {
      if (statuses != null)
        query = query.Where(i => statuses.Contains(i.Status));
      return query;
    }

    #region Sort
    public IOrderedQueryable<StuffFractionResult> SortStuffFractionResult(IQueryable<StuffFractionResult> input, SortInput<StuffFractionSortType> options)
    {
      switch (options.SortType)
      {
        case StuffFractionSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case StuffFractionSortType.StuffNoun:
          return input.OrderBy(i => i.StuffNoun, options.SortOrder);
        case StuffFractionSortType.StuffTitle:
          return input.OrderBy(i => i.StuffTitle, options.SortOrder);
        case StuffFractionSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case StuffFractionSortType.StuffFaultyPercentage:
          return input.OrderBy(i => i.StuffFaultyPercentage, options.SortOrder);
        case StuffFractionSortType.UnitName:
          return input.OrderBy(i => i.UnitName, options.SortOrder);
        case StuffFractionSortType.StuffStockSafety:
          return input.OrderBy(i => i.StuffStockSafety, options.SortOrder);
        case StuffFractionSortType.AvailableAmount:
          return input.OrderBy(i => i.AvailableAmount, options.SortOrder);
        case StuffFractionSortType.BlockedAmount:
          return input.OrderBy(i => i.BlockedAmount, options.SortOrder);
        case StuffFractionSortType.PlanAmount:
          return input.OrderBy(i => i.PlanAmount, options.SortOrder);
        case StuffFractionSortType.PlanCargoAmount:
          return input.OrderBy(i => i.PlanCargoAmount, options.SortOrder);
        case StuffFractionSortType.PlanConsumeAmount:
          return input.OrderBy(i => i.PlanConsumeAmount, options.SortOrder);
        case StuffFractionSortType.PlanProduceAmount:
          return input.OrderBy(i => i.PlanProduceAmount, options.SortOrder);
        case StuffFractionSortType.PlanSaleOrderAmount:
          return input.OrderBy(i => i.PlanSaleOrderAmount, options.SortOrder);
        case StuffFractionSortType.PlanPurchaseOrderAmount:
          return input.OrderBy(i => i.PlanPurchaseOrderAmount, options.SortOrder);
        case StuffFractionSortType.PlanPurchaseRequestAmount:
          return input.OrderBy(i => i.PlanPurchaseRequestAmount, options.SortOrder);
        case StuffFractionSortType.QualityControlAmount:
          return input.OrderBy(i => i.QualityControlAmount, options.SortOrder);
        case StuffFractionSortType.WasteAmount:
          return input.OrderBy(i => i.WasteAmount, options.SortOrder);
        case StuffFractionSortType.TotalAmount:
          return input.OrderBy(i => i.TotalAmount, options.SortOrder);
        case StuffFractionSortType.RemainedAmount:
          return input.OrderBy(i => i.RemainedAmount, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanPlusAmount:
          return input.OrderBy(i => i.BeforePeriodPlanPlusAmount, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanMinusAmount:
          return input.OrderBy(i => i.BeforePeriodPlanMinusAmount, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanAmount:
          return input.OrderBy(i => i.BeforePeriodPlanAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanPlusAmount:
          return input.OrderBy(i => i.PeriodPlanPlusAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanMinusAmount:
          return input.OrderBy(i => i.PeriodPlanMinusAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanAmount:
          return input.OrderBy(i => i.PeriodPlanAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanPlusAmount:
          return input.OrderBy(i => i.AfterPeriodPlanPlusAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanMinusAmount:
          return input.OrderBy(i => i.AfterPeriodPlanMinusAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanAmount:
          return input.OrderBy(i => i.AfterPeriodPlanAmount, options.SortOrder);
        case StuffFractionSortType.BufferRemainingAmount:
          return input.OrderBy(i => i.BufferRemainingAmount, options.SortOrder);
        case StuffFractionSortType.Status:
          return input.OrderBy(i => i.Status, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanConsumeAmount:
          return input.OrderBy(i => i.BeforePeriodPlanConsumeAmount, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanProduceAmount:
          return input.OrderBy(i => i.BeforePeriodPlanProduceAmount, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanSaleOrderAmount:
          return input.OrderBy(i => i.BeforePeriodPlanSaleOrderAmount, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanPurchaseRequestAmount:
          return input.OrderBy(i => i.BeforePeriodPlanPurchaseRequestAmount, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanPurchaseOrderAmount:
          return input.OrderBy(i => i.BeforePeriodPlanPurchaseOrderAmount, options.SortOrder);
        case StuffFractionSortType.BeforePeriodPlanCargoAmount:
          return input.OrderBy(i => i.BeforePeriodPlanCargoAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanConsumeAmount:
          return input.OrderBy(i => i.PeriodPlanConsumeAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanProduceAmount:
          return input.OrderBy(i => i.PeriodPlanProduceAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanSaleOrderAmount:
          return input.OrderBy(i => i.PeriodPlanSaleOrderAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanPurchaseRequestAmount:
          return input.OrderBy(i => i.PeriodPlanPurchaseRequestAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanPurchaseOrderAmount:
          return input.OrderBy(i => i.PeriodPlanPurchaseOrderAmount, options.SortOrder);
        case StuffFractionSortType.PeriodPlanCargoAmount:
          return input.OrderBy(i => i.PeriodPlanCargoAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanConsumeAmount:
          return input.OrderBy(i => i.AfterPeriodPlanConsumeAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanProduceAmount:
          return input.OrderBy(i => i.AfterPeriodPlanProduceAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanSaleOrderAmount:
          return input.OrderBy(i => i.AfterPeriodPlanSaleOrderAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanPurchaseRequestAmount:
          return input.OrderBy(i => i.AfterPeriodPlanPurchaseRequestAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanPurchaseOrderAmount:
          return input.OrderBy(i => i.AfterPeriodPlanPurchaseOrderAmount, options.SortOrder);
        case StuffFractionSortType.AfterPeriodPlanCargoAmount:
          return input.OrderBy(i => i.AfterPeriodPlanCargoAmount, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}