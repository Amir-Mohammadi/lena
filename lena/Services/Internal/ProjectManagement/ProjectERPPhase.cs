using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPPhase;
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
    public ProjectERPPhase GetProjectERPPhase(int id) => GetProjectERPPhase(selector: e => e, id: id);
    public TResult GetProjectERPPhase<TResult>(
        Expression<Func<ProjectERPPhase, TResult>> selector,
        int id)
    {

      var projectERPPhase = GetProjectERPPhases(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPPhase == null)
        throw new ProjectERPPhaseNotFoundException(id: id);
      return projectERPPhase;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPPhases<TResult>(
        Expression<Func<ProjectERPPhase, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<string> description = null
    )
    {

      var query = repository.GetQuery<ProjectERPPhase>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (description != null)
        query = query.Where(m => m.Description == description);
      if (isActive != null)
        query = query.Where(m => m.IsActive == isActive);
      if (name != null)
        query = query.Where(m => m.Name == name);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPPhase

    public ProjectERPPhase AddProjectERPPhase(
        string name,
        bool isActive,
        string description)
    {

      var projectERPPhase = repository.Create<ProjectERPPhase>();
      projectERPPhase.Name = name;
      projectERPPhase.IsActive = isActive;
      projectERPPhase.Description = description;
      repository.Add(projectERPPhase);
      return projectERPPhase;
    }

    #endregion

    #region EditProjectERPPhase

    public ProjectERPPhase EditProjectERPPhase(
     int id,
     byte[] rowVersion,
     TValue<string> name = null,
     TValue<bool> isActive = null,
     TValue<string> description = null)
    {

      var projectERPPhase = GetProjectERPPhase(id: id);
      if (name != null)
        projectERPPhase.Name = name;
      if (isActive != null)
        projectERPPhase.IsActive = isActive;
      if (description != null)
        projectERPPhase.Description = description;

      repository.Update(entity: projectERPPhase, rowVersion: projectERPPhase.RowVersion);
      return projectERPPhase;
    }
    #endregion

    #region DeleteProjectERPPhase
    public void DeleteProjectERPPhase(int id)
    {

      var projectERPPhase = GetProjectERPPhase(id: id);
      repository.Delete(projectERPPhase);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPPhaseResult> SortProjectERPPhaseResult(
        IQueryable<ProjectERPPhaseResult> query, SortInput<ProjectERPPhaseSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPPhaseSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case ProjectERPPhaseSortType.Name:
          return query.OrderBy(a => a.Name, type.SortOrder);
        case ProjectERPPhaseSortType.IsActive:
          return query.OrderBy(a => a.IsActive, type.SortOrder);
        case ProjectERPPhaseSortType.Description:
          return query.OrderBy(a => a.Description, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPPhaseResult> SearchProjectERPPhaseResult(
        IQueryable<ProjectERPPhaseResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                    item.Name.Contains(searchText) ||
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

    #region ToComboResult
    public IQueryable<ProjectERPPhaseComboResult> ToProjectERPPhaseComboResultQuery(IQueryable<ProjectERPPhase> query)
    {
      var result = from projectERPPhase in query
                   select new ProjectERPPhaseComboResult()
                   {
                     Id = projectERPPhase.Id,
                     Name = projectERPPhase.Name,
                     IsActive = projectERPPhase.IsActive,
                     Description = projectERPPhase.Description
                   };
      return result;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPPhase, ProjectERPPhaseResult>> ToProjectERPPhaseResult =
         projectERPPhase => new ProjectERPPhaseResult()
         {
           Id = projectERPPhase.Id,
           Name = projectERPPhase.Name,
           IsActive = projectERPPhase.IsActive,
           Description = projectERPPhase.Description,
           RowVersion = projectERPPhase.RowVersion
         };
    #endregion
  }
}
