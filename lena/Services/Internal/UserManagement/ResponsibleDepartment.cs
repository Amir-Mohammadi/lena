using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using System;
using lena.Models.UserManagement.ResponseDepartment;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using System.Linq.Expressions;
using System.Linq;
using lena.Services.Core;
using lena.Services.Internals.UserManagement.Exception;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Get
    public ResponsibleDepartment GetResponsibleDepartment(int id) => GetResponsibleDepartment(selector: e => e, id: id);
    public TResult GetResponsibleDepartment<TResult>(Expression<Func<ResponsibleDepartment, TResult>> selector, int id)
    {

      var responsibleDepartment = GetResponsibleDepartments(
                selector: selector,
                id: id)


            .FirstOrDefault();


      return responsibleDepartment;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetResponsibleDepartments<TResult>(
       Expression<Func<ResponsibleDepartment, TResult>> selector,
        TValue<int> id = null,
        TValue<int> employeeComplainDepartmentId = null,
        TValue<int> userId = null,
        TValue<string> opinion = null)

    {

      var query = repository.GetQuery<ResponsibleDepartment>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (employeeComplainDepartmentId != null)
        query = query.Where(i => i.EmployeeComplainDepartmentId == employeeComplainDepartmentId);
      if (opinion != null)
        query = query.Where(i => i.Opinion == opinion);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);

      return query.Select(selector);
    }
    #endregion

    #region delete 
    public void DeleteResponsibleDepartmentProcess(int id)
    {

      var userManagement = App.Internals.UserManagement;
      var responseDepartment = userManagement.GetResponsibleDepartment(id: id);
      repository.Delete(responseDepartment);
    }
    #endregion

    public ResponsibleDepartment AddResponsibleDepartment(
         int employeeComplainDepartmentId,
        string opinion)
    {


      var userManagement = App.Internals.UserManagement;
      var responsibleDepartment = repository.Create<ResponsibleDepartment>();
      responsibleDepartment.EmployeeComplainDepartmentId = employeeComplainDepartmentId;
      responsibleDepartment.UserId = App.Providers.Security.CurrentLoginData.UserId;
      responsibleDepartment.Opinion = opinion;
      repository.Add(responsibleDepartment);
      return responsibleDepartment;

    }

    #region Add
    public ResponsibleDepartment AddResponsibleDepartmentProcess(
        int employeeComplainDepartmentId,
        int responseDepartment,
        string opinion)
    {



      var getResponsibleDepartments = GetResponsibleDepartments(selector: e => e, employeeComplainDepartmentId: employeeComplainDepartmentId, userId: App.Providers.Security.CurrentLoginData.UserId);

      if (getResponsibleDepartments.Any())
      {
        throw new ResponsibleDepartmentOpinionHasAddedException(employeeComplainDepartmentId);
      }

      return AddResponsibleDepartment(
                employeeComplainDepartmentId: employeeComplainDepartmentId,
                opinion: opinion);



    }
    #endregion

    #region Edit
    public ResponsibleDepartment EditResponsibleDepartment(
        int id,
        byte[] rowVersion,
        TValue<int> employeeComplainDepartmentId = null,
        TValue<string> opinion = null)
    {

      var responsible = GetResponsibleDepartment(id: id);
      if (employeeComplainDepartmentId != null)
        responsible.EmployeeComplainDepartmentId = employeeComplainDepartmentId;
      if (opinion != null)
        responsible.Opinion = opinion;
      repository.Update(responsible, rowVersion);
      return responsible;
    }
    #endregion

    #region ToResult
    public Expression<Func<ResponsibleDepartment, ResponsibleDepartmentResult>> ToResponseDepartmentResult =
        responseDepartment => new ResponsibleDepartmentResult()
        {
          Id = responseDepartment.Id,
          DepartmentId = responseDepartment.EmployeeComplainDepartment.DepartmentId,
          DepartmentName = responseDepartment.EmployeeComplainDepartment.Department.Name,
          UserId = App.Providers.Security.CurrentLoginData.UserId,
          UserFullName = App.Providers.Security.CurrentLoginData.UserFirstName + " " + App.Providers.Security.CurrentLoginData.UserLastName,
          Opinion = responseDepartment.Opinion,
          EmployeeComplainDepartmentId = responseDepartment.EmployeeComplainDepartment.Id,
          RowVersion = responseDepartment.RowVersion

        };

    #endregion

    #region sort
    public IOrderedQueryable<ResponsibleDepartmentResult> SortResponseDepartmentResult(IQueryable<ResponsibleDepartmentResult> query,
      SortInput<ResponsibleDepartmentSortType> sort)
    {
      switch (sort.SortType)
      {
        case ResponsibleDepartmentSortType.Id:
          return query.OrderBy(i => i.Id, sort.SortOrder);
        case ResponsibleDepartmentSortType.DepartmentId:
          return query.OrderBy(i => i.DepartmentId, sort.SortOrder);
        default:
          return null;
      }
    }
    #endregion

    #region search
    public IQueryable<ResponsibleDepartmentResult> SearchResponsibleDepartment(IQueryable<ResponsibleDepartmentResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.DepartmentName.Contains(searchText) ||
            item.Opinion.Contains(searchText)
            );
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

  }
}

