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
    public AssetLog AddAssetLog(
        int? employeeId,
        short departmentId,
        int assetId,
        string description)
    {

      var assetLog = repository.Create<AssetLog>();
      assetLog.AssetId = assetId;
      assetLog.EmployeeId = employeeId;
      assetLog.DepartmentId = departmentId;
      assetLog.Description = description;
      assetLog.UserId = App.Providers.Security.CurrentLoginData.UserId;
      assetLog.CreateDateTime = DateTime.UtcNow;
      repository.Add(assetLog);
      return assetLog;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetAssetLogs<TResult>(
        Expression<Func<AssetLog, TResult>> selector,
        TValue<int> assetId = null,
        TValue<string> assetCode = null,
        TValue<int> stuffId = null,
        TValue<int> employeeId = null,
        TValue<short> departmentId = null)
    {


      var assetLog = repository.GetQuery<AssetLog>();

      if (assetId != null)
        assetLog = assetLog.Where(i => i.Id == assetId);
      if (assetCode != null && assetCode != "")
        assetLog = assetLog.Where(i => i.Asset.Code == assetCode);
      if (stuffId != null)
        assetLog = assetLog.Where(i => i.Asset.StuffId == stuffId);
      if (employeeId != null)
        assetLog = assetLog.Where(i => i.EmployeeId == employeeId);
      if (departmentId != null)
        assetLog = assetLog.Where(i => i.DepartmentId == departmentId);

      return assetLog.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<AssetLog, AssetLogResult>> ToAssetLogResult =
     assetLog => new AssetLogResult
     {
       AssetId = assetLog.AssetId,
       AssetCode = assetLog.Asset.Code,
       StuffId = assetLog.Asset.StuffId,
       StuffCode = assetLog.Asset.Stuff.Code,
       StuffName = assetLog.Asset.Stuff.Name,
       EmployeeId = assetLog.EmployeeId,
       EmployeeName = assetLog.Employee.FirstName + " " + assetLog.Employee.LastName,
       DepartmentId = assetLog.DepartmentId,
       DepartmentName = assetLog.Department.Name,
       UserId = assetLog.UserId,
       UserFullName = assetLog.User.Employee.FirstName + " " + assetLog.User.Employee.LastName,
       WarehouseId = assetLog.Asset.WarehouseId,
       WarehouseName = assetLog.Asset.Warehouse.Name,
       Serial = assetLog.Asset.StuffSerial.Code,
       CreateDateTime = assetLog.CreateDateTime,
       Description = assetLog.Description,
     };

    #endregion

    #region Search
    public IQueryable<AssetLogResult> SearchAssetLog(IQueryable<AssetLogResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.AssetCode.Contains(searchText) ||
            item.StuffCode.Contains(searchText) ||
            item.StuffName.Contains(searchText) ||
            item.EmployeeName.Contains(searchText) ||
            item.DepartmentName.Contains(searchText) ||
            item.UserFullName.Contains(searchText) ||
            item.WarehouseName.Contains(searchText) ||
            item.Description.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<AssetLogResult> SortAssetLogResult(IQueryable<AssetLogResult> query,
        SortInput<AssetLogSortType> sort)
    {
      switch (sort.SortType)
      {
        case AssetLogSortType.Id:
          return query.OrderBy(a => a.AssetId, sort.SortOrder);
        case AssetLogSortType.Code:
          return query.OrderBy(a => a.AssetCode, sort.SortOrder);
        case AssetLogSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case AssetLogSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case AssetLogSortType.EmployeeName:
          return query.OrderBy(a => a.EmployeeName, sort.SortOrder);
        case AssetLogSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case AssetLogSortType.UserFullName:
          return query.OrderBy(a => a.UserFullName, sort.SortOrder);
        case AssetLogSortType.CreateDateTime:
          return query.OrderBy(a => a.CreateDateTime, sort.SortOrder);
        case AssetLogSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion


  }
}
