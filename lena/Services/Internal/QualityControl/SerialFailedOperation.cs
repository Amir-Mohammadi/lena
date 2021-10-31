using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Production.FaildProductionOperation;
using lena.Models.QualityControl.SerialFailedOperation;
using lena.Models.WarehouseManagement.WarehouseTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add
    public SerialFailedOperation AddSerialFailedOperation(
        int productionOperationId,
        int productionOrderId)
    {

      var serialFailedOperation = repository.Create<SerialFailedOperation>();
      serialFailedOperation.ProductionOrderId = productionOrderId;
      serialFailedOperation.ProductionOperationId = productionOperationId;
      serialFailedOperation.CreatedDateTime = DateTime.UtcNow;
      serialFailedOperation.Status = SerialFailedOperationStatus.NotAction;
      serialFailedOperation.IsRepaired = false;
      repository.Add(serialFailedOperation);
      return serialFailedOperation;
    }
    #endregion

    #region Edit

    public SerialFailedOperation EditSerialFailedOperation(
        int id,
        byte[] rowVersion,
        TValue<bool> isRepaired = null,
        TValue<SerialFailedOperationStatus> status = null,
        TValue<DateTime> reviewedDateTime = null,
        TValue<int> reviewerUserId = null,
        TValue<int> confirmUserId = null,
        TValue<int> repairProductionId = null,
        TValue<string> description = null)
    {

      var serialFailedOperation = GetSerialFailedOperation(id: id);
      return EditSerialFailedOperation(
                    serialFailedOperation: serialFailedOperation,
                    rowVersion: rowVersion,
                    isRepaired: isRepaired,
                    status: status,
                    reviewerUserId: reviewerUserId,
                    confirmUserId: confirmUserId,
                    reviewedDateTime: reviewedDateTime,
                    description: description,
                    repairProductionId: repairProductionId);
    }

    public SerialFailedOperation EditSerialFailedOperation(
        SerialFailedOperation serialFailedOperation,
        byte[] rowVersion,
        TValue<bool> isRepaired = null,
        TValue<SerialFailedOperationStatus> status = null,
        TValue<DateTime> reviewedDateTime = null,
        TValue<int> reviewerUserId = null,
        TValue<int> confirmUserId = null,
        TValue<int> repairProductionId = null,
        TValue<string> description = null)
    {


      if (isRepaired != null)
        serialFailedOperation.IsRepaired = isRepaired;
      if (status != null)
        serialFailedOperation.Status = status;
      if (reviewedDateTime != null)
        serialFailedOperation.ReviewedDateTime = reviewedDateTime;
      if (reviewerUserId != null)
        serialFailedOperation.ReviewerUserId = reviewerUserId;

      if (confirmUserId != null)
        serialFailedOperation.ConfirmUserId = confirmUserId;

      if (repairProductionId != null)
        serialFailedOperation.RepairProductionId = repairProductionId;

      if (description != null)
        serialFailedOperation.Description = description;

      repository.Update(entity: serialFailedOperation, rowVersion: rowVersion);
      return serialFailedOperation;
    }

    #endregion

    #region Get
    public SerialFailedOperation GetSerialFailedOperation(int id) => GetSerialFailedOperation(selector: e => e, id: id);
    public TResult GetSerialFailedOperation<TResult>(
        Expression<Func<SerialFailedOperation, TResult>> selector,
        int id)
    {

      var serialFailedOperation = GetSerialFailedOperations(
                selector: selector,
                id: id)


                .FirstOrDefault();
      if (serialFailedOperation == null)
        throw new SerialFailedOperationNotFoundException(id);
      return serialFailedOperation;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetSerialFailedOperations<TResult>(
        Expression<Func<SerialFailedOperation, TResult>> selector,
        TValue<int> id = null,
        TValue<string> serial = null,
        TValue<int> stuffId = null,
        TValue<string> productionOrderCode = null,
        TValue<int> productionOrderId = null,
        TValue<int> stuffSerialStuffId = null,
        TValue<long> stuffSerialCode = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<bool> isRepaired = null,
        TValue<SerialFailedOperationStatus> status = null)
    {

      var query = repository.GetQuery<SerialFailedOperation>();
      if (id != null)
        query = query.Where(x => x.Id == id);

      if (!string.IsNullOrEmpty(serial))
      {
        var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(
                  serial: serial);
        query = query.Where(x => x.ProductionOperation.Production.StuffSerialCode == stuffSerial.Code && x.ProductionOperation.Production.StuffSerialStuffId == stuffSerial.StuffId);
      }


      if (status != null)
        query = query.Where(x => x.Status == status);

      if (stuffSerialStuffId != null)
        query = query.Where(x => x.ProductionOperation.Production.StuffSerialStuffId == stuffSerialStuffId);

      if (stuffSerialCode != null)
        query = query.Where(x => x.ProductionOperation.Production.StuffSerialCode == stuffSerialCode);

      if (fromDateTime != null)
        query = query.Where(x => x.CreatedDateTime >= fromDateTime);

      if (toDateTime != null)
        query = query.Where(x => x.CreatedDateTime <= toDateTime);

      if (productionOrderId != null)
        query = query.Where(x => x.ProductionOrderId == productionOrderId);

      if (isRepaired != null)
        query = query.Where(x => x.IsRepaired == isRepaired);

      if (productionOrderCode != null)
        query = query.Where(x => x.ProductionOrder.Code == productionOrderCode);

      if (stuffId != null)
        query = query.Where(x => x.ProductionOperation.Production.StuffSerialStuffId == stuffId);

      if (isRepaired != null)
        query = query.Where(x => x.IsRepaired == isRepaired);
      return query.Select(selector);
    }
    #endregion

    #region ToResult
    public IQueryable<SerialFailedOperationResult> ToSerialFaildOperationResult(
        IQueryable<SerialFailedOperation> serialFailedOperations,
        IQueryable<SerialFailedOperationFaultOperation> serialFailedOperationFaultOperations)
    {

      var result = from serialFailedOperation in serialFailedOperations
                   join serialFailedOperationFaultOperation in serialFailedOperationFaultOperations on serialFailedOperation.Id equals serialFailedOperationFaultOperation.SerialFailedOperationId into tempSerialFailedOperationFaultOperation
                   from tSerialFailedOperationFaultOperation in tempSerialFailedOperationFaultOperation.DefaultIfEmpty()
                   select new SerialFailedOperationResult
                   {
                     Id = serialFailedOperation.Id,
                     Serial = serialFailedOperation.ProductionOperation.Production.StuffSerial.Serial,
                     SuffId = serialFailedOperation.ProductionOperation.Production.StuffSerialStuffId,
                     StuffCode = serialFailedOperation.ProductionOperation.Production.StuffSerial.Stuff.Code,
                     StuffName = serialFailedOperation.ProductionOperation.Production.StuffSerial.Stuff.Name,
                     CreatedDateTime = serialFailedOperation.CreatedDateTime,
                     Status = serialFailedOperation.Status,
                     IsRepaired = serialFailedOperation.IsRepaired,
                     Description = serialFailedOperation.Description,
                     ProductionOrderCode = serialFailedOperation.ProductionOrder.Code,
                     ProductionLineId = serialFailedOperation.ProductionOrder.WorkPlanStep.ProductionLineId,
                     ConfirmUserId = serialFailedOperation.ConfirmUserId,
                     RowVersion = serialFailedOperation.RowVersion,
                     ReviewedDateTime = serialFailedOperation.ReviewedDateTime,
                     ReviewerUserFullName = serialFailedOperation.ReviewerUser.Employee.FirstName + " " + serialFailedOperation.ReviewerUser.Employee.LastName,
                     ConfirmUserFullName = serialFailedOperation.ConfirmUser.Employee.FirstName + " " + serialFailedOperation.ConfirmUser.Employee.LastName,
                     FailedInOperationId = serialFailedOperation.ProductionOperation.OperationId,
                     FailedInOperationTitle = serialFailedOperation.ProductionOperation.Operation.Title,
                     SerialFailedOperationFaultOperationId = tSerialFailedOperationFaultOperation.Id,
                     FaultOperationId = tSerialFailedOperationFaultOperation.OperationId,
                     FaultOperationTitle = tSerialFailedOperationFaultOperation.Operation.Title,
                   };

      return result;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<SerialFailedOperationResult> SortSerialFailedOperationResult(
      IQueryable<SerialFailedOperationResult> query,
      SortInput<SerialFailedOperationSortType> sort)
    {
      switch (sort.SortType)
      {
        case SerialFailedOperationSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case SerialFailedOperationSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case SerialFailedOperationSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case SerialFailedOperationSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case SerialFailedOperationSortType.CreatedDateTime:
          return query.OrderBy(a => a.CreatedDateTime, sort.SortOrder);
        case SerialFailedOperationSortType.FaultyOprationTitle:
          return query.OrderBy(a => a.FaultOperationTitle, sort.SortOrder);
        case SerialFailedOperationSortType.ReviewedDateTime:
          return query.OrderBy(a => a.ReviewedDateTime, sort.SortOrder);
        case SerialFailedOperationSortType.ReviewerUserFullName:
          return query.OrderBy(a => a.ReviewerUserFullName, sort.SortOrder);
        case SerialFailedOperationSortType.ConfirmUserFullName:
          return query.OrderBy(a => a.ConfirmUserFullName, sort.SortOrder);
        case SerialFailedOperationSortType.ProductionOrderCode:
          return query.OrderBy(a => a.ProductionOrderCode, sort.SortOrder);
        case SerialFailedOperationSortType.IsRepaired:
          return query.OrderBy(a => a.IsRepaired, sort.SortOrder);
        case SerialFailedOperationSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case SerialFailedOperationSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<SerialFailedOperationResult> SearchSerialFailedOperationResult(
     IQueryable<SerialFailedOperationResult> query,
     string search,
     AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.StuffName.Contains(search) ||
                item.StuffCode.Contains(search) ||
                item.Serial.Contains(search) ||
                item.ProductionOrderCode.Contains(search)
                select item;


      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region AcceptSerialFailedOperation
    public void AcceptSerialFailedOperation(int id, byte[] rowVersion)
    {

      EditSerialFailedOperation(
                id: id,
                rowVersion: rowVersion,
                status: SerialFailedOperationStatus.Accepted,
                confirmUserId: App.Providers.Security.CurrentLoginData.UserId);

    }
    #endregion

    #region RejectSerialFailedOperation
    public void RejectSerialFailedOperation(int id, byte[] rowVersion)
    {

      var serialFailedOperation = GetSerialFailedOperation(
                e => new
                {
                  Serial = e.ProductionOperation.Production.StuffSerial.Serial,
                  RepairWarehouseId = e.ProductionOrder.WorkPlanStep.ProductionLine.ProductionLineRepairUnit.WarehouseId,
                  ProductWarehouesId = e.ProductionOrder.WorkPlanStep.ProductionLine.ProductWarehouseId
                },
                id: id);
      var serialInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                serialFailedOperation.RepairWarehouseId,
                serial: serialFailedOperation.Serial)


                .FirstOrDefault();

      var addWarehouseIssueInput = new AddWarehouseIssueItemInput()
      {
        Amount = serialInventory.AvailableAmount,
        Serial = serialFailedOperation.Serial,
        StuffId = serialInventory.StuffId,
        UnitId = serialInventory.UnitId
      };

      var transactionBatchIssue = App.Internals.WarehouseManagement.AddTransactionBatch();

      var warehouseIssue = App.Internals.WarehouseManagement.AddDirectWarehouseIssueProcess(
                transactionBatch: transactionBatchIssue,
                fromWarehouseId: serialInventory.WarehouseId,
                toWarehouseId: serialFailedOperation.ProductWarehouesId,
                addWarehouseIssueItems: new[] { addWarehouseIssueInput },
                description: " حواله خودکار محصول از انبار ایستگاه رفع معیوبی به انبار مصرف خط تولید",
                toDepartmentId: null,
                toEmployeeId: null);

      EditSerialFailedOperation(
                id: id,
                rowVersion: rowVersion,
                status: SerialFailedOperationStatus.Rejected,
                confirmUserId: App.Providers.Security.CurrentLoginData.UserId);
    }
    #endregion

    #region GetSerialFailedProductionInfos
    public IQueryable<SerialFailedProductionInfoResult> GetSerialFailedProductionInfos(int serialFailedOperationId)
    {



      var serialFailedOperation = GetSerialFailedOperation(
                selector: e => new
                {
                  StuffSerialCode = e.ProductionOperation.Production.StuffSerialCode,
                  StuffSerialStuffId = e.ProductionOperation.Production.StuffSerialStuffId
                },
                id: serialFailedOperationId);

      var productions = App.Internals.Production.GetProductions(
                selector: e => e,
                stuffSerialCode: serialFailedOperation.StuffSerialCode,
                stuffSerialStuffId: serialFailedOperation.StuffSerialStuffId);

      var serialFailedOperationFaultOperationEmployees = GetSerialFailedOperatonFaultOperationEmployees(
                selector: e => new
                {
                  ProductionOperationEmployeeId = e.ProductionOperationEmployeeId,
                  OperationId = e.SerialFailedOperationFaultOperation.OperationId,
                  SerialFailedOperationFaultOperationId = e.SerialFailedOperationFaultOperationId
                },
                serialFailedOperationId: serialFailedOperationId);

      var productionOperationEmployees = from production in productions
                                         from productionOperation in production.ProductionOperations
                                         from productionOperationEmployee in productionOperation.ProductionOperationEmployeeGroup.ProductionOperationEmployees
                                         select new
                                         {
                                           OperationId = productionOperation.OperationId,
                                           OperationTitle = productionOperation.Operation.Title,
                                           ProductionOperationEmployeeId = productionOperationEmployee.Id,
                                           EmployeeId = productionOperationEmployee.EmployeeId,
                                           EmployeeFullName = productionOperationEmployee.Employee.FirstName + " " + productionOperationEmployee.Employee.LastName
                                         };



      var result = from productionOperationEmployee in productionOperationEmployees
                   join serialFailedOperationFaultOperationEmployee in serialFailedOperationFaultOperationEmployees
                         on new { productionOperationEmployee.OperationId, productionOperationEmployee.ProductionOperationEmployeeId }
                         equals
                         new { serialFailedOperationFaultOperationEmployee.OperationId, serialFailedOperationFaultOperationEmployee.ProductionOperationEmployeeId }
                         into tempSerialFailedOperationFaultOperationEmployee
                   from tSerialFailedOperationFaultOperationEmployee in tempSerialFailedOperationFaultOperationEmployee.DefaultIfEmpty()
                   select new SerialFailedProductionInfoResult
                   {
                     OperationId = productionOperationEmployee.OperationId,
                     OperationTitle = productionOperationEmployee.OperationTitle,
                     ProductionOperationEmployeeId = productionOperationEmployee.ProductionOperationEmployeeId,
                     SerialFailedOperationFaultOperationId = tSerialFailedOperationFaultOperationEmployee.SerialFailedOperationFaultOperationId,
                     EmployeeId = productionOperationEmployee.EmployeeId,
                     EmployeeFullName = productionOperationEmployee.EmployeeFullName,

                   };


      return result;
    }
    #endregion

    #region CheckSerialFailedOperationRepaired
    public void CheckSerialFailedOperationRepaired(
        int stuffSerialStuffId,
        long stuffSerialStuffCode,
        string serial)
    {

      var serialFailedOperation = GetSerialFailedOperations(
                selector: e => e,
                stuffSerialCode: stuffSerialStuffCode,
                stuffSerialStuffId: stuffSerialStuffId,
                isRepaired: false)

                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
      if (serialFailedOperation != null)
      {
        if (serialFailedOperation.Status == SerialFailedOperationStatus.NotAction)
        {
          throw new ThisSerialFailedIsInNotActionStatusException(serial: serial);
        }

        if (serialFailedOperation.Status == SerialFailedOperationStatus.Accepted)
        {
          throw new ThisSerialIsFaildAndNotRepairedYetException(serial: serial);
        }
      }

    }
    #endregion
  }
}
