//using lena.Services.Core.Foundation.Service.Internal.Action;
using System;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;
using lena.Models.UserManagement.EmployeeComplain;
using lena.Models.UserManagement.EmployeeComplainDepartment;
using lena.Models.UserManagement.EmployeeComplainItem;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using lena.Domains.Enums;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.StaticData;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {

    #region Get
    public EmployeeComplain GetEmployeeComplain(int id) => GetEmployeeComplain(selector: e => e, id: id);
    public TResult GetEmployeeComplain<TResult>(Expression<Func<EmployeeComplain, TResult>> selector, int id)
    {

      var emploeeComplain = GetEmployeeComplaints(
                selector: selector,
                id: id)


            .FirstOrDefault();

      return emploeeComplain;
    }
    #endregion

    #region GetFull
    public EmployeeComplainResult GetFullEmployeeComplain(
   int id)
    {

      var userManagement = App.Internals.UserManagement;
      var employeeComplain = GetEmployeeComplain(
                selector: App.Internals.UserManagement.ToFullEmployeeComplainResult,
                id: id);

      return employeeComplain;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetEmployeeComplaints<TResult>(
        Expression<Func<EmployeeComplain, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userId = null,
        TValue<DateTime> dateTime = null,
        TValue<int> employeeId = null,
        TValue<EmployeeComplainItem[]> employeeComplainItems = null)
    {

      var query = repository.GetQuery<EmployeeComplain>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      if (employeeId != null)
        query = query.Where(i => i.EmployeeId == employeeId);

      return query.Select(selector);
    }
    #endregion


    #region GetProcess
    public IQueryable<TResult> GetEmployeeComplaintProcess<TResult>(
        Expression<Func<EmployeeComplain, TResult>> selector,
        TValue<int> id = null,
          TValue<int> userId = null,
          TValue<DateTime> dateTime = null,
          TValue<int> employeeId = null,
          TValue<EmployeeComplainItem[]> employeeComplainItems = null)
    {

      var employeeComplainShowPermission = App.Internals.UserManagement.CheckPermission(
                   actionName: StaticActionName.EmployeeComplainShow,
                   actionParameters: null);

      var membershipId = App.Internals.UserManagement.GetMemberships(
                 selector: e => e,
                 userId: App.Providers.Security.CurrentLoginData.UserId);

      if (employeeComplainShowPermission.AccessType == AccessType.Allowed && membershipId.Any())
      {
        return GetEmployeeComplaints(
                   selector: selector);
      }
      else
      {
        return GetEmployeeComplaints(
                  selector: selector,
                 id: id,
                 userId: App.Providers.Security.CurrentLoginData.UserId);
      }
    }
    #endregion
    #region AddEmployeeComplain

    public EmployeeComplain AddEmployeeComplain(

        int employeeId,
        DateTime dateTime)
    {

      var employeeComplain = repository.Create<EmployeeComplain>();
      employeeComplain.UserId = App.Providers.Security.CurrentLoginData.UserId;
      employeeComplain.EmployeeId = employeeId;
      employeeComplain.DateTime = dateTime;
      repository.Add(employeeComplain);
      return employeeComplain;

    }
    #endregion

    #region AddProcess
    public void AddEmployeeComplainProcess(
        int userId,
        int employeeId,
        DateTime dateTime,
        AddEmployeeComplainItemInput[] employeeComplainItems,
        DeleteEmployeeComplainItemInput[] deleteEmployeeComplainItemInputs,
        DeleteEmployeeComplainDepartmentInput[] deleteEmployeeComplainDepartmentInputs
       )
    {

      var userManagement = App.Internals.UserManagement;

      var employeeComplain = AddEmployeeComplain(

                employeeId: employeeId,
                dateTime: dateTime);


      foreach (var item in employeeComplainItems)
      {
        userManagement.AddEmployeeComplainItemProcess(
                  employeeComplainId: employeeComplain.Id,
                  type: item.Type,
                  description: item.Description,
                  addEmployeeComplainDepartmentInputs: item.AddEmployeeComplainDepartmentInput);
      }


      foreach (var item in deleteEmployeeComplainItemInputs.ToList())
      {
        userManagement.DeleteEmployeeComplainItemProcess(
                  id: item.Id);

      }




      foreach (var item in deleteEmployeeComplainDepartmentInputs)
      {

        userManagement.DeleteEmployeeComplainDepartmentProcess(
                  employeeComplainItemId: (int)item.EmployeeComplainItemId,
                  ids: item.Ids);

      }


    }
    #endregion

    #region Edit
    public EmployeeComplain EditEmployeeComplain(
        int id,
        byte[] rowVersion,
        TValue<int> employeeId,
        TValue<DateTime> dateTime)
    {

      var employeeComplain = GetEmployeeComplain(id: id);
      if (employeeId != null)
        employeeComplain.EmployeeId = employeeId;
      if (dateTime != null)
        employeeComplain.DateTime = dateTime;
      repository.Update(rowVersion: employeeComplain.RowVersion, entity: employeeComplain);
      return employeeComplain;

    }
    #endregion

    #region EditProcess
    public void EditEmployeeComplainProcess(
        int id,
        int employeeId,
        DateTime dateTime,
        byte[] rowVersion,
        AddEmployeeComplainItemInput[] addEmployeeComplainItemInputs,
        DeleteEmployeeComplainItemInput[] deleteEmployeeComplainItemInputs,
        DeleteEmployeeComplainDepartmentInput[] deleteEmployeeComplainDepartmentInputs
        )
    {

      var userManagement = App.Internals.UserManagement;



      foreach (var item in addEmployeeComplainItemInputs)
      {
        userManagement.AddEmployeeComplainItemProcess(
                  employeeComplainId: item.EmployeeComplainId,
                  type: item.Type,
                  description: item.Description,
                  addEmployeeComplainDepartmentInputs: item.AddEmployeeComplainDepartmentInput);
      }


      foreach (var item in deleteEmployeeComplainItemInputs.ToList())
      {

        userManagement.DeleteEmployeeComplainItemProcess(
                  id: item.Id);

      }




      foreach (var item in deleteEmployeeComplainDepartmentInputs)
      {


        userManagement.DeleteEmployeeComplainDepartmentProcess(
                  employeeComplainItemId: (int)item.EmployeeComplainItemId,
                  ids: item.Ids);

      }



      var employeeComplain = EditEmployeeComplain(
                id: id,
                employeeId: employeeId,
                dateTime: dateTime,
                rowVersion: rowVersion);





    }
    #endregion

    #region ToFullResult
    public Expression<Func<EmployeeComplain, EmployeeComplainResult>> ToFullEmployeeComplainResult =
         employeeComplain => new EmployeeComplainResult
         {


           Id = employeeComplain.Id,
           UserId = employeeComplain.User.Id,
           UserFullName = employeeComplain.User.Employee.FirstName + " " + employeeComplain.User.Employee.LastName,
           EmployeeId = employeeComplain.Employee.Id,
           EmployeeFullName = employeeComplain.Employee.FirstName + " " + employeeComplain.Employee.LastName,
           EmployeeCode = employeeComplain.Employee.Code,
           DateTime = employeeComplain.DateTime,
           EmployeeComplainItems = employeeComplain.EmployeeComplainItems.AsQueryable().Select(App.Internals.UserManagement.ToFullEmployeeComplainItemResult),
           RowVersion = employeeComplain.RowVersion,
           DepartmentId = employeeComplain.Employee.Department.Id,
           DepartmentName = employeeComplain.Employee.Department.Name,
           DepartmentCount = employeeComplain.EmployeeComplainItems.SelectMany(i => i.EmployeeComplainDepartments).Count(),


         };

    #endregion

    #region Sort
    public IOrderedQueryable<EmployeeComplainResult> SortEmployeeComplainResult(IQueryable<EmployeeComplainResult> query,
        SortInput<EmployeeComplainSortType> sort)
    {
      switch (sort.SortType)
      {
        case EmployeeComplainSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case EmployeeComplainSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);

        default:
          return null;
      }
    }
    #endregion

    #region Search
    public IQueryable<EmployeeComplainResult> SearchEmployeeComplain(IQueryable<EmployeeComplainResult>
    query,
    TValue<int> employeeId,
    TValue<DateTime> dateTime,
    string searchText,
     AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
            item.EmployeeFullName.Contains(searchText) ||
            item.EmployeeCode.Contains(searchText) ||
            item.UserFullName.Contains(searchText) ||
            item.DepartmentName.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      if (employeeId != null)
        query = query.Where(i => i.EmployeeId == employeeId);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);




      return query;
    }
    #endregion


    #region delete
    public void DeleteEmployeeComplain(int id)
    {

      var deleteEmployeeComplain = GetEmployeeComplain(id: id);
      repository.Delete(deleteEmployeeComplain);
    }
    #endregion
  }
}
