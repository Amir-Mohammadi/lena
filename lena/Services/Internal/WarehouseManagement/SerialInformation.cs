using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.WarehouseManagement.SerialDetail;
using lena.Models.WarehouseManagement.SerialTracking;
using lena.Services.CryptoMessaging.Crypto;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region GetSerialDetailLinkedSerials
    public IQueryable<TResult> GetSerialDetailLinkedSerials<TResult>(
        Expression<Func<ProductionStuffDetail, TResult>> selector,
        TValue<string> productionSerial,
        TValue<int[]> stuffIds)
    {

      var query = App.Internals.Production.GetProductionStuffDetails(
                    productionSerial: productionSerial,
                    stuffIds: stuffIds,
                    hasLinkedSerial: true);
      return query.Select(selector);
    }
    #endregion

    #region GetSerialDetails
    public IQueryable<TResult> GetSerialDetails<TResult>(
        Expression<Func<ProductionStuffDetail, TResult>> selector,
        TValue<string> productionSerial,
        TValue<int[]> stuffIds)
    {

      var query = App.Internals.Production.GetProductionStuffDetails(
                    productionSerial: productionSerial,
                    stuffIds: stuffIds);
      return query.Select(selector);
    }
    #endregion

    #region GetSerialDetailStuffs
    public IQueryable<ProductionStuffDetail> GetSerialDetailStuffs(
        TValue<string> productionSerial)
    {

      var query = App.Internals.Production.GetProductionStuffDetails(
                    productionSerial: productionSerial,
                    excludeSerial: productionSerial);

      return query;
    }
    #endregion

    #region GetSerialTrackingConsumptions
    public IQueryable<TResult> GetSerialTrackingConsumptions<TResult>(
        Expression<Func<ProductionStuffDetail, TResult>> selector,
        TValue<string> serial = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<ProductionStuffDetailType> productionStuffDetailType = null)
    {

      var query = App.Internals.Production.GetProductionStuffDetails(
                    serial: serial,
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    productionStuffDetailType: productionStuffDetailType
                );
      return query.Select(selector);
    }
    #endregion

    #region GetSerialOperationsTracking
    public IQueryable<SerialOperationsTrackingResult> GetSerialOperationsTrackings(
        TValue<string> serial)
    {

      var productionOperations = App.Internals.Production.GetProductionOperations(
                    selector: e => new
                    {
                      e.OperationId,
                      ProductionTerminalDescription = e.ProductionTerminal.Description,
                      e.DateTime,
                      Time = e.Time,
                      e.Status,
                      OperationCode = e.Operation.Code,
                      OperationTitle = e.Operation.Title,
                      ProductionOrderCode = e.ProductionOperator.ProductionOrder.Code,
                      Qty = e.Qty,
                      ProductionOperationEmployeeGroupId = (int?)e.ProductionOperationEmployeeGroupId
                    },
                    serial: serial
                )


                .ToList();

      var operationSequences = App.Internals.Planning.GetOperationSequences(
                    selector: e => new
                    {
                      e.OperationId,
                      e.Index,
                      OperationCode = e.Operation.Code,
                      OperationTitle = e.Operation.Title,
                    },
                    serial: serial)


            .ToList();

      var serialOperationsTrackings = (from operationSequence in operationSequences
                                       join productionOperation in productionOperations on operationSequence.OperationId equals
                                                 productionOperation.OperationId into operations
                                       from operation in operations.DefaultIfEmpty()

                                       select new SerialOperationsTrackingResult
                                       {
                                         Order = operationSequence.Index,
                                         OperationCode = operationSequence.OperationCode,
                                         OperationTitle = operationSequence.OperationTitle,
                                         TerminalDescription = operation?.ProductionTerminalDescription,
                                         DateTime = operation?.DateTime,
                                         OperationTime = operation?.Time,
                                         Qty = operation?.Qty,
                                         IsDone = operation != null,
                                         ProductionOperationEmployeeGroupId = operation?.ProductionOperationEmployeeGroupId,
                                         IsFaild = operation != null && (operation?.Status & ProductionOperationStatus.Faild) > 0,
                                         ProductionOrderCode = operation?.ProductionOrderCode

                                       }).ToList();

      var productionOperationsNotInSequence = productionOperations.Where(x => operationSequences.All(s => s.OperationId != x.OperationId))
            .Select(i => new SerialOperationsTrackingResult()
            {
              Order = 0,
              OperationCode = i.OperationCode,
              OperationTitle = i.OperationTitle,
              TerminalDescription = i.ProductionTerminalDescription,
              ProductionOperationEmployeeGroupId = i.ProductionOperationEmployeeGroupId,
              DateTime = i.DateTime,
              OperationTime = i.Time,
              Qty = i.Qty,
              IsDone = i != null,
              ProductionOrderCode = i.ProductionOrderCode,
              IsFaild = i != null && (i.Status & ProductionOperationStatus.Faild) > 0
            }).ToList();

      serialOperationsTrackings = serialOperationsTrackings.Union(productionOperationsNotInSequence).ToList();

      var productionOperationEmployeeGroupIds = serialOperationsTrackings.Select(i => i.ProductionOperationEmployeeGroupId).ToArray();
      var productionOperationEmployees = repository.GetQuery<ProductionOperationEmployee>()
            .Where(i => productionOperationEmployeeGroupIds.Contains(i.ProductionOperationEmployeeGroupId))
            .Select(e =>
                new
                {
                  ProductionOperationEmployeeGroupId = e.ProductionOperationEmployeeGroupId,
                  EmployeeFullName = e.Employee.LastName + " " + e.Employee.FirstName + "(" + e.Employee.Code + ")"
                })
            .ToList();
      foreach (var item in serialOperationsTrackings)
      {
        item.Employees = productionOperationEmployees.Where(i => i.ProductionOperationEmployeeGroupId == item.ProductionOperationEmployeeGroupId)
              .Select(i => i.EmployeeFullName)
              .ToList();

      }
      return serialOperationsTrackings.AsQueryable();
    }
    #endregion

    #region SerialAllDependencies

    public IQueryable<TResult> GetSerialAllDependencies<TResult>(
        Expression<Func<ProductionStuffDetail, TResult>> selector,
        TValue<string> serial)
    {

      var getSerialDetails = App.Internals.Production.GetProductionStuffDetails(
                    productionSerial: serial
                );


      var getSerialTrackingConsumptions = App.Internals.Production.GetProductionStuffDetails(
                    serial: serial
                );

      var result = getSerialDetails.Union(getSerialTrackingConsumptions);



      return result.Select(selector);
    }

    #endregion


    #region Search
    public IQueryable<SerialDetailResult> SearchSerialDetail(
        IQueryable<SerialDetailResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.StuffCode.Contains(search) ||
                    item.StuffName.Contains(search) ||
                    item.Serial.Contains(search) ||
                    item.UnitName.Contains(search) ||
                    item.Qty.ToString().Contains(search)
                select item;

      return query;
    }

    public IQueryable<SerialDetailStuffResult> SearchSerialDetailStuffs(
       IQueryable<SerialDetailStuffResult> query,
       string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.StuffCode.Contains(search) ||
                    item.StuffName.Contains(search) ||
                    item.StuffNoun.Contains(search)
                select item;

      return query;
    }

    public IQueryable<SerialTrackingConsumptionResult> SearchSerialTrackingConsumption(
        IQueryable<SerialTrackingConsumptionResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.StuffCode.Contains(search) ||
                    item.StuffName.Contains(search) ||
                    item.Serial.Contains(search) ||
                    item.UnitName.Contains(search) ||
                    item.InitUnitName.Contains(search)
                select item;

      return query;
    }

    public IQueryable<SerialOperationsTrackingResult> SearchSerialOperationsTracking(
        IQueryable<SerialOperationsTrackingResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.OperationCode.Contains(search) ||
                    item.OperationTitle.Contains(search) ||
                    item.OperationTime.ToString().Contains(search)
                select item;

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<SerialDetailResult> SortSerialDetail(
        IQueryable<SerialDetailResult> query,
        SortInput<SerialDetailSortType> sort)
    {
      switch (sort.SortType)
      {
        case SerialDetailSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);
        case SerialDetailSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case SerialDetailSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case SerialDetailSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case SerialDetailSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case SerialDetailSortType.UnitId:
          return query.OrderBy(a => a.UnitId, sort.SortOrder);
        case SerialDetailSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IOrderedQueryable<SerialDetailStuffResult> SortSerialDetailStuffs(
       IQueryable<SerialDetailStuffResult> query,
       SortInput<SerialDetailStuffsSortType> sort)
    {
      switch (sort.SortType)
      {
        case SerialDetailStuffsSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);
        case SerialDetailStuffsSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case SerialDetailStuffsSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case SerialDetailStuffsSortType.StuffNoun:
          return query.OrderBy(a => a.StuffNoun, sort.SortOrder);
        case SerialDetailStuffsSortType.Count:
          return query.OrderBy(a => a.Count, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IOrderedQueryable<SerialTrackingConsumptionResult> SortSerialTrackingConsumption(
        IQueryable<SerialTrackingConsumptionResult> query,
        SortInput<SerialTrackingConsumptionSortType> sort)
    {
      switch (sort.SortType)
      {
        case SerialTrackingConsumptionSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);
        case SerialTrackingConsumptionSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case SerialTrackingConsumptionSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case SerialTrackingConsumptionSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case SerialTrackingConsumptionSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case SerialTrackingConsumptionSortType.UnitId:
          return query.OrderBy(a => a.UnitId, sort.SortOrder);
        case SerialTrackingConsumptionSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case SerialTrackingConsumptionSortType.InitQty:
          return query.OrderBy(a => a.InitQty, sort.SortOrder);
        case SerialTrackingConsumptionSortType.InitUnitId:
          return query.OrderBy(a => a.InitUnitId, sort.SortOrder);
        case SerialTrackingConsumptionSortType.InitUnitName:
          return query.OrderBy(a => a.InitUnitName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }


    public IOrderedQueryable<SerialOperationsTrackingResult> SortSerialOperationsTracking(
        IQueryable<SerialOperationsTrackingResult> query,
        SortInput<SerialOperationsTrackingSortType> sort)
    {
      switch (sort.SortType)
      {
        case SerialOperationsTrackingSortType.Order:
          return query.OrderBy(a => a.Order, sort.SortOrder);
        case SerialOperationsTrackingSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case SerialOperationsTrackingSortType.OperationCode:
          return query.OrderBy(a => a.OperationCode, sort.SortOrder);
        case SerialOperationsTrackingSortType.OperationTime:
          return query.OrderBy(a => a.OperationTime, sort.SortOrder);
        case SerialOperationsTrackingSortType.OperationTitle:
          return query.OrderBy(a => a.OperationTitle, sort.SortOrder);
        case SerialOperationsTrackingSortType.Employees:
          return query.OrderBy(a => a.Employees, sort.SortOrder);
        case SerialOperationsTrackingSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case SerialOperationsTrackingSortType.TerminalDescription:
          return query.OrderBy(a => a.TerminalDescription, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion

    #region ToResult

    internal Expression<Func<ProductionStuffDetail, SerialDetailResult>> ToSerialDetailResult =
        entity => new SerialDetailResult
        {
          LinkedSerial = entity.StuffSerial.LinkSerial.LinkedSerial,
          TechnicalNumber = entity.StuffSerial.LinkSerial.IranKhodroSerial.CustomerStuff.TechnicalNumber,
          StuffId = entity.StuffId,
          StuffCode = entity.Stuff.Code,
          StuffName = entity.Stuff.Name,
          Serial = entity.StuffSerial.Serial,
          Qty = entity.Qty - entity.DetachedQty,
          UnitId = entity.UnitId,
          UnitName = entity.Unit.Name,
          WorkPlanStepId = entity.Production.ProductionOrder.WorkPlanStepId,
          Version = entity.Production.ProductionOrder.WorkPlanStep.WorkPlan.Version,
          Status = entity.StuffSerial.Status,
          RowVersion = entity.RowVersion
        };

    internal IQueryable<SerialDetailStuffResult> ToSerialDetailStuffResult(
        IQueryable<ProductionStuffDetail> query)
    {

      var result = from item in query
                   group item by new
                   {
                     StuffId = item.StuffId,
                     StuffCode = item.Stuff.Code,
                     StuffName = item.Stuff.Name,
                     StuffNoun = item.Stuff.Noun
                   }
                    into grp
                   select new SerialDetailStuffResult
                   {
                     StuffId = grp.Key.StuffId,
                     StuffCode = grp.Key.StuffCode,
                     StuffName = grp.Key.StuffName,
                     StuffNoun = grp.Key.StuffNoun,
                     Count = grp.Count()
                   };

      return result;
    }

    internal Expression<Func<ProductionStuffDetail, SerialTrackingConsumptionResult>> ToSerialTrackingConsumptionResult =
        entity => new SerialTrackingConsumptionResult
        {
          LinkedSerial = entity.Production.StuffSerial.LinkSerial.LinkedSerial,
          StuffId = entity.Production.StuffSerial.StuffId,
          StuffCode = entity.Production.StuffSerial.Stuff.Code,
          StuffName = entity.Production.StuffSerial.Stuff.Name,
          Serial = entity.Production.StuffSerial.Serial,
          Qty = entity.Qty - entity.DetachedQty,
          UnitId = entity.UnitId,
          UnitName = entity.Unit.Name,
          InitQty = entity.Production.StuffSerial.InitQty,
          PartitionedQty = entity.Production.StuffSerial.PartitionedQty,
          InitUnitId = entity.Production.StuffSerial.InitUnitId,
          InitUnitName = entity.Production.StuffSerial.InitUnit.Name,
          RowVersion = entity.RowVersion
        };




    #endregion

  }
}
