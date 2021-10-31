using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.Supplier;
using lena.Models.Supplies.SuppliesPurchaserUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    public SuppliesPurchaserUser GetSuppliesPurchaserUser(int id) => GetSuppliesPurchaserUser(selector: e => e, id: id);
    public TResult GetSuppliesPurchaserUser<TResult>(
       Expression<Func<SuppliesPurchaserUser, TResult>> selector,
       int id)
    {

      var supplier = GetSuppliesPurchaserUsers(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (supplier == null)
        throw new SuppliesPurchaserUserNotFoundException(id);
      return supplier;
    }

    public IQueryable<TResult> GetSuppliesPurchaserUsers<TResult>(
             Expression<Func<SuppliesPurchaserUser, TResult>> selector,
             TValue<int> id = null,
             TValue<int> purchaserUserId = null,
             TValue<bool> isDefault = null,
             TValue<string> stuffCode = null,
             TValue<int> stuffId = null,
             TValue<int[]> stuffIds = null)
    {

      var query = repository.GetQuery<SuppliesPurchaserUser>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (purchaserUserId != null)
        query = query.Where(x => x.PurchaserUserId == purchaserUserId);
      if (isDefault != null)
        query = query.Where(x => x.IsDefault == isDefault);
      if (stuffCode != null)
        query = query.Where(x => x.Stuff.Code == stuffCode);
      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);
      if (stuffIds != null)
        query = query.Where(x => stuffIds.Value.Contains(x.StuffId));

      return query.Select(selector);
    }

    public SuppliesPurchaserUser AddSuppliesPurchaserUser(
       int userId,
       int stuffId,
       bool isDefault,
       string description
           )
    {

      var entity = repository.Create<SuppliesPurchaserUser>();
      entity.PurchaserUserId = userId;
      entity.StuffId = stuffId;
      entity.IsDefault = isDefault;
      entity.Description = description;
      entity.DateTime = DateTime.UtcNow;

      repository.Add(entity);
      return entity;
    }

    public void AddSuppliesPurchaserUsersProcess(
       AddSuppliesPurchaserUsersInput addSuppliesPurchaserUsersInput)
    {

      var currentSuppliesPurchaserUsers = GetSuppliesPurchaserUsers(
                      selector: e => e,
                      stuffIds: addSuppliesPurchaserUsersInput.StuffIds
                      )


                      .ToList();

      foreach (var item in currentSuppliesPurchaserUsers)
      {
        EditSuppliesPurchaserUser(
                  suppliesPurchaserUser: item,
                  rowVersion: item.RowVersion,
                    isDefault: false);
      }


      foreach (var stuffId in addSuppliesPurchaserUsersInput.StuffIds)
      {
        foreach (var item in addSuppliesPurchaserUsersInput.Details)
        {
          var suppliesPurchaserUser = currentSuppliesPurchaserUsers.
                FirstOrDefault(i => i.StuffId == stuffId && i.PurchaserUserId == item.UserId);
          if (suppliesPurchaserUser != null)
          {
            EditSuppliesPurchaserUser(
                suppliesPurchaserUser: suppliesPurchaserUser,
                rowVersion: suppliesPurchaserUser.RowVersion,
                  isDefault: item.IsDefault,
                  description: item.Description
                  );
          }
          else
          {
            AddSuppliesPurchaserUser(
                      userId: item.UserId,
                      stuffId: stuffId,
                      isDefault: item.IsDefault,
                      description: item.Description);
          }
        }
      }
    }

    public SuppliesPurchaserUser EditSuppliesPurchaserUser(
        int id,
        byte[] rowVersion,
        TValue<int> purchaserUserId = null,
        TValue<int> stuffId = null,
        TValue<string> description = null,
        TValue<bool> isDefault = null)
    {

      var suppliesPurchaserUser = GetSuppliesPurchaserUser(id: id);
      EditSuppliesPurchaserUser(
                suppliesPurchaserUser: suppliesPurchaserUser,
                rowVersion: suppliesPurchaserUser.RowVersion,
                purchaserUserId: purchaserUserId,
                stuffId: stuffId,
                description: description,
                isDefault: isDefault
                );

      return suppliesPurchaserUser;
    }


    public SuppliesPurchaserUser EditSuppliesPurchaserUser(
      SuppliesPurchaserUser suppliesPurchaserUser,
      byte[] rowVersion,
      TValue<int> purchaserUserId = null,
      TValue<int> stuffId = null,
      TValue<string> description = null,
      TValue<bool> isDefault = null)
    {

      if (isDefault != null && isDefault && suppliesPurchaserUser.IsDefault)
        throw new IsDefaultSuppliesPurchaserUserException(stuffId: stuffId);

      // Prevent the stuff being default when it is in default state.
      if (isDefault)
      {
        var purchaserUser = GetSuppliesPurchaserUsers(
                  selector: e => e,
                  stuffId: stuffId,
                  isDefault: true)

                  .FirstOrDefault();

        if (purchaserUser != null)
          throw new SuppliesPurchaserUserException(stuffId: stuffId);
      }

      if (purchaserUserId != null)
        suppliesPurchaserUser.PurchaserUserId = purchaserUserId;
      if (stuffId != null)
        suppliesPurchaserUser.StuffId = stuffId;
      if (description != null)
        suppliesPurchaserUser.Description = description;
      if (isDefault != null)
        suppliesPurchaserUser.IsDefault = isDefault;
      repository.Update(suppliesPurchaserUser, rowVersion);
      return suppliesPurchaserUser;
    }

    #region Delete
    public void DeleteSuppliesPurchaserUser(int id)
    {

      var suppliesPurchaserUser = GetSuppliesPurchaserUser(id: id);
      repository.Delete(suppliesPurchaserUser);
    }
    #endregion

    public IOrderedQueryable<SuppliesPurchaserUserResult> SortSuppliesPurchaserUserResult(
      IQueryable<SuppliesPurchaserUserResult> query,
      SortInput<SuppliesPurchaserUserSortType> sort)
    {
      switch (sort.SortType)
      {
        case SuppliesPurchaserUserSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case SuppliesPurchaserUserSortType.CreateDate:
          return query.OrderBy(a => a.CreateDate, sort.SortOrder);
        case SuppliesPurchaserUserSortType.EmployeeCode:
          return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case SuppliesPurchaserUserSortType.EmployeeName:
          return query.OrderBy(a => a.EmployeeName, sort.SortOrder);
        case SuppliesPurchaserUserSortType.IsDefault:
          return query.OrderBy(a => a.IsDefault, sort.SortOrder);
        case SuppliesPurchaserUserSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case SuppliesPurchaserUserSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case SuppliesPurchaserUserSortType.StuffEnName:
          return query.OrderBy(a => a.StuffEnName, sort.SortOrder);
        case SuppliesPurchaserUserSortType.UserId:
          return query.OrderBy(a => a.UserId, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<SuppliesPurchaserUserResult> SearchSuppliesPurchaserUserResult(
      IQueryable<SuppliesPurchaserUserResult> query,
      string searchText,
      AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.EmployeeName.Contains(searchText) ||
                    item.StuffCode.Contains(searchText) ||
                    item.StuffName.Contains(searchText) ||
                    item.StuffEnName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }

    public Expression<Func<SuppliesPurchaserUser, SuppliesPurchaserUserResult>> ToSuppliesPurchaserUserResult =
      s => new SuppliesPurchaserUserResult()
      {
        Id = s.Id,
        UserId = s.PurchaserUserId,
        EmployeeCode = s.PurchaserUser.Employee.Code,
        EmployeeName = s.PurchaserUser.Employee.FirstName + " " + s.PurchaserUser.Employee.LastName,
        CreateDate = s.DateTime,
        IsDefault = s.IsDefault,
        RowVersion = s.RowVersion,
        StuffCode = s.Stuff.Code,
        StuffId = s.StuffId,
        StuffName = s.Stuff.Noun,
        StuffEnName = s.Stuff.Name,
        Description = s.Description
      };

    public User GetDefaultSuppliesPurchaserUser(
     TValue<int> stuffId = null)
    {

      var suppliesPurchaserUsers = GetSuppliesPurchaserUsers(
                     selector: e => e,
                     stuffId: stuffId,
                     isDefault: true);
      var first = suppliesPurchaserUsers.OrderByDescending(r => r.Id).FirstOrDefault();
      return first?.PurchaserUser;
    }

    public SuppliesPurchaserUser IsDefaultSuppliesPurchaserUser(byte[] rowVersion, int stuffId, int purchaserUserId, int id)
    {
      return EditSuppliesPurchaserUser(
          id: id,
          stuffId: stuffId,
          rowVersion: rowVersion,
          purchaserUserId: purchaserUserId,
          isDefault: true);
    }

    public SuppliesPurchaserUser UnDefaultSuppliesPurchaserUser(byte[] rowVersion, int stuffId, int purchaserUserId, int id)
    {
      return EditSuppliesPurchaserUser(
          id: id,
          stuffId: stuffId,
          rowVersion: rowVersion,
          purchaserUserId: purchaserUserId,
          isDefault: false);
    }
  }
}
