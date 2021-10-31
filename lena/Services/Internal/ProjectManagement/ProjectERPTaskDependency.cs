using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.ProjectManagement.ProjectERPTaskDependency;
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
    public ProjectERPTaskDependency GetProjectERPTaskDependency(
        int id) => GetProjectERPTaskDependency(
        selector: e => e,
        id: id);

    public TResult GetProjectERPTaskDependency<TResult>(
        Expression<Func<ProjectERPTaskDependency, TResult>> selector,
        int id)
    {

      var projectERPTaskDependency = GetProjectERPTaskDependencys(selector: selector,
                projectERPTaskId: id)

            .FirstOrDefault();

      if (projectERPTaskDependency == null)
        throw new ProjectERPTaskDependencyNotFoundException(id: id);
      return projectERPTaskDependency;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPTaskDependencys<TResult>(
        Expression<Func<ProjectERPTaskDependency, TResult>> selector,
        TValue<int> lagMinutues = null,
        TValue<int> projectERPTaskId = null,
        TValue<ProjectERPTaskDependencyType> dependencyType = null,
        TValue<int> predecessorProjectERPTaskId = null)
    {

      var query = repository.GetQuery<ProjectERPTaskDependency>();
      if (projectERPTaskId != null)
        query = query.Where(m => m.ProjectERPTaskId == projectERPTaskId);
      if (predecessorProjectERPTaskId != null)
        query = query.Where(m => m.PredecessorProjectERPTaskId == predecessorProjectERPTaskId);
      if (dependencyType != null)
        query = query.Where(m => m.DependencyType == dependencyType);
      if (lagMinutues != null)
        query = query.Where(m => m.LagMinutues == lagMinutues);

      return query.Select(selector);
    }
    #endregion


    #region AddProjectERPTaskDependency
    public ProjectERPTaskDependency AddProjectERPTaskDependency(
        int projectERPTaskId,
        int lagMinutues,
        ProjectERPTaskDependencyType dependencyType,
        int predecessorProjectERPTaskId)
    {

      var projectERPTaskDependency = repository.Create<ProjectERPTaskDependency>();
      projectERPTaskDependency.LagMinutues = lagMinutues;
      projectERPTaskDependency.DependencyType = dependencyType;
      projectERPTaskDependency.ProjectERPTaskId = projectERPTaskId;
      projectERPTaskDependency.PredecessorProjectERPTaskId = predecessorProjectERPTaskId;
      repository.Add(projectERPTaskDependency);
      return projectERPTaskDependency;
    }

    #endregion

    #region EditProjectERPTaskDependency

    public ProjectERPTaskDependency EditProjectERPTaskDependency(
        int id,
        byte[] rowVersion,
        int projectERPTaskId,
        int lagMinutues,
        ProjectERPTaskDependencyType dependencyType,
        int predecessorProjectERPTaskId)
    {

      var projectERPTaskDependency = GetProjectERPTaskDependency(id: id);
      if (projectERPTaskId != null)
        projectERPTaskDependency.ProjectERPTaskId = projectERPTaskId;
      if (lagMinutues != null)
        projectERPTaskDependency.LagMinutues = lagMinutues;
      if (dependencyType != null)
        projectERPTaskDependency.DependencyType = dependencyType;
      if (predecessorProjectERPTaskId != null)
        projectERPTaskDependency.PredecessorProjectERPTaskId = predecessorProjectERPTaskId;

      repository.Update(entity: projectERPTaskDependency, rowVersion: projectERPTaskDependency.RowVersion);
      return projectERPTaskDependency;
    }
    #endregion

    #region DeleteProjectERPTaskDependency
    public void DeleteProjectERPTaskDependency(
        int id,
        byte[] rowVersion)
    {

      var projectERPTaskDependency = GetProjectERPTaskDependency(id: id);
      repository.Delete(projectERPTaskDependency);
    }
    #endregion

    #region Sort
    //public IOrderedQueryable<ProjectERPTaskDependencyResult> SortProjectERPTaskDependencyResult(
    //    IQueryable<ProjectERPTaskDependencyResult> query, SortInput<ProjectERPTaskDependencySortType> type)
    //{
    //    switch (type.SortType)
    //    {
    //        case ProjectERPTaskDependencySortType.ProjectERPTaskId:
    //            return query.OrderBy(a => a.ProjectERPTaskId, type.SortOrder);
    //        case ProjectERPTaskDependencySortType.ProjectERPLabelId:
    //            return query.OrderBy(a => a.ProjectERPLabelId, type.SortOrder);

    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}
    #endregion

    #region Search
    //public IQueryable<ProjectERPTaskDependencyResult> SearchProjectERPTaskDependencyResult(
    //    IQueryable<ProjectERPTaskDependencyResult> query,
    //    string searchText,
    //    AdvanceSearchItem[] advanceSearchItems)
    //{
    //    if (!string.IsNullOrWhiteSpace(searchText))
    //    {
    //        query = from item in query
    //                where
    //                    item.ProjectERPTaskId.ToString().Contains(searchText)
    //                select item;
    //    }

    //    if (advanceSearchItems.Any())
    //    {
    //        query = query.Where(advanceSearchItems);
    //    }

    //    return query;
    //}
    #endregion


    #region ToResult
    public Expression<Func<ProjectERPTaskDependency, ProjectERPTaskDependencyResult>> ToProjectERPTaskDependencyResult =
         projectERPTaskDependency => new ProjectERPTaskDependencyResult()
         {
           ProjectERPTaskId = projectERPTaskDependency.ProjectERPTaskId,
           PredecessorProjectERPTaskId = projectERPTaskDependency.PredecessorProjectERPTaskId,
           DependencyType = projectERPTaskDependency.DependencyType,
           LagMinutues = projectERPTaskDependency.LagMinutues,
           RowVersion = projectERPTaskDependency.RowVersion
         };
    #endregion

  }
}
