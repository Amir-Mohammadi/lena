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
    #region Add  Process
    public Asset AddAssetProcess(
        string code,
        int stuffId,
        int? employeeId,
        short departmentId,
        short warehouseId,
        string description,
        string serial)
    {

      var asset = AddAsset(
                code: code,
                stuffId: stuffId,
                employeeId: employeeId,
                departmentId: departmentId,
                warehouseId: warehouseId,
                description: description,
                serial: serial);
      var assetLog = AddAssetLog(
                employeeId: employeeId,
                departmentId: departmentId,
                assetId: asset.Id,
                description: description);
      return asset;
    }
    #endregion
    #region Add
    public Asset AddAsset(
        string code,
        int stuffId,
        int? employeeId,
        short departmentId,
        short warehouseId,
        string description,
        string serial)
    {

      var asset = repository.Create<Asset>();

      var stuffSerial = GetStuffSerial(serial: serial);
      asset.Code = code;
      asset.StuffId = stuffId;
      asset.EmployeeId = employeeId;
      asset.DepartmentId = departmentId;
      asset.WarehouseId = warehouseId;
      asset.Description = description;
      asset.UserId = App.Providers.Security.CurrentLoginData.UserId;
      asset.CreateDateTime = DateTime.UtcNow;
      asset.Status = AssetStatus.IsTransferring;
      asset.StuffSerial = stuffSerial;
      repository.Add(asset);
      return asset;
    }
    #endregion
    #region Edit Process
    public Asset EditAssetProcess(
        byte[] rowVersion,
        int id,
        TValue<int> employeeId = null,
        TValue<short> departmentId = null,
        TValue<AssetTransferRequestStatus> assetTransferRequestStatus = null,
        TValue<AssetStatus> assetStatus = null,
        TValue<string> description = null
        )
    {

      var asset = EditAsset(
                id: id,
                rowVersion: rowVersion,
                employeeId: employeeId,
                departmentId: departmentId,
                status: assetStatus,
                description: description);
      if (assetTransferRequestStatus == AssetTransferRequestStatus.Accepted)
      {
        var assetLog = AddAssetLog(
                  employeeId: employeeId ?? asset.EmployeeId,
                  departmentId: departmentId ?? asset.DepartmentId,
                  assetId: asset.Id,
                  description: description);
      }
      return asset;
    }
    #endregion
    #region Edit
    public Asset EditAsset(
        byte[] rowVersion,
        int id,
        TValue<int> employeeId = null,
        TValue<short> departmentId = null,
        TValue<AssetStatus> status = null,
        TValue<string> description = null
        )
    {

      var asset = GetAsset(id: id);
      if (employeeId != null)
        asset.EmployeeId = employeeId;
      if (departmentId != null)
        asset.DepartmentId = departmentId;
      if (status != null)
        asset.Status = status;
      if (description != null)
        asset.Description = description;

      repository.Update(asset, rowVersion);
      return asset;
    }
    #endregion

    #region Get
    public Asset GetAsset(int id) => GetAsset(selector: e => e, id: id);
    internal TResult GetAsset<TResult>(
    Expression<Func<Asset, TResult>> selector,
    int id)
    {


      var asset = GetAssets(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (asset == null)
        throw new AssetNotFoundException(id: id);
      return asset;

    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetAssets<TResult>(
        Expression<Func<Asset, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<int> stuffId = null,
        TValue<int> employeeId = null,
        TValue<short> departmentId = null,
        TValue<string> serial = null)
    {


      var asset = repository.GetQuery<Asset>();

      if (id != null)
        asset = asset.Where(i => i.Id == id);
      if (code != null)
        asset = asset.Where(i => i.Code == code);
      if (stuffId != null)
        asset = asset.Where(i => i.StuffId == stuffId);
      if (employeeId != null)
        asset = asset.Where(i => i.EmployeeId == employeeId);
      if (departmentId != null)
        asset = asset.Where(i => i.DepartmentId == departmentId);
      if (serial != null)
        asset = asset.Where(i => i.StuffSerial.Serial == serial);

      return asset.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<Asset, AssetResult>> ToAssetResult =
     asset => new AssetResult
     {
       Id = asset.Id,
       Code = asset.Code,
       StuffId = asset.StuffId,
       StuffCode = asset.Stuff.Code,
       StuffName = asset.Stuff.Name,
       EmployeeId = asset.EmployeeId,
       EmployeeName = asset.Employee.FirstName + " " + asset.Employee.LastName,
       DepartmentId = asset.DepartmentId,
       DepartmentName = asset.Department.Name,
       UserId = asset.UserId,
       UserFullName = asset.User.Employee.FirstName + " " + asset.User.Employee.LastName,
       WarehouseId = asset.WarehouseId,
       WarehouseName = asset.Warehouse.Name,
       Serial = asset.StuffSerial.Serial,
       CreateDateTime = asset.CreateDateTime,
       Description = asset.Description,
       Status = asset.Status
     };

    #endregion

    #region Search
    public IQueryable<AssetResult> SearchAsset(IQueryable<AssetResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrEmpty(searchText))
      {
        query = query.Where(item =>
            item.Code.Contains(searchText) ||
            item.StuffCode.Contains(searchText) ||
            item.StuffName.Contains(searchText) ||
            item.EmployeeName.Contains(searchText) ||
            item.DepartmentName.Contains(searchText) ||
            item.UserFullName.Contains(searchText) ||
            item.WarehouseName.Contains(searchText) ||
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
    public IOrderedQueryable<AssetResult> SortAssetResult(IQueryable<AssetResult> query,
        SortInput<AssetSortType> sort)
    {
      switch (sort.SortType)
      {
        case AssetSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case AssetSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case AssetSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case AssetSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case AssetSortType.EmployeeName:
          return query.OrderBy(a => a.EmployeeName, sort.SortOrder);
        case AssetSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case AssetSortType.UserFullName:
          return query.OrderBy(a => a.UserFullName, sort.SortOrder);
        case AssetSortType.CreateDateTime:
          return query.OrderBy(a => a.CreateDateTime, sort.SortOrder);
        case AssetSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case AssetSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case AssetSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case AssetSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

  }
}
