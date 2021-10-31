using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPResponsibleEmployee;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement
{
  public partial class ProjectManagement
  {

    #region Get
    public ProjectERPResponsibleEmployee GetProjectERPResponsibleEmployee(int responsibleEmployeeId, int? projectERPId) =>
        GetProjectERPResponsibleEmployee(
            selector: e => e,
            responsibleEmployeeId: responsibleEmployeeId,
            projectERPId: projectERPId);

    public TResult GetProjectERPResponsibleEmployee<TResult>(
        Expression<Func<ProjectERPResponsibleEmployee, TResult>> selector,
        int? projectERPId,
        int responsibleEmployeeId)
    {

      var projectERPResponsibleEmployee = GetProjectERPResponsibleEmployeees(selector: selector,
                projectERPId: projectERPId,
                responsibleEmployeeId: responsibleEmployeeId)

            .FirstOrDefault();
      if (projectERPResponsibleEmployee == null)
        throw new ProjectERPResponsibleEmployeeNotFoundException(projectERPId: projectERPId ?? 0);
      return projectERPResponsibleEmployee;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPResponsibleEmployeees<TResult>(
        Expression<Func<ProjectERPResponsibleEmployee, TResult>> selector,
        TValue<int> projectERPId = null,
        TValue<int> responsibleEmployeeId = null,
        TValue<bool> isActive = null,
        TValue<string> color = null,
        TValue<string> description = null)
    {

      var query = repository.GetQuery<ProjectERPResponsibleEmployee>();
      if (projectERPId != null)
        query = query.Where(m => m.ProjectERPId == projectERPId);
      if (isActive != null)
        query = query.Where(m => m.IsActive == isActive);
      if (responsibleEmployeeId != null)
        query = query.Where(m => m.ResponsibleEmployeeId == responsibleEmployeeId);
      if (description != null)
        query = query.Where(m => m.Description == description);

      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPResponsibleEmployee
    public ProjectERPResponsibleEmployee AddProjectERPResponsibleEmployee(
        int projectERPId,
        int responsibleEmployeeId,
        bool isActive,
        string description)
    {

      var projectERPResponsibleEmployee = repository.Create<ProjectERPResponsibleEmployee>();
      projectERPResponsibleEmployee.ProjectERPId = projectERPId;
      projectERPResponsibleEmployee.IsActive = isActive;
      projectERPResponsibleEmployee.ResponsibleEmployeeId = responsibleEmployeeId;
      projectERPResponsibleEmployee.CreatorDateTime = DateTime.UtcNow;
      projectERPResponsibleEmployee.Description = description;

      repository.Add(projectERPResponsibleEmployee);
      return projectERPResponsibleEmployee;
    }

    #endregion

    #region EditProjectERPResponsibleEmployee
    //public ProjectERPResponsibleEmployee EditProjectERPResponsibleEmployee(
    //    int projectERPId,
    //    byte[] rowVersion,
    //    TValue<bool> isActive = null,
    //    TValue<int> responsibleUserId = null,
    //    TValue<string> description = null)
    //{
    //    
    //        var projectERPResponsibleEmployee = GetProjectERPResponsibleEmployee(responsibleUserId: responsibleUserId, projectERPId: projectERPId);

    //        if (isActive != null)
    //            projectERPResponsibleEmployee.IsActive = isActive;
    //        if (responsibleUserId != null)
    //            projectERPResponsibleEmployee.ResponsibleUserId = responsibleUserId;
    //        if (description != null)
    //            projectERPResponsibleEmployee.Description = description;

    //        repository.Update(entity: projectERPResponsibleEmployee, rowVersion: projectERPResponsibleEmployee.RowVersion);
    //        return projectERPResponsibleEmployee;
    //    });
    //}
    #endregion

    #region DeleteProjectERPResponsibleEmployee
    public void DeleteProjectERPResponsibleEmployee(
        int responsibleEmployeeId,
        int projectERPId,
        byte[] rowVersion)
    {

      var projectERPResponsibleEmployee = GetProjectERPResponsibleEmployee(responsibleEmployeeId: responsibleEmployeeId,
                                                                         projectERPId: projectERPId);
      repository.Delete(projectERPResponsibleEmployee);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPResponsibleEmployeeResult> SortProjectERPResponsibleEmployeeResult(
        IQueryable<ProjectERPResponsibleEmployeeResult> query, SortInput<ProjectERPResponsibleEmployeeSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPResponsibleEmployeeSortType.ProjectERPTitle:
          return query.OrderBy(a => a.ProjectERPTitle, type.SortOrder);
        case ProjectERPResponsibleEmployeeSortType.CreatorDateTime:
          return query.OrderBy(a => a.CreatorDateTime, type.SortOrder);
        case ProjectERPResponsibleEmployeeSortType.ResponsibleEmployeeFullName:
          return query.OrderBy(a => a.ResponsibleEmployeeFullName, type.SortOrder);
        case ProjectERPResponsibleEmployeeSortType.IsActive:
          return query.OrderBy(a => a.IsActive, type.SortOrder);
        case ProjectERPResponsibleEmployeeSortType.Description:
          return query.OrderBy(a => a.Description, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPResponsibleEmployeeResult> SearchProjectERPResponsibleEmployeeResult(
        IQueryable<ProjectERPResponsibleEmployeeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.ResponsibleEmployeeFullName.Contains(searchText) ||
                      item.ProjectERPTitle.Contains(searchText) ||
                      item.Description.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPResponsibleEmployee, ProjectERPResponsibleEmployeeResult>> ToProjectERPResponsibleEmployeeResult =
         projectERPResponsibleEmployee => new ProjectERPResponsibleEmployeeResult()
         {
           ProjectERPId = projectERPResponsibleEmployee.ProjectERPId,
           ProjectERPTitle = projectERPResponsibleEmployee.ProjectERP.Title,
           IsActive = projectERPResponsibleEmployee.IsActive,
           ResponsibleEmployeeId = projectERPResponsibleEmployee.ResponsibleEmployeeId,
           ResponsibleEmployeeFullName = projectERPResponsibleEmployee.ResponsibleEmployee.FirstName + " " + projectERPResponsibleEmployee.ResponsibleEmployee.LastName,
           CreatorDateTime = projectERPResponsibleEmployee.CreatorDateTime,
           Description = projectERPResponsibleEmployee.Description,
           RowVersion = projectERPResponsibleEmployee.RowVersion
         };
    #endregion
  }
}
