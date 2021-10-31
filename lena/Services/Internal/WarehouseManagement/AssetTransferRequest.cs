using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public AssetTransferRequest AddAssetTransferRequestProcess(
        int? newEmployeeId,
        short? newDepartmentId,
        int assetId,
        string description)
    {
      var previousAssetTransferRequest = GetAssetTransferRequests(selector: e => e,
                newDepartmentId: newDepartmentId,
                newEmployeeId: newEmployeeId,
                assetId: assetId)
            .Where(i =>
            i.Status == AssetTransferRequestStatus.NotAction);
      if (previousAssetTransferRequest.Any())
        throw new ThisRequestHasAlreadyBeenRegisteredException();
      if (newEmployeeId != null)
      {
        var employee = App.Internals.UserManagement.GetEmployee(id: newEmployeeId.Value);
        if (newDepartmentId != employee.DepartmentId)
          throw new SelectedEmployeeIsNotMembersOfTheSelectedDepartmentException();
      }
      var asset = GetAsset(id: assetId);
      if (asset.Status != AssetStatus.Referred)
        throw new TransferringAssetException();
      if (asset.DepartmentId == newDepartmentId && asset.EmployeeId == newEmployeeId)
        throw new NotDiffrenceInRequestException(asset.Code);
      if (newEmployeeId == null && newDepartmentId == null)
        description = description + " درخواست برگشت به انبار مبدا ";
      var assetTransferRequest = AddAssetTransferRequest(newEmployeeId: newEmployeeId,
                newDepartmentId: newDepartmentId,
                assetId: assetId,
                description: description);
      EditAsset(id: assetId,
                rowVersion: asset.RowVersion,
                employeeId: assetTransferRequest.Asset.EmployeeId,
                departmentId: assetTransferRequest.Asset.DepartmentId,
                status: AssetStatus.IsTransferring);
      return assetTransferRequest;
    }
    #endregion
    #region Add
    public AssetTransferRequest AddAssetTransferRequest(
        int? newEmployeeId,
        short? newDepartmentId,
        int assetId,
        string description)
    {
      var assetTransferRequest = repository.Create<AssetTransferRequest>();
      assetTransferRequest.AssetId = assetId;
      assetTransferRequest.NewEmployeeId = newEmployeeId;
      assetTransferRequest.NewDepartmentId = newDepartmentId;
      assetTransferRequest.Description = description;
      assetTransferRequest.RequestingUserId = App.Providers.Security.CurrentLoginData.UserId;
      assetTransferRequest.RequestDateTime = DateTime.UtcNow;
      assetTransferRequest.Status = AssetTransferRequestStatus.NotAction;
      repository.Add(assetTransferRequest);
      return assetTransferRequest;
    }
    #endregion
    #region Confirm
    public AssetTransferRequest ConfirmAssetTransferRequestProcess(
        byte[] rowVersion,
        int id,
        TValue<AssetTransferRequestStatus> status = null
        )
    {
      var assetTransferRequest = GetAssetTransferRequest(id: id);
      if (assetTransferRequest.Status != AssetTransferRequestStatus.NotAction)
        throw new ThisRequestHasAlreadyBeenSetException();
      var asset = GetAsset(id: assetTransferRequest.Asset.Id);
      if (status == AssetTransferRequestStatus.Accepted)
      {
        if (assetTransferRequest.NewEmployeeId == null &&
                 assetTransferRequest.NewDepartmentId == null)
        {
          var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
          var billOfMaterialVersion = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(stuffId: asset.StuffId, stuffSerialCode: asset.StuffSerial.Code);
          AddWarehouseTransaction(
                     effectDateTime: DateTime.Now.ToUniversalTime(),
                     billOfMaterialVersion: billOfMaterialVersion,
                     amount: asset.StuffSerial.InitQty,
                     unitId: asset.StuffSerial.InitUnitId,
                     description: "برگشت اموال با کد:" + asset.Code + "به انبار مبدا",
                     referenceTransaction: null,
                     transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportProduction.Id,
                     stuffId: asset.StuffId,
                     stuffSerialCode: asset.StuffSerial.Code,
                     transactionBatchId: transactionBatch.Id,
                     warehouseId: asset.WarehouseId);
          EditAssetProcess(id: assetTransferRequest.Asset.Id,
                    assetTransferRequestStatus: status,
                    assetStatus: AssetStatus.DeliveredToWarehouse,
                    rowVersion: asset.RowVersion);
        }
        else
        {
          EditAssetProcess(id: assetTransferRequest.Asset.Id,
                    employeeId: assetTransferRequest.NewEmployeeId,
                    departmentId: assetTransferRequest.NewDepartmentId,
                    assetTransferRequestStatus: status,
                    assetStatus: AssetStatus.Referred,
                    rowVersion: asset.RowVersion);
        }
      }
      else if (status == AssetTransferRequestStatus.Rejected)
      {
        EditAssetProcess(id: assetTransferRequest.Asset.Id,
                  assetTransferRequestStatus: status,
                  assetStatus: AssetStatus.Referred,
                  rowVersion: asset.RowVersion);
      }
      ConfirmAssetTransferRequest(id: id, rowVersion: rowVersion, status: status);
      return assetTransferRequest;
    }
    public AssetTransferRequest ConfirmAssetTransferRequest(
        byte[] rowVersion,
        int id,
        TValue<AssetTransferRequestStatus> status = null
        )
    {
      var assetTransferRequest = GetAssetTransferRequest(id: id);
      assetTransferRequest.Status = status;
      assetTransferRequest.ConfirmDateTime = DateTime.UtcNow;
      assetTransferRequest.ConfirmerUserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Update(assetTransferRequest, assetTransferRequest.RowVersion);
      return assetTransferRequest;
    }
    #endregion
    #region Delete
    public void DeleteAssetTransferRequest(int id)
    {
      var assetTransferRequest = GetAssetTransferRequest(id: id);
      repository.Delete(assetTransferRequest);
    }
    #endregion
    #region Get
    public AssetTransferRequest GetAssetTransferRequest(int id) => GetAssetTransferRequest(selector: e => e, id: id);
    internal TResult GetAssetTransferRequest<TResult>(
    Expression<Func<AssetTransferRequest, TResult>> selector,
    int id)
    {
      var assetTransferRequest = GetAssetTransferRequests(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (assetTransferRequest == null)
        throw new AssetTransferRequestNotFoundException(id: id);
      return assetTransferRequest;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetAssetTransferRequests<TResult>(
        Expression<Func<AssetTransferRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<int> assetId = null,
        TValue<int> stuffId = null,
        TValue<int> newEmployeeId = null,
        TValue<short> newDepartmentId = null,
        TValue<string> assetCode = null)
    {
      var assetTransferRequest = repository.GetQuery<AssetTransferRequest>();
      if (id != null)
        assetTransferRequest = assetTransferRequest.Where(i => i.Id == id);
      if (assetId != null)
        assetTransferRequest = assetTransferRequest.Where(i => i.AssetId == assetId);
      if (stuffId != null)
        assetTransferRequest = assetTransferRequest.Where(i => i.Asset.StuffId == stuffId);
      if (newEmployeeId != null)
        assetTransferRequest = assetTransferRequest.Where(i => i.NewEmployeeId == newEmployeeId);
      if (newDepartmentId != null)
        assetTransferRequest = assetTransferRequest.Where(i => i.NewDepartmentId == newDepartmentId);
      if (assetCode != null)
        assetTransferRequest = assetTransferRequest.Where(i => i.Asset.Code == assetCode);
      return assetTransferRequest.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<AssetTransferRequest, AssetTransferRequestResult>> ToAssetTransferRequestResult =
     assetTransferRequest => new AssetTransferRequestResult
     {
       Id = assetTransferRequest.Id,
       StuffId = assetTransferRequest.Asset.StuffId,
       StuffCode = assetTransferRequest.Asset.Stuff.Code,
       StuffName = assetTransferRequest.Asset.Stuff.Name,
       AssetId = assetTransferRequest.AssetId,
       AssetCode = assetTransferRequest.Asset.Code,
       NewEmployeeId = assetTransferRequest.NewEmployeeId,
       NewEmployeeName = assetTransferRequest.NewEmployee.FirstName + " " + assetTransferRequest.NewEmployee.LastName,
       NewDepartmentId = assetTransferRequest.NewDepartmentId,
       NewDepartmentName = assetTransferRequest.NewDepartment.Name,
       RequestingUserId = assetTransferRequest.RequestingUserId,
       RequestingUserFullName = assetTransferRequest.RequestingUser.Employee.FirstName + " " + assetTransferRequest.RequestingUser.Employee.LastName,
       ConfirmerUserId = assetTransferRequest.ConfirmerUserId,
       ConfirmerUserFullName = assetTransferRequest.ConfirmerUser.Employee.FirstName + " " + assetTransferRequest.ConfirmerUser.Employee.LastName,
       RequestDateTime = assetTransferRequest.RequestDateTime,
       ConfirmDateTime = assetTransferRequest.ConfirmDateTime,
       Description = assetTransferRequest.Description,
       Status = assetTransferRequest.Status
     };
    #endregion
    #region Search
    public IQueryable<AssetTransferRequestResult> SearchAssetTransferRequest(IQueryable<AssetTransferRequestResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.StuffCode.Contains(searchText) ||
            item.StuffName.Contains(searchText) ||
            item.StuffName.Contains(searchText) ||
            item.AssetCode.Contains(searchText) ||
            item.NewEmployeeName.Contains(searchText) ||
            item.NewDepartmentName.Contains(searchText) ||
            item.RequestingUserFullName.Contains(searchText) ||
            item.ConfirmerUserFullName.Contains(searchText) ||
            item.Description.Contains(searchText) ||
            item.Status.ToString().Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<AssetTransferRequestResult> SortAssetTransferRequestResult(IQueryable<AssetTransferRequestResult> query,
        SortInput<AssetTransferRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case AssetTransferRequestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case AssetTransferRequestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case AssetTransferRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case AssetTransferRequestSortType.AssetCode:
          return query.OrderBy(a => a.AssetCode, sort.SortOrder);
        case AssetTransferRequestSortType.NewEmployeeName:
          return query.OrderBy(a => a.NewEmployeeName, sort.SortOrder);
        case AssetTransferRequestSortType.NewDepartmentName:
          return query.OrderBy(a => a.NewDepartmentName, sort.SortOrder);
        case AssetTransferRequestSortType.RequestingUserFullName:
          return query.OrderBy(a => a.RequestingUserFullName, sort.SortOrder);
        case AssetTransferRequestSortType.RequestDateTime:
          return query.OrderBy(a => a.RequestDateTime, sort.SortOrder);
        case AssetTransferRequestSortType.ConfirmerUserFullName:
          return query.OrderBy(a => a.ConfirmerUserFullName, sort.SortOrder);
        case AssetTransferRequestSortType.ConfirmDateTime:
          return query.OrderBy(a => a.ConfirmDateTime, sort.SortOrder);
        case AssetTransferRequestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case AssetTransferRequestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}