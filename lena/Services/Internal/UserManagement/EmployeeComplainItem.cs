using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.UserManagement.EmployeeComplain;
using lena.Models.UserManagement.EmployeeComplainDepartment;
using lena.Models.UserManagement.EmployeeComplainItem;
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
    public EmployeeComplainItem GetEmployeeComplainItem(int id) => GetEmployeeComplainItem(selector: e => e, id: id);
    public TResult GetEmployeeComplainItem<TResult>(Expression<Func<EmployeeComplainItem, TResult>> selector, int id)
    {

      var employeeComplainItem = GetEmployeeComplainItems(
                selector: selector,
                id: id)


            .FirstOrDefault();


      return employeeComplainItem;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetEmployeeComplainItems<TResult>(
       Expression<Func<EmployeeComplainItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int> employeeComplainId = null,
        TValue<string> description = null,
        TValue<EmployeeComplainType[]> employeeComplainType = null)

    {

      var query = repository.GetQuery<EmployeeComplainItem>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (employeeComplainId != null)
        query = query.Where(i => i.EmployeeComplainId == employeeComplainId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (employeeComplainType != null)
        query = query.Where(i => employeeComplainType.Value.Contains(i.Type));
      return query.Select(selector);
    }
    #endregion

    #region Add
    public EmployeeComplainItem AddEmployeeComplainItem(

        int employeeComplainId,
        EmployeeComplainType employeeComplainType,
        string description
        )
    {


      var userManagement = App.Internals.UserManagement;

      var employeeComplainItem = repository.Create<EmployeeComplainItem>();
      employeeComplainItem.EmployeeComplainId = employeeComplainId;
      employeeComplainItem.Type = employeeComplainType;
      employeeComplainItem.Description = description;
      repository.Add(employeeComplainItem);
      return employeeComplainItem;

    }
    #endregion

    #region AddProcess
    public void AddEmployeeComplainItemProcess(
        int employeeComplainId,
        EmployeeComplainType type,
        string description,
        AddEmployeeComplainDepartmentInput[] addEmployeeComplainDepartmentInputs)
    {


      foreach (var item in addEmployeeComplainDepartmentInputs)
      {
        var userManagement = App.Internals.UserManagement;

        var employeeComplainItem = AddEmployeeComplainItem(
                  employeeComplainId: employeeComplainId,
                  employeeComplainType: type,
                  description: description
                  );


        foreach (var departmentIds in item.DepartmentIds)
        {
          userManagement.AddEmployeeComplainDepartment(
                    employeeComplainItemId: employeeComplainItem.Id,
                    departmentId: departmentIds
                    );
        }
      }



    }
    #endregion




    #region ToResult

    public Expression<Func<EmployeeComplainItem, EmployeeComplainItemResult>> ToFullEmployeeComplainItemResult =
   (employeeComplainItem) => new EmployeeComplainItemResult
   {
     Id = employeeComplainItem.Id,
     Description = employeeComplainItem.Description,
     EmployeeComplainId = employeeComplainItem.EmployeeComplainId,
     Type = employeeComplainItem.Type,
     EmployeeComplainDepartments = employeeComplainItem.EmployeeComplainDepartments.AsQueryable().Select(App.Internals.UserManagement.ToFullEmployeeComplainDepartment),
     QAReviewEmployeeComplains = employeeComplainItem.QAReviewEmployeeComplains.AsQueryable().Select(App.Internals.UserManagement.ToQAReviewEmployeeComplain),
     RowVersion = employeeComplainItem.RowVersion
   };


    #endregion

    #region Edit
    public EmployeeComplainItem EditEmployeeComplainItem(
        int id,
        byte[] rowVersion,
        TValue<int> employeeComplainId,
        TValue<EmployeeComplainType> employeeComplainType,
        TValue<string> description)
    {

      var employeeComplainItem = GetEmployeeComplainItem(id: id);
      if (employeeComplainId != null)
        employeeComplainItem.EmployeeComplainId = employeeComplainId;
      if (employeeComplainType != null)
        employeeComplainItem.Type = employeeComplainType;
      if (description != null)
        employeeComplainItem.Description = description;
      repository.Update(rowVersion: employeeComplainItem.RowVersion, entity: employeeComplainItem);
      return employeeComplainItem;
    }
    #endregion

    #region Delete
    public void DeleteEmployeeComplainItemProcess(int id)
    {

      var employeeComplainItem = GetEmployeeComplainItem(id: id);


      var getEmployeeComplainItems = GetEmployeeComplainItems(
               selector: e => e,
               employeeComplainId: employeeComplainItem.EmployeeComplainId)


            .FirstOrDefault();

      var getEmployeeComplainDepartments = App.Internals.UserManagement.GetEmployeeComplainDepartments(
                selector: e => e,
                employeeComplainItemId: getEmployeeComplainItems.Id)


            .FirstOrDefault();

      var getResponsibleDepartments = App.Internals.UserManagement.GetResponsibleDepartments(selector: e => e,
                employeeComplainDepartmentId: getEmployeeComplainDepartments.Id,
                userId: App.Providers.Security.CurrentLoginData.UserId);


      if (getResponsibleDepartments.Any())
      {
        throw new EmployeeComplainItemHasOpinionException();
      }

      foreach (var item in employeeComplainItem.EmployeeComplainDepartments.ToList())
      {


        DeleteEmployeeComplainDepartment(employeeComplainItemId: employeeComplainItem.Id);
      }



      DeleteEmployeeComplainItem(employeeComplainItem.Id);

    }

    public void DeleteEmployeeComplainItem(int id)
    {

      var employeeComplainItem = GetEmployeeComplainItem(id: id);

      DeleteItem(employeeComplainItem);

    }

    public void DeleteItem(EmployeeComplainItem employeeComplainItem)
    {

      repository.Delete(employeeComplainItem);
    }
    #endregion


  }
}
