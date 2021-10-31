using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.UserManagement.EmployeeComplainDepartment;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Get
    public EmployeeComplainDepartment GetEmployeeComplainDepartment(int id) => GetEmployeeComplainDepartment(selector: e => e, id: id);
    public TResult GetEmployeeComplainDepartment<TResult>(Expression<Func<EmployeeComplainDepartment, TResult>> selector, int id)
    {
      var employeeComplainDepartment = GetEmployeeComplainDepartments(
                selector: selector,
                id: id)

            .FirstOrDefault();
      return employeeComplainDepartment;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetEmployeeComplainDepartments<TResult>(
       Expression<Func<EmployeeComplainDepartment, TResult>> selector,
        TValue<int> id = null,
        TValue<int> employeeComplainItemId = null,
        TValue<short> departmentId = null)
    {
      var query = repository.GetQuery<EmployeeComplainDepartment>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (employeeComplainItemId != null)
        query = query.Where(i => i.EmployeeComplainItem.Id == employeeComplainItemId);
      if (departmentId != null)
        query = query.Where(i => i.DepartmentId == departmentId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public EmployeeComplainDepartment AddEmployeeComplainDepartment(
        int employeeComplainItemId,
        short departmentId)
    {
      var employeeComplainDepartment = repository.Create<EmployeeComplainDepartment>();
      employeeComplainDepartment.EmployeeComplainItemId = employeeComplainItemId;
      employeeComplainDepartment.DepartmentId = departmentId;
      repository.Add(employeeComplainDepartment);
      return employeeComplainDepartment;
    }
    #endregion
    #region ToResult
    public Expression<Func<EmployeeComplainDepartment, EmployeeComplainDepartmentResult>> ToFullEmployeeComplainDepartment =
        employeeComplainDepartment => new EmployeeComplainDepartmentResult()
        {
          Id = employeeComplainDepartment.Id,
          EmployeeComplainItemId = employeeComplainDepartment.EmployeeComplainItemId,
          DepartmentId = employeeComplainDepartment.DepartmentId,
          DepartmentName = employeeComplainDepartment.Department.Name,
          ResponsibleDepartment = employeeComplainDepartment.ResponsibleDepartments.AsQueryable().Select(App.Internals.UserManagement.ToResponseDepartmentResult),
          RowVersion = employeeComplainDepartment.RowVersion
        };
    #endregion
    #region Edit
    public EmployeeComplainDepartment EditEmployeeComplainDepartment(
        int id,
        byte[] rowVersion,
        TValue<int> employeeComplainItemId,
        TValue<short> departmentId)
    {
      var complainDepartment = GetEmployeeComplainDepartment(id: id);
      if (employeeComplainItemId != null)
        complainDepartment.EmployeeComplainItemId = employeeComplainItemId;
      if (departmentId != null)
        complainDepartment.DepartmentId = departmentId;
      repository.Update(rowVersion: complainDepartment.RowVersion, entity: complainDepartment);
      return complainDepartment;
    }
    #endregion
    #region Delete
    public void DeleteEmployeeComplainDepartmentProcess(
        int employeeComplainItemId,
        int[] ids)
    {
      var employeeComplainItem = GetEmployeeComplainItem(id: employeeComplainItemId);
      foreach (var employeeComplainDepartmentId in ids)
      {
        var employeeComplainDepartment = GetEmployeeComplainDepartment(id: employeeComplainDepartmentId);
        var getEmployeeComplainDepartments = App.Internals.UserManagement.GetEmployeeComplainDepartments(selector: e => e, employeeComplainItemId: employeeComplainItemId).FirstOrDefault();
        var getResponsibleDepartments = App.Internals.UserManagement.GetResponsibleDepartments(
               selector: e => e,
               employeeComplainDepartmentId: getEmployeeComplainDepartments.Id,
               userId: App.Providers.Security.CurrentLoginData.UserId);
        if (getResponsibleDepartments.Any())
        {
          throw new EmployeeComplainDepartmentHasOpinionException();
        }
        DeleteEmployeeComplainDepartment(employeeComplainDepartment: employeeComplainDepartment);
      }
    }
    public void DeleteEmployeeComplainDepartment(int employeeComplainItemId)
    {
      var employeeComplainItem = GetEmployeeComplainItem(id: employeeComplainItemId);
      foreach (var employeeComplainDepartment in employeeComplainItem.EmployeeComplainDepartments.ToList())
      {
        DeleteEmployeeComplainDepartment(employeeComplainDepartment: employeeComplainDepartment);
      }
    }
    public void DeleteEmployeeComplainDepartment(EmployeeComplainDepartment employeeComplainDepartment)
    {
      repository.Delete(employeeComplainDepartment);
    }
    #endregion
  }
}