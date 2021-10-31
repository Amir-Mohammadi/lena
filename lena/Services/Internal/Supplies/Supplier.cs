using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.Supplier;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Get
    public Supplier GetSupplier(int id) => GetSupplier(selector: e => e, id: id);
    public TResult GetSupplier<TResult>(
        Expression<Func<Supplier, TResult>> selector,
        int id)
    {

      var supplier = GetSuppliers(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (supplier == null)
        throw new SupplierNotFoundException(id);
      return supplier;
    }

    #endregion
    #region Gets
    public IQueryable<TResult> GetSuppliers<TResult>(
            Expression<Func<Supplier, TResult>> selector,
            TValue<int> id = null,
            TValue<int> employeeId = null,
            TValue<string> employeeCode = null,
            TValue<bool> isActive = null,
            TValue<string> description = null)
    {

      var query = repository.GetQuery<Supplier>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (employeeId != null)
        query = query.Where(x => x.Employee.Id == employeeId);
      if (employeeCode != null)
        query = query.Where(x => x.Employee.Code == employeeCode);
      if (isActive != null)
        query = query.Where(x => x.IsActive == isActive);
      if (description != null)
        query = query.Where(x => x.Description == description);
      return query.Select(selector);
    }


    #endregion
    #region Add
    public Supplier AddSupplier(
            int employeeId,
            string description)
    {

      var supplier = repository.Create<Supplier>();

      supplier.IsActive = true;
      supplier.Employee = App.Internals.UserManagement.GetEmployee(id: employeeId);
      supplier.Description = description;
      repository.Add(supplier);
      return supplier;
    }



    #endregion
    #region Edit
    public Supplier EditSupplier(
        int id,
        byte[] rowVersion,
        TValue<int> employeeId = null,
        TValue<bool> isActive = null,
        TValue<string> description = null)
    {

      var supplier = GetSupplier(id: id);
      return EditSupplier(
                supplier: supplier,
                rowVersion: rowVersion,
                 employeeId: employeeId,
                 isActive: isActive,
                 description: description
                );

    }

    public Supplier EditSupplier(
                Supplier supplier,
                byte[] rowVersion,
                TValue<int> employeeId = null,
                TValue<bool> isActive = null,
                TValue<string> description = null)
    {


      if (employeeId != null)
        supplier.Employee = App.Internals.UserManagement.GetEmployee(id: employeeId);
      if (isActive != null)
        supplier.IsActive = isActive;
      if (description != null)
        supplier.Description = description;
      repository.Update(rowVersion: rowVersion, entity: supplier);
      return supplier;
    }

    #endregion
    #region Active
    public Supplier ActiveSupplier(
        int id,
        byte[] rowVersion)
    {

      var supplier = GetSupplier(id: id);
      return ActiveSupplier(
                    supplier: supplier,
                    rowVersion: rowVersion
                );

    }

    public Supplier ActiveSupplier(
        Supplier supplier,
        byte[] rowVersion)
    {

      return EditSupplier(
                    supplier: supplier,
                    rowVersion: rowVersion,
                    isActive: true
                );
    }

    #endregion
    #region Deactive
    public Supplier DeactiveSupplier(
        int id,
        byte[] rowVersion)
    {

      var supplier = GetSupplier(id: id);
      return DeactiveSupplier(
                    supplier: supplier,
                    rowVersion: rowVersion
                );

    }

    public Supplier DeactiveSupplier(
        Supplier supplier,
        byte[] rowVersion)
    {

      return EditSupplier(
                    supplier: supplier,
                    rowVersion: rowVersion,
                    isActive: false
                );
    }

    #endregion
    #region Delete
    public void DeleteSupplier(int id)
    {

      var supplier = GetSupplier(id: id);
      repository.Delete(supplier);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<SupplierResult> SortSupplierResult(
        IQueryable<SupplierResult> query,
        SortInput<SupplierSortType> sort)
    {
      switch (sort.SortType)
      {
        case SupplierSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case SupplierSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case SupplierSortType.EmployeeCode:
          return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case SupplierSortType.IsActive:
          return query.OrderBy(a => a.IsActive, sort.SortOrder);
        case SupplierSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion
    #region Search
    public IQueryable<SupplierResult> SearchSupplierResult(
        IQueryable<SupplierResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.EmployeeFullName.Contains(searchText) ||
                    item.EmployeeCode.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }

    #endregion
    #region ToResult
    public Expression<Func<Supplier, SupplierResult>> ToSupplierResult =
                supplier => new SupplierResult
                {
                  Id = supplier.Id,
                  EmployeeId = supplier.Employee.Id,
                  EmployeeFullName = supplier.Employee.FirstName + " " + supplier.Employee.LastName,
                  EmployeeCode = supplier.Employee.Code,
                  IsActive = supplier.IsActive,
                  Description = supplier.Description,
                  RowVersion = supplier.RowVersion,

                };


    #endregion
  }
}
