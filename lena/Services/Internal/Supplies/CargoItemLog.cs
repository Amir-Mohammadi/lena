using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.StaticData;
using lena.Models.ApplicationBase.Unit;
using lena.Models.Common;
using lena.Models.Supplies.Cargo;
using lena.Models.Supplies.CargoItem;
using lena.Models.Supplies.LadingItemDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public CargoItemLog AddCargoItemLog(
        int cargoItemId,
        string cargoItemCode,
        bool isDelete,
        double newcargoItemQty,
        double oldCargoItemQty,
        CargoItemLogStatus cargoItemLogStatus)
    {

      var cargoItemLog = repository.Create<CargoItemLog>();
      cargoItemLog.ModifierUserId = App.Providers.Security.CurrentLoginData.UserId;
      cargoItemLog.ModifyDateTime = DateTime.UtcNow.ToUniversalTime();
      cargoItemLog.CargoItemId = cargoItemId;
      cargoItemLog.CargoItemCode = cargoItemCode;
      cargoItemLog.IsDelete = isDelete;
      cargoItemLog.CargoItemLogStatus = cargoItemLogStatus;
      cargoItemLog.NewCargoItemQty = newcargoItemQty;
      cargoItemLog.OldCargoItemDetailQty = oldCargoItemQty;
      repository.Add(cargoItemLog);
      return cargoItemLog;
    }
    #endregion

    #region Get
    public CargoItemLog GetCargoItemLog(int id) => GetCargoItemLog(selector: e => e, id: id);
    public TResult GetCargoItemLog<TResult>(
        Expression<Func<CargoItemLog, TResult>> selector,
        int id)
    {

      var cargoItemLog = GetCargoItemLogs(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (cargoItemLog == null)
        throw new CargoItemLogNotFoundException(id);
      return cargoItemLog;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetCargoItemLogs<TResult>(
        Expression<Func<CargoItemLog, TResult>> selector,
        TValue<int> id = null,
        TValue<string> cargoItemCode = null,
        TValue<int> cargoItemId = null,
        TValue<DateTime> fromModifyDateTime = null,
        TValue<DateTime> toModifyDateTime = null,
        TValue<int> stuffId = null,
        TValue<int> modifierEmployeeId = null
        )
    {

      var query = repository.GetQuery<CargoItemLog>();
      if (id != null)
        query = query.Where(t => t.Id == id);
      if (cargoItemId != null)
        query = query.Where(t => t.CargoItemId == cargoItemId);
      if (cargoItemCode != null)
        query = query.Where(t => t.CargoItemCode == cargoItemCode);
      if (fromModifyDateTime != null)
        query = query.Where(t => t.ModifyDateTime >= fromModifyDateTime);
      if (toModifyDateTime != null)
        query = query.Where(t => t.ModifyDateTime <= toModifyDateTime);
      if (stuffId != null)
        query = query.Where(t => t.CargoItem.PurchaseOrder.StuffId == stuffId);
      if (modifierEmployeeId != null)
        query = query.Where(t => t.ModifierUser.Employee.Id == modifierEmployeeId);
      return query.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<CargoItemLog, CargoItemLogResult>> ToCargoItemLogResult =
        (cargoItemLog) => new CargoItemLogResult
        {
          Id = cargoItemLog.Id,
          CargoItemId = cargoItemLog.CargoItemId,
          CargoItemCode = cargoItemLog.CargoItem.Code,
          ModifierUserId = cargoItemLog.ModifierUser.Id,
          ModifierUserFullName = cargoItemLog.ModifierUser.UserName,
          ModifierEmployeeId = cargoItemLog.ModifierUser.Employee.Id,
          ModifierEmployeeFullName = cargoItemLog.ModifierUser.Employee.FirstName + " " + cargoItemLog.ModifierUser.Employee.LastName,
          ModifyDateTime = cargoItemLog.ModifyDateTime,
          NewCargoItemQty = Math.Round(cargoItemLog.NewCargoItemQty, cargoItemLog.CargoItem.Unit.DecimalDigitCount),
          OldCargoItemQty = Math.Round(cargoItemLog.OldCargoItemDetailQty, cargoItemLog.CargoItem.Unit.DecimalDigitCount),
          CargoItemLogStatus = cargoItemLog.CargoItemLogStatus,
          IsDelete = cargoItemLog.IsDelete,
          StuffId = cargoItemLog.CargoItem.PurchaseOrder.StuffId,
          StuffCode = cargoItemLog.CargoItem.PurchaseOrder.Stuff.Code,
          StuffName = cargoItemLog.CargoItem.PurchaseOrder.Stuff.Name,
          UnitId = cargoItemLog.CargoItem.UnitId,
          UnitName = cargoItemLog.CargoItem.Unit.Name,
          RowVersion = cargoItemLog.RowVersion
        };
    #endregion

    #region Search
    public IQueryable<CargoItemLogResult> SearchCargoItemLogResults(
        IQueryable<CargoItemLogResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
            i.ModifierUserFullName.Contains(searchText) ||
            i.CargoItemCode.Contains(searchText));
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<CargoItemLogResult> SortCargoItemLogResult(IQueryable<CargoItemLogResult> query, SortInput<CargoItemLogSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CargoItemLogSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}