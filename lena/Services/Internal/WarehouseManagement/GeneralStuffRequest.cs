using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.GeneralStuffRequest;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public GeneralStuffRequest AddGeneralStuffRequest(
        GeneralStuffRequest stuffRequest,
        int stuffId,
        double qty,
        byte unitId,
        short? billOfMaterialVersion,
        int? scrumEntityId,
        short fromWarehouseId,
        short? toWarehouseId,
        int? toEmployeeId,
        short? toDepartmentId,
        int? productionMaterialRequestId,
        StuffRequestType stuffRequestType,
        DateTime deadline,
        string description)
    {

      stuffRequest = stuffRequest ?? repository.Create<GeneralStuffRequest>();
      stuffRequest.ProductionMaterialRequestId = productionMaterialRequestId;
      stuffRequest.StuffId = stuffId;
      stuffRequest.Qty = qty;
      stuffRequest.StuffRequestQty = 0;
      stuffRequest.UnitId = unitId;
      stuffRequest.ScrumEntityId = scrumEntityId;
      stuffRequest.BillOfMaterialVersion = billOfMaterialVersion;
      stuffRequest.FromWarehouseId = fromWarehouseId;
      stuffRequest.ToWarehouseId = toWarehouseId;
      stuffRequest.StuffRequestType = stuffRequestType;
      stuffRequest.ToEmployeeId = toEmployeeId;
      stuffRequest.ToDepartmentId = toDepartmentId;
      stuffRequest.ProductionMaterialRequestId = productionMaterialRequestId;
      stuffRequest.Description = description;
      stuffRequest.Deadline = deadline;
      stuffRequest.UserId = App.Providers.Security.CurrentLoginData.UserId;
      stuffRequest.DateTime = DateTime.UtcNow;
      stuffRequest.Status = GeneralStuffRequestStatus.NotAction;

      repository.Add(stuffRequest);

      return stuffRequest;
    }


    public void AddPurchaseRequestGeneralStuffRequestProcess(
        int generalStuffRequestId,
        double qty,
        byte unitId,
        DateTime deadLine,
        bool isAlternativePurchaseRequest)
    {


      var generalRequest = GetGeneralStuffRequest(generalStuffRequestId);

      GeneralStuffRequestStatus status = generalRequest.Status;

      if (status == GeneralStuffRequestStatus.NotAction)
        status = GeneralStuffRequestStatus.Confirmed;

      if (!isAlternativePurchaseRequest && !status.HasFlag(GeneralStuffRequestStatus.PurchaseRequest))
        status = status | GeneralStuffRequestStatus.PurchaseRequest;

      if (isAlternativePurchaseRequest)
        status = status | GeneralStuffRequestStatus.AlternativePurchaseRequest | GeneralStuffRequestStatus.WarehouseRequest;

      var purchaseRequest = App.Internals.Supplies.AddPurchaseRequestProcess(
                deadline: deadLine,
                requestQty: qty,
                qty: qty,
                unitId: unitId,
                stuffId: generalRequest.StuffId,
                planCodeId: null,
                costCenterId: null,
                projectCode: null,
                supplyType: PurchaseRequestSupplyType.Normal,
                purchaseRequestStatus: PurchaseRequestStatus.Waiting,
                description: null);

      App.Internals.WarehouseManagement.AddGeneralStuffRequestDetail(null,
                    generalStuffRequestId: generalRequest.Id,
                    stuffRequestId: null,
                    purchaseRequestId: isAlternativePurchaseRequest ? null : (int?)purchaseRequest.Id,
                    alternativePurchaseRequestId: isAlternativePurchaseRequest ? (int?)purchaseRequest.Id : null,
                    qty: qty,
                    unitId: unitId,
                    description: null);

      App.Internals.WarehouseManagement.EditGeneralStuffRequest(
                   generalStuffRequest: generalRequest,
                    rowVersion: generalRequest.RowVersion,
                    status: status,
                    stuffRequestQty: isAlternativePurchaseRequest ? (double?)(generalRequest.StuffRequestQty + qty) : null,
                    purchaseRequestQty: isAlternativePurchaseRequest ? null : (double?)(generalRequest.PurchaseRequestQty + qty),
                    alternativePurchaseRequestQty: isAlternativePurchaseRequest ? (double?)(generalRequest.AlternativePurchaseRequestQty + qty) : null
                    );

      var stuffInfo = App.Internals.SaleManagement.GetStuffs(e => new { StuffCode = e.Code }, id: generalRequest.StuffId)


                .FirstOrDefault();

      var fromWarehouseName = App.Internals.WarehouseManagement.GetWarehouse(e => e.Name, id: generalRequest.FromWarehouseId);

      if (!isAlternativePurchaseRequest)
        App.Internals.Notification.NotifyToUser(
                 userId: generalRequest.UserId,
                 title: $"تایید درخواست کالای {stuffInfo.StuffCode}",
                 description: $"درخواست کالای {stuffInfo.StuffCode} از انبار {fromWarehouseName} توسط {App.Providers.Security.CurrentLoginData.UserFullName} تایید شد",
                 scrumEntityId: null);
    }


    #endregion
    #region AddProcess
    public void AddGeneralStuffRequestProcess(
       AddGeneralStuffRequestInput[] addGeneralStuffRequestInputs)
    {

      if (addGeneralStuffRequestInputs.Any())
      {
        var stuffRequestType = addGeneralStuffRequestInputs.FirstOrDefault().StuffRequestType;
        if (stuffRequestType == StuffRequestType.Consume)
        {
          var stuffIds = addGeneralStuffRequestInputs.Select(x => x.StuffId).ToArray();

          var stuffs = App.Internals.SaleManagement.GetStuffs(
                    selector: e => e,
                    ids: stuffIds,
                    stuffType: StuffType.Product);

          if (stuffs.Any())
          {
            var stuff = stuffs.FirstOrDefault();
            throw new ConsumeTypeStuffRequestCannotRequestProductTypeStuff(stuffCode: stuff.Code);
          }
        }
      }
      foreach (var item in addGeneralStuffRequestInputs)
      {

        App.Internals.WarehouseManagement.AddGeneralStuffRequest(
                       stuffRequest: null,
                       productionMaterialRequestId: item.ProductionMaterialRequestId,
                       stuffId: item.StuffId,
                       qty: item.Qty,
                       unitId: item.UnitId,
                       deadline: item.Deadline,
                       billOfMaterialVersion: item.BillOfMaterialVersion,
                       scrumEntityId: item.ScrumEntityId,
                       fromWarehouseId: item.FromWarehouseId,
                       toWarehouseId: item.ToWarehouseId,
                       toEmployeeId: item.ToEmployeeId,
                       toDepartmentId: item.ToDepartmentId,
                       stuffRequestType: item.StuffRequestType,
                       description: item.Description);

      }

    }
    #endregion
    #region Edit
    public GeneralStuffRequest EditGeneralStuffRequest(
        int id,
        byte[] rowVersion,
        TValue<int> stuffId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> stuffRequestQty = null,
        TValue<double> purchaseRequestQty = null,
        TValue<double> alternativePurchaseRequestQty = null,
        TValue<int> productionMaterialRequestId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<int?> scrumEntityId = null,
        TValue<short> fromWarehouseId = null,
        TValue<short?> toWarehouseId = null,
        TValue<int?> toEmployeeId = null,
        TValue<short?> toDepartmentId = null,
        TValue<DateTime> deadline = null,
        TValue<StuffRequestType> stuffRequestType = null,
        TValue<string> description = null,
        TValue<string> statusDescription = null,
        TValue<GeneralStuffRequestStatus> status = null)
    {

      var stuffRequest = GetGeneralStuffRequest(id: id);
      return EditGeneralStuffRequest(
                    generalStuffRequest: stuffRequest,
                    rowVersion: rowVersion,
                    stuffId: stuffId,
                    qty: qty,
                    stuffRequestQty: stuffRequestQty,
                    purchaseRequestQty: purchaseRequestQty,
                    alternativePurchaseRequestQty: alternativePurchaseRequestQty,
                    unitId: unitId,
                    productionMaterialRequestId: productionMaterialRequestId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    scrumEntityId: scrumEntityId,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId,
                    stuffRequestType: stuffRequestType,
                    toEmployeeId: toEmployeeId,
                    toDepartmentId: toDepartmentId,
                    deadline: deadline,
                    description: description,
                    statusDescription: statusDescription,
                    status: status);
    }
    public GeneralStuffRequest EditGeneralStuffRequest(
        GeneralStuffRequest generalStuffRequest,
        byte[] rowVersion,
        TValue<int> stuffId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> stuffRequestQty = null,
        TValue<double> purchaseRequestQty = null,
        TValue<double> alternativePurchaseRequestQty = null,
        TValue<int> productionMaterialRequestId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<int?> scrumEntityId = null,
        TValue<short> fromWarehouseId = null,
        TValue<short?> toWarehouseId = null,
        TValue<int?> toEmployeeId = null,
        TValue<short?> toDepartmentId = null,
        TValue<DateTime> deadline = null,
        TValue<StuffRequestType> stuffRequestType = null,
        TValue<string> description = null,
        TValue<string> statusDescription = null,
        TValue<GeneralStuffRequestStatus> status = null)
    {

      if (stuffId != null)
        generalStuffRequest.StuffId = stuffId;
      if (qty != null)
        generalStuffRequest.Qty = qty;
      if (unitId != null)
        generalStuffRequest.UnitId = unitId;
      if (stuffRequestQty != null)
        generalStuffRequest.StuffRequestQty = stuffRequestQty;
      if (purchaseRequestQty != null)
        generalStuffRequest.PurchaseRequestQty = purchaseRequestQty;
      if (alternativePurchaseRequestQty != null)
        generalStuffRequest.AlternativePurchaseRequestQty = alternativePurchaseRequestQty;
      if (productionMaterialRequestId != null)
        generalStuffRequest.ProductionMaterialRequestId = productionMaterialRequestId;
      if (scrumEntityId != null)
        generalStuffRequest.ScrumEntityId = scrumEntityId;
      if (fromWarehouseId != null)
        generalStuffRequest.FromWarehouseId = fromWarehouseId;
      if (toWarehouseId != null)
        generalStuffRequest.ToWarehouseId = toWarehouseId;
      if (toEmployeeId != null)
        generalStuffRequest.ToEmployeeId = toEmployeeId;
      if (toDepartmentId != null)
        generalStuffRequest.ToDepartmentId = toDepartmentId;
      if (stuffRequestType != null)
        generalStuffRequest.StuffRequestType = stuffRequestType;
      if (description != null)
        generalStuffRequest.Description = description;
      if (statusDescription != null)
        generalStuffRequest.StatusDescription = statusDescription;
      if (status != null)
        generalStuffRequest.Status = status;
      if (billOfMaterialVersion != null)
        generalStuffRequest.BillOfMaterialVersion = billOfMaterialVersion;

      repository.Update(generalStuffRequest, rowVersion);

      return generalStuffRequest;
    }
    #endregion

    #region EditProcess
    public GeneralStuffRequest EditGeneralStuffRequestProcess(
        int id,
        byte[] rowVersion,
        int stuffId,
        double qty,
        byte unitId,
        double stuffRequestQty,
        double purchaseRequestQty,
        int? scrumEntityId,
        short fromWarehouseId,
        short? toWarehouseId,
        int? toEmployeeId,
        short? toDepartmentId,
        DateTime deadline,
        StuffRequestType stuffRequestType,
        string description)
    {


      if (stuffRequestType == StuffRequestType.Consume)
      {
        var stuff = App.Internals.SaleManagement.GetStuffs(
                  selector: e => e,
                  id: stuffId,
                  stuffType: StuffType.Product)


                  .FirstOrDefault();

        if (stuff != null)
        {
          throw new ConsumeTypeStuffRequestCannotRequestProductTypeStuff(stuffCode: stuff.Code);
        }
      }
      return EditGeneralStuffRequest(
                    id: id,
                    rowVersion: rowVersion,
                    stuffId: stuffId,
                    qty: qty,
                    stuffRequestQty: stuffRequestQty,
                    purchaseRequestQty: purchaseRequestQty,
                    unitId: unitId,
                    scrumEntityId: scrumEntityId,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId,
                    stuffRequestType: stuffRequestType,
                    toEmployeeId: toEmployeeId,
                    toDepartmentId: toDepartmentId,
                    deadline: deadline,
                    description: description);
    }
    #endregion
    #region Get
    public GeneralStuffRequest GetGeneralStuffRequest(int id) => GetGeneralStuffRequest(selector: e => e, id: id);
    public TResult GetGeneralStuffRequest<TResult>(
        Expression<Func<GeneralStuffRequest, TResult>> selector,
        int id)
    {

      var result = GetGeneralStuffRequests(
                selector: selector,
                id: id)


            .FirstOrDefault();

      if (result == null)
        throw new GeneralStuffRequestNotFoundException(id);

      return result;
    }

    #endregion
    #region Gets
    public IQueryable<TResult> GetGeneralStuffRequests<TResult>(
        Expression<Func<GeneralStuffRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<int> userId = null,
        TValue<int> stuffId = null,
        TValue<int> scrumEntityId = null,
        TValue<int> fromWarehouseId = null,
        TValue<int> toWarehouseId = null,
        TValue<int> toDepartmentId = null,
        TValue<int> productionMaterialRequestId = null,
        TValue<StuffRequestType> stuffRequestType = null,
        TValue<string> productionOrderCode = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<GeneralStuffRequestStatus> status = null,
        TValue<DateTime> deadline = null,
        TValue<int> toEmployeeId = null,
        TValue<int> employeeId = null
        )
    {


      var generalStuffRequest = repository.GetQuery<GeneralStuffRequest>();
      if (id != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.Id == id);
      if (ids != null && ids.Value.Length != 0)
        generalStuffRequest = generalStuffRequest.Where(r => ids.Value.Contains(r.Id));
      if (userId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.UserId == userId);
      if (stuffId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.StuffId == stuffId);
      if (scrumEntityId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.ScrumEntityId == scrumEntityId);
      if (fromWarehouseId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.FromWarehouseId == fromWarehouseId);
      if (toWarehouseId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.ToWarehouseId == toWarehouseId);
      if (stuffRequestType != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.StuffRequestType == stuffRequestType);
      if (productionMaterialRequestId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.ProductionMaterialRequestId == productionMaterialRequestId);
      if (!string.IsNullOrEmpty(productionOrderCode))
        generalStuffRequest = generalStuffRequest.Where(r => r.ProductionMaterialRequest.ProductionOrder.Code == productionOrderCode);
      if (fromDateTime != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.DateTime >= fromDateTime);
      if (toDateTime != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.DateTime <= toDateTime);
      if (toDepartmentId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.ToDepartmentId == toDepartmentId);
      if (status != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.Status.HasFlag(status));
      if (deadline != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.Deadline == deadline);
      if (toEmployeeId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.ToEmployeeId == toEmployeeId);
      if (employeeId != null)
        generalStuffRequest = generalStuffRequest.Where(r => r.User.Employee.Id == employeeId);

      return generalStuffRequest.Select(selector);
    }
    #endregion

    #region Search
    public IQueryable<GeneralStuffRequestFullResult> SearchGeneralStuffRequestResult(IQueryable<GeneralStuffRequestFullResult> query, string search, string scrumProjectCode = null, int? requestConfirmerUserGroupId = null)
    {

      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.ToEmployeeFullName.Contains(search) ||
                    item.FromWarehouseName.Contains(search) ||
                    item.ToDepartmentName.Contains(search) ||
                    item.ToEmployeeFullName.Contains(search) ||
                    item.ToWarehouseName.Contains(search) ||
                    item.ProductionOrderCode.Contains(search) ||
                    item.ScrumProjectName.Contains(search) ||
                    item.Description.Contains(search) ||
                    item.StatusDescription.Contains(search)
                select item;
      if (scrumProjectCode != null)
        query = query.Where(t => t.ScrumProjectCode == scrumProjectCode);
      if (requestConfirmerUserGroupId != null)
        query = query.Where(i => i.RequestConfirmerUserGroupIds.Any(r => r.Value == requestConfirmerUserGroupId));

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<GeneralStuffRequestFullResult> SortGeneralStuffRequestFullResult(IQueryable<GeneralStuffRequestFullResult> query,
        SortInput<GeneralStuffRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case GeneralStuffRequestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case GeneralStuffRequestSortType.StuffRequestType:
          return query.OrderBy(a => a.StuffRequestType, sort.SortOrder);
        case GeneralStuffRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case GeneralStuffRequestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case GeneralStuffRequestSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case GeneralStuffRequestSortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, sort.SortOrder);
        case GeneralStuffRequestSortType.BillOfMaterialTitle:
          return query.OrderBy(a => a.BillOfMaterialTitle, sort.SortOrder);
        case GeneralStuffRequestSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case GeneralStuffRequestSortType.StuffRequestQty:
          return query.OrderBy(a => a.StuffRequestQty, sort.SortOrder);
        case GeneralStuffRequestSortType.PurchaseRequestQty:
          return query.OrderBy(a => a.PurchaseRequestQty, sort.SortOrder);
        case GeneralStuffRequestSortType.AlternativePurchaseRequestQty:
          return query.OrderBy(a => a.AlternativePurchaseRequestQty, sort.SortOrder);
        case GeneralStuffRequestSortType.RemainQty:
          return query.OrderBy(a => a.RemainQty, sort.SortOrder);
        case GeneralStuffRequestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case GeneralStuffRequestSortType.FromWarehouseName:
          return query.OrderBy(a => a.FromWarehouseName, sort.SortOrder);
        case GeneralStuffRequestSortType.ToWarehouseName:
          return query.OrderBy(a => a.ToWarehouseName, sort.SortOrder);
        case GeneralStuffRequestSortType.ToDepartmentName:
          return query.OrderBy(a => a.ToDepartmentName, sort.SortOrder);
        case GeneralStuffRequestSortType.ToEmployeeFullName:
          return query.OrderBy(a => a.ToEmployeeFullName, sort.SortOrder);
        case GeneralStuffRequestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case GeneralStuffRequestSortType.ProductionOrderCode:
          return query.OrderBy(a => a.ProductionOrderCode, sort.SortOrder);
        case GeneralStuffRequestSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case GeneralStuffRequestSortType.Deadline:
          return query.OrderBy(a => a.Deadline, sort.SortOrder);
        case GeneralStuffRequestSortType.ScrumProjectCode:
          return query.OrderBy(a => a.ScrumProjectCode, sort.SortOrder);
        case GeneralStuffRequestSortType.ScrumProjectName:
          return query.OrderBy(a => a.ScrumProjectName, sort.SortOrder);
        case GeneralStuffRequestSortType.StatusDescription:
          return query.OrderBy(a => a.StatusDescription, sort.SortOrder);
        case GeneralStuffRequestSortType.StuffAvailableQty:
          return query.OrderBy(a => a.WarehouseAvailableQty, sort.SortOrder);
        case GeneralStuffRequestSortType.RequestConfirmerUserGroupNames:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);// Can Not sort by RequestConfirmerUserGroupNames Field
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToGeneralStuffRequestResult
    public Expression<Func<GeneralStuffRequest, GeneralStuffRequestResult>> ToGeneralStuffRequestResult =
        request => new GeneralStuffRequestResult
        {
          Id = request.Id,
          StuffId = request.StuffId,
          StuffCode = request.Stuff.Code,
          StuffName = request.Stuff.Name,
          StuffType = request.Stuff.StuffType,
          Qty = request.Qty,
          UnitId = request.UnitId,
          UnitName = request.Unit.Name,
          StuffRequestQty = request.StuffRequestQty,
          PurchaseRequestQty = request.PurchaseRequestQty,
          AlternativePurchaseRequestQty = request.AlternativePurchaseRequestQty,
          RemainQty = request.Qty - (request.StuffRequestQty + request.PurchaseRequestQty),
          WarehouseAvailableQty = 0,
          StatusDescription = request.StatusDescription,
          FromWarehouseId = request.FromWarehouseId,
          FromWarehouseName = request.FromWarehosue.Name,
          ToWarehouseId = request.ToWarehouseId,
          ToWarehouseName = request.ToWarehouse.Name,
          ToDepartmentId = request.ToDepartmentId,
          ToDepartmentName = request.ToDepartment.Name,
          BillOfMaterialVersion = request.BillOfMaterialVersion,
          BillOfMaterialTitle = request.BillOfMaterial.Title,
          DateTime = request.DateTime,
          Deadline = request.Deadline,
          Description = request.Description,
          EmployeeFullName = request.User.Employee.FirstName + " " + request.User.Employee.LastName,
          ProductionMaterialRequestId = request.ProductionMaterialRequestId,
          ProductionOrderCode = request.ProductionMaterialRequest.ProductionOrder.Code,
          ScrumProjectId = request.ScrumEntityId,
          ScrumProjectCode = request.ScrumEntity.Code,
          ScrumProjectName = request.ScrumEntity.Name,
          Status = request.Status,
          RowVersion = request.RowVersion,
          StuffRequestType = request.StuffRequestType
          //RequestConfirmerUserGroupIds =  ,
          //RequestConfirmerUserGroupRawNames = ,
        };
    #endregion
    public void DeleteGeneralStuffRequest(int id)
    {

      var stuffRequestItem = GetGeneralStuffRequest(id: id);
      repository.Delete(stuffRequestItem);
    }
  }
}
