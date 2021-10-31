using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
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
using lena.Models.Supplies.StuffFractionDetail;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    internal IQueryable<StuffFractionDetailResult> GetStuffFractionDetails(
        int? productStuffId,
        int? productStuffVersion,
        int? stuffId,
        DateTime? dateTime,
        DateTime? fromDateTime,
        DateTime? toDateTime)
    {

      var warehouse = App.Internals.WarehouseManagement;
      var utcDate = DateTime.Now.ToUniversalTime();
      #region transaction Query
      var baseTransactions = warehouse.GetBaseTransactions(
              stuffId: stuffId,
              selector: warehouse.ToBaseTransactionMinResult);
      var transactionPlans = warehouse.GetTransactionPlans(
                    selector: e => new { e.Id, e.IsDelete },
                    stuffId: stuffId);
      var transactions = from baseTransaction in baseTransactions
                         from transactionPlan in transactionPlans.Where(i => baseTransaction.Id == i.Id).DefaultIfEmpty()
                         let isDelete = (bool?)transactionPlan.IsDelete
                         where isDelete != true && baseTransaction.TransactionLevel != TransactionLevel.Waste
                         select baseTransaction;
      var importConsumPlanId = Models.StaticData.StaticTransactionTypes.ImportConsumPlan.Id;
      var transactionQuery = from transaction in transactions
                               //فقط در تراکنش های از نوع برنامه مصرف  ضریب معیوبی کالا در مقدار مصرف اعمال  می شود
                             let faultyPercentage = transaction.TransactionTypeId == importConsumPlanId ? transaction.StuffFaultyPercentage : 0
                             select new
                             {
                               TransactionBatchId = transaction.TransactionBatchId,
                               BaseEntityId = transaction.BaseEntityId,
                               StuffId = transaction.StuffId,
                               EffectDateTime = transaction.EffectDateTime,
                               TransactionLevel = transaction.TransactionLevel,
                               TransactionTypeFactor = transaction.TransactionTypeFactor,
                               //فقط در تراکنش های از نوع برنامه مصرف  ضریب معیوبی کالا در مقدار مصرف اعمال  می شود
                               Value = Math.Floor(transaction.Value * (100 + faultyPercentage) / 100)
                             };


      #endregion
      #region Group Query
      var groupQuery = from transaction in transactionQuery
                       join previousTransaction in transactionQuery on transaction.StuffId equals previousTransaction.StuffId
                       where previousTransaction.EffectDateTime <= transaction.EffectDateTime
                       group previousTransaction by new
                       {
                         StuffId = transaction.StuffId,
                         EffectDateTime = transaction.EffectDateTime,
                         TransactionBatchId = transaction.TransactionBatchId
                       }
          into gItems
                       let key = gItems.Key
                       select new
                       {
                         key.StuffId,
                         key.EffectDateTime,
                         key.TransactionBatchId,
                         TotalAmount = gItems.Sum(i => i.Value),
                         AvailableAmount = (double?)gItems.Where(i => i.TransactionLevel == TransactionLevel.Available).Sum(i => i.Value),
                         BlockedAmount = (double?)gItems.Where(i => i.TransactionLevel == TransactionLevel.Blocked).Sum(i => i.Value),
                         PlanAmount = (double?)gItems.Where(i => i.TransactionLevel == TransactionLevel.Plan).Sum(i => i.Value),
                         QualityControlAmount = (double?)gItems.Where(i => i.TransactionLevel == TransactionLevel.QualityControl).Sum(i => i.Value)
                       };
      #endregion
      #region ProductionSchedule Query

      var productionScheduleQuery = GetProductionSchedules(
              selector: e => e);
      #endregion

      #region ProductionPlanDetail Query
      var productionPlanDetailQuery = GetProductionPlanDetails(
              selector: e => e);
      #endregion

      #region BillOfMaterialDetailQuery
      var billOfMaterialDetails = GetBillOfMaterialDetails();
      #endregion


      #region Get ResultTransactions 
      var stuffs = repository.GetQuery<Stuff>();
      var resultTransactions = from item in groupQuery
                               join transaction in transactionQuery on
                                        new { item.StuffId, item.EffectDateTime } equals new { transaction.StuffId, transaction.EffectDateTime }
                               join stuff in stuffs on item.StuffId equals stuff.Id
                               from unit in stuff.UnitType.Units
                               where unit.IsMainUnit
                               join tps in productionScheduleQuery on transaction.TransactionBatchId equals tps.TransactionBatch.Id into tpss
                               from productionSchedule in tpss.DefaultIfEmpty()
                               join tpp in productionPlanDetailQuery on transaction.TransactionBatchId equals tpp.TransactionBatch.Id into tpps
                               from productionPlanDetail in tpps.DefaultIfEmpty()
                               let psId = (int?)productionSchedule.Id
                               let currentProductionPlanDetail = psId == null ? productionPlanDetail : productionSchedule.ProductionPlanDetail
                               join billOfMaterialDetail in billOfMaterialDetails on
                                     new
                                     {
                                       StuffId = item.StuffId,
                                       BillOfMaterialVersion = currentProductionPlanDetail.BillOfMaterialVersion,
                                       BillOfMaterialStuffId = currentProductionPlanDetail.BillOfMaterialStuffId
                                     }
                                     equals
                                     new
                                     {
                                       StuffId = billOfMaterialDetail.StuffId,
                                       BillOfMaterialVersion = billOfMaterialDetail.BillOfMaterialVersion,
                                       BillOfMaterialStuffId = billOfMaterialDetail.BillOfMaterialStuffId
                                     }

                               select new StuffFractionDetailResult()
                               {
                                 StuffId = stuff.Id,
                                 StuffName = stuff.Name,
                                 StuffNoun = stuff.Noun,
                                 StuffTitle = stuff.Title,
                                 StuffCode = stuff.Code,
                                 StuffFaultyPercentage = stuff.FaultyPercentage,
                                 StuffStockSafety = stuff.StockSafety,
                                 EffectDateTime = item.EffectDateTime,
                                 UnitId = unit.Id,
                                 UnitName = unit.Name,
                                 Value = transaction.Value,
                                 TotalAmount = item.TotalAmount,
                                 //اعمال ذخیره احتیاطی در مقدار کل  موجودی
                                 RemainedAmount = item.TotalAmount - stuff.StockSafety,
                                 AvailableAmount = item.AvailableAmount ?? 0,
                                 BlockedAmount = item.BlockedAmount ?? 0,
                                 PlanAmount = item.PlanAmount ?? 0,
                                 QualityControlAmount = item.QualityControlAmount ?? 0,
                                 ProductionScheduleId = productionSchedule.Id,
                                 ProductionScheduleCode = productionSchedule.Code,
                                 ProductionPlanDetailId = productionSchedule.ProductionPlanDetailId,
                                 ProductionPlanId = productionPlanDetail.ProductionPlanId,
                                 ProductionPlanCode = productionPlanDetail.ProductionPlan.Code,
                                 ProductionRequestId = productionPlanDetail.ProductionPlan.ProductionRequestId,
                                 ProductionRequestCode = productionPlanDetail.ProductionPlan.ProductionRequest.Code,
                                 OrderItemId = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Id,
                                 OrderItemCode = productionPlanDetail.ProductionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Code,
                                 BillOfMaterialDetailId = billOfMaterialDetail.Id
                               };
      #endregion
      var result = resultTransactions.Where(i => i.Value < 0 && i.RemainedAmount < 0 && i.EffectDateTime >= dateTime);
      if (fromDateTime != null)
        result = result.Where(i => i.EffectDateTime >= fromDateTime.Value);
      if (toDateTime != null)
        result = result.Where(i => i.EffectDateTime <= toDateTime.Value);
      return result;
    }
    public IQueryable<StuffFractionDetailResult> SearchStuffFractionDetailResult(
        IQueryable<StuffFractionDetailResult> query)
    {

      return query;
    }

    #region Sort
    public IOrderedQueryable<StuffFractionDetailResult> SortStuffFractionDetailResult(IQueryable<StuffFractionDetailResult> input, SortInput<StuffFractionDetailSortType> options)
    {
      switch (options.SortType)
      {
        case StuffFractionDetailSortType.StuffCode:
          return input.OrderBy(o => o.StuffCode, options.SortOrder);
        case StuffFractionDetailSortType.StuffName:
          return input.OrderBy(o => o.StuffName, options.SortOrder);
        case StuffFractionDetailSortType.StuffFaultyPercentage:
          return input.OrderBy(o => o.StuffFaultyPercentage, options.SortOrder);
        case StuffFractionDetailSortType.UnitName:
          return input.OrderBy(o => o.UnitName, options.SortOrder);
        case StuffFractionDetailSortType.Value:
          return input.OrderBy(o => o.Value, options.SortOrder);
        case StuffFractionDetailSortType.EffectDateTime:
          return input.OrderBy(o => o.EffectDateTime, options.SortOrder);
        case StuffFractionDetailSortType.AvailableAmount:
          return input.OrderBy(o => o.AvailableAmount, options.SortOrder);
        case StuffFractionDetailSortType.QualityControlAmount:
          return input.OrderBy(o => o.QualityControlAmount, options.SortOrder);
        case StuffFractionDetailSortType.BlockedAmount:
          return input.OrderBy(o => o.BlockedAmount, options.SortOrder);
        case StuffFractionDetailSortType.PlanAmount:
          return input.OrderBy(o => o.PlanAmount, options.SortOrder);
        case StuffFractionDetailSortType.TotalAmount:
          return input.OrderBy(o => o.TotalAmount, options.SortOrder);
        case StuffFractionDetailSortType.StuffStockSafety:
          return input.OrderBy(o => o.StuffStockSafety, options.SortOrder);
        case StuffFractionDetailSortType.RemainedAmount:
          return input.OrderBy(o => o.RemainedAmount, options.SortOrder);
        case StuffFractionDetailSortType.OrderItemCode:
          return input.OrderBy(o => o.OrderItemCode, options.SortOrder);
        case StuffFractionDetailSortType.ProductionRequestCode:
          return input.OrderBy(o => o.ProductionRequestCode, options.SortOrder);
        case StuffFractionDetailSortType.ProductionPlanCode:
          return input.OrderBy(o => o.ProductionPlanCode, options.SortOrder);
        case StuffFractionDetailSortType.ProductionScheduleCode:
          return input.OrderBy(o => o.ProductionScheduleCode, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
      #endregion
    }
  }
}