using System;
using System.Linq;
using System.Linq.Expressions;
//using System.Runtime.Remoting.Channels;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
using lena.Services.Core.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.ApplicationBase.BaseEntityDocument;
using lena.Models.ApplicationBase.Unit;
using System.Collections.Generic;
using lena.Models.StaticData;
using lena.Models.SaleManagement.CustomerComplaint;
using lena.Models.SaleManagement.CustomerComplaintDepartment;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public CustomerComplaintDepartment AddCustomerComplaintDepartmentProcess(
        int customerComplaintSummaryId,
        short departmentId)
    {

      #region Check
      var customerComplaintDepartments = GetCustomerComplaintDepartments(
      selector: e => e,
      departmentId: departmentId,
      customerComplaintSummaryId: customerComplaintSummaryId)

          .FirstOrDefault();
      if (customerComplaintDepartments != null)
        throw new ExistCustomerComplaintDepartmentException(customerComplaintDepartments.DepartmentId);
      #endregion

      var customerComplaintDepartment = AddCustomerComplaintDepartment(
          customerComplaintSummaryId: customerComplaintSummaryId,
          departmentId: departmentId
         );
      return customerComplaintDepartment;
    }

    public CustomerComplaintDepartment AddCustomerComplaintDepartment(
        int customerComplaintSummaryId,
        short departmentId)
    {

      var customerComplaintDepartment = repository.Create<CustomerComplaintDepartment>();
      customerComplaintDepartment.CustomerComplaintSummaryId = customerComplaintSummaryId;
      customerComplaintDepartment.DepartmentId = departmentId;
      repository.Add(customerComplaintDepartment);
      return customerComplaintDepartment;
    }
    #endregion

    #region AddCustomerComplaintDepartments
    public CustomerComplaintDepartment AddCustomerComplaintDepartments(
        int customerComplaintSummaryId,
        short[] departmentIds)
    {


      var customerComplaintDepartment = repository.Create<CustomerComplaintDepartment>();
      foreach (var item in departmentIds)
      {
        customerComplaintDepartment = AddCustomerComplaintDepartmentProcess(
              customerComplaintSummaryId: customerComplaintSummaryId,
              departmentId: item
             );
      }
      return customerComplaintDepartment;
    }
    #endregion
    #region Get
    public CustomerComplaintDepartment GetCustomerComplaintDepartment(int id) => GetCustomerComplaintDepartment(selector: e => e, id: id);
    public TResult GetCustomerComplaintDepartment<TResult>(
        Expression<Func<CustomerComplaintDepartment, TResult>> selector,
        int id)
    {

      var customerComplaint = GetCustomerComplaintDepartments(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      return customerComplaint;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetCustomerComplaintDepartments<TResult>(
        Expression<Func<CustomerComplaintDepartment, TResult>> selector,
        TValue<int> id = null,
        TValue<int> customerComplaintSummaryId = null,
        TValue<int> departmentId = null
        )
    {

      var baseQuery = repository.GetQuery<CustomerComplaintDepartment>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (customerComplaintSummaryId != null)
        baseQuery = baseQuery.Where(i => i.CustomerComplaintSummaryId == customerComplaintSummaryId);
      if (departmentId != null)
        baseQuery = baseQuery.Where(i => i.DepartmentId == departmentId);
      return baseQuery.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<CustomerComplaintDepartment, CustomerComplaintDepartmentResult>> ToCustomerComplaintDepartmentResult =
        (customerComplaintDepartment) => new CustomerComplaintDepartmentResult
        {
          Id = customerComplaintDepartment.Id,
          DepartmentId = customerComplaintDepartment.DepartmentId,
          DepartmentName = customerComplaintDepartment.Department.Name,
          InhibitionAction = customerComplaintDepartment.InhibitionAction,
          CustomerComplaintSummaryId = customerComplaintDepartment.CustomerComplaintSummaryId,
          DateOfInhibition = customerComplaintDepartment.DateOfInhibition,
        };
    #endregion

    #region Edit
    public CustomerComplaintDepartment EditCustomerComplaintDepartment(
        int id,
        string inhibitionAction,
        DateTime? dateOfInhibition
    )
    {

      var customerComplaintDepartment = GetCustomerComplaintDepartment(id: id);
      if (inhibitionAction != null)
        customerComplaintDepartment.InhibitionAction = inhibitionAction;
      if (dateOfInhibition != null)
        customerComplaintDepartment.DateOfInhibition = dateOfInhibition;
      repository.Update(rowVersion: customerComplaintDepartment.RowVersion, entity: customerComplaintDepartment);
      return customerComplaintDepartment;
    }
    #endregion

    #region EditProcess
    public void EditCustomerComplaintDepartmentProcess(
        int customerComplaintSummaryId,
        short[] departmentIds)

    {

      var customerComplaintDepartments = GetCustomerComplaintDepartments(
                selector: e => e,
                customerComplaintSummaryId: customerComplaintSummaryId);

      foreach (var customerComplaintDepartment in customerComplaintDepartments)
      {
        DeleteCustomerComplaintDepartment(customerComplaintDepartment);
      }
      var addCustomerComplaintDepartment = AddCustomerComplaintDepartments(
                customerComplaintSummaryId: customerComplaintSummaryId,
                departmentIds: departmentIds
              );
    }
    #endregion
    #region Delete
    public void DeleteCustomerComplaintDepartment(CustomerComplaintDepartment customerComplaintDepartment)
    {

      repository.Delete(customerComplaintDepartment);
    }
    #endregion


    #region Search
    public IQueryable<CustomerComplaintDepartmentResult> SearchCustomerComplaintDepartmentResults(
        IQueryable<CustomerComplaintDepartmentResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
        )
    {

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region Sort
    public IQueryable<CustomerComplaintDepartmentResult> SortCustomerComplaintDepartmentResults(IQueryable<CustomerComplaintDepartmentResult> query,
        SortInput<CustomerComplaintDepartmentSortType> sortInput)
    {

      switch (sortInput.SortType)
      {
        case CustomerComplaintDepartmentSortType.DepartmentName:
          query = query.OrderBy(x => x.DepartmentName, sortInput.SortOrder);
          break;
        case CustomerComplaintDepartmentSortType.InhibitionAction:
          query = query.OrderBy(x => x.InhibitionAction, sortInput.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
      }

      return query;
    }
    #endregion
  }
}
