using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting.CostCenter;
using lena.Models.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    public CostCenter AddCostCenter(
        string description,
        string name)
    {

      var costCenter = repository.Create<CostCenter>();
      costCenter.Name = name;
      costCenter.Status = CostCenterStatus.NotAction;
      costCenter.Description = description;
      repository.Add(costCenter);
      return costCenter;
    }
    #endregion
    #region Edit
    public CostCenter EditCostCenterProcess(
     int id,
     byte[] rowVersion,
     //CostCenter costCenter,
     TValue<string> name = null,
     TValue<CostCenterStatus> status = null,
     TValue<int> confirmerUserId = null,
     TValue<DateTime> confirmDateTime = null,
     TValue<string> description = null)
    {

      var costCenter = GetCostCenter(
                id: id);
      if (costCenter.Status != CostCenterStatus.NotAction)
        throw new CostCenterCanNotEdit(id: costCenter.Id);
      return EditCostCenter(
                id: id,
                name: name,
                description: description,
                rowVersion: rowVersion);
    }
    public CostCenter EditCostCenter(
        int id,
        byte[] rowVersion,
        //CostCenter costCenter,
        TValue<string> name = null,
        TValue<CostCenterStatus> status = null,
        TValue<int> confirmerUserId = null,
        TValue<DateTime> confirmDateTime = null,
        TValue<string> description = null)
    {

      var costCenter = GetCostCenter(
                id: id);
      if (name != null)
        costCenter.Name = name;
      if (status != null)
        costCenter.Status = status;
      if (description != null)
        costCenter.Description = description;
      if (confirmerUserId != null)
        costCenter.ConfirmerUserId = confirmerUserId;
      if (confirmDateTime != null)
        costCenter.ConfirmDateTime = confirmDateTime;
      repository.Update(entity: costCenter, rowVersion: costCenter.RowVersion);
      return costCenter;
    }
    #endregion
    #region Delete 
    public void DeleteCostCenter(int id)
    {

      var costCenter = GetCostCenter(id: id);
      if (costCenter.Status != CostCenterStatus.NotAction)
        throw new CanNotDeleteCostCenterException(id: costCenter.Id);
      DeleteCostCenter(costCenter: costCenter);
    }
    public void DeleteCostCenter(CostCenter costCenter)
    {

      repository.Delete(costCenter);
    }
    #endregion
    #region Get
    public CostCenter GetCostCenter(int id) => GetCostCenter(selector: e => e, id: id);
    public TResult GetCostCenter<TResult>(
        Expression<Func<CostCenter, TResult>> selector,
        int id)
    {

      var costCenter = GetCostCenters(selector: selector,
                id: id)

            .FirstOrDefault();
      if (costCenter == null)
        throw new RecordNotFoundException(id, typeof(CostCenter));
      return costCenter;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCostCenters<TResult>(
        Expression<Func<CostCenter, TResult>> selector,
        TValue<int> id = null,
        TValue<CostCenterStatus> status = null,
        TValue<string> name = null)
    {

      var costCenter = repository.GetQuery<CostCenter>();
      if (id != null)
        costCenter = costCenter.Where(i => i.Id == id);
      if (name != null)
        costCenter = costCenter.Where(i => i.Name == name);
      if (status != null)
        costCenter = costCenter.Where(i => i.Status == status);
      return costCenter.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<CostCenter, CostCenterResult>> ToCostCenterResult =
        costCenter => new CostCenterResult
        {
          Id = costCenter.Id,
          Name = costCenter.Name,
          Status = costCenter.Status,
          ConfirmDateTime = costCenter.ConfirmDateTime,
          ConfirmerUserId = costCenter.ConfirmerUserId,
          ConfirmerEmployeeFullName = costCenter.ConfirmerUser.Employee.FirstName + " " + costCenter.ConfirmerUser.Employee.LastName,
          Description = costCenter.Description,
          RowVersion = costCenter.RowVersion
        };
    public Expression<Func<CostCenter, CostCenterComboResult>> ToCostCenterComboResult =
        costCenter => new CostCenterComboResult
        {
          Id = costCenter.Id,
          Name = costCenter.Name,
          Status = costCenter.Status,
          Description = costCenter.Description,
          RowVersion = costCenter.RowVersion
        };
    #endregion
    #region Sort
    public IOrderedQueryable<CostCenterResult> SortCostCenterResult(
      IQueryable<CostCenterResult> query,
      SortInput<CostCenterSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CostCenterSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case CostCenterSortType.Name:
          return query.OrderBy(i => i.Name, sortInput.SortOrder);
        case CostCenterSortType.Description:
          return query.OrderBy(i => i.Description, sortInput.SortOrder);
        case CostCenterSortType.ConfirmerUserId:
          return query.OrderBy(i => i.ConfirmerUserId, sortInput.SortOrder);
        case CostCenterSortType.ConfirmDateTime:
          return query.OrderBy(i => i.ConfirmDateTime, sortInput.SortOrder);
        case CostCenterSortType.Status:
          return query.OrderBy(i => i.Status, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<CostCenterResult> SearchCostCenterResult(
         IQueryable<CostCenterResult> query,
         string searchText,
         AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Name.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region AcceptCostCenters
    public CostCenter AcceptCostCenter(
      int id,
      byte[] rowVersion)
    {

      var costCenter = GetCostCenter(id: id);
      costCenter = EditCostCenter(
                id: id,
                confirmerUserId: App.Providers.Security.CurrentLoginData.UserId,
                confirmDateTime: DateTime.UtcNow,
                rowVersion: rowVersion,
                status: CostCenterStatus.Accepted);
      return costCenter;
    }
    #endregion
    #region AcceptCostCenters
    public CostCenter RejectCostCenter(
      int id,
      byte[] rowVersion)
    {

      var costCenter = GetCostCenter(id: id);
      costCenter = EditCostCenter(
                id: id,
                confirmerUserId: App.Providers.Security.CurrentLoginData.UserId,
                confirmDateTime: DateTime.UtcNow,
                rowVersion: rowVersion,
                status: CostCenterStatus.Rejected);
      return costCenter;
    }
    #endregion
  }
}