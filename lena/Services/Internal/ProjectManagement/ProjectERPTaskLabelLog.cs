using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPTaskLabelLog;
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
    public ProjectERPTaskLabelLog GetProjectERPTaskLabelLog(
        int projectERPTaskId,
        TValue<short> projectERPLabelId = null) => GetProjectERPTaskLabelLog(
        selector: e => e,
        projectERPTaskId: projectERPTaskId,
        projectERPLabelId: projectERPLabelId);

    public TResult GetProjectERPTaskLabelLog<TResult>(
        Expression<Func<ProjectERPTaskLabelLog, TResult>> selector,
        int projectERPTaskId,
        short projectERPLabelId)
    {

      var projectERPTaskLabelLog = GetProjectERPTaskLabelLogs(selector: selector,
                projectERPTaskId: projectERPTaskId,
                projectERPLabelId: projectERPLabelId)

            .FirstOrDefault();

      if (projectERPTaskLabelLog == null)
        throw new ProjectERPTaskLabelLogNotFoundException(projectERPTaskId: projectERPTaskId, projectERPLabelId: projectERPLabelId);
      return projectERPTaskLabelLog;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPTaskLabelLogs<TResult>(
        Expression<Func<ProjectERPTaskLabelLog, TResult>> selector,
        TValue<int> projectERPTaskId = null,
        TValue<int> projectERPLabelId = null)
    {

      var query = repository.GetQuery<ProjectERPTaskLabelLog>();
      if (projectERPTaskId != null)
        query = query.Where(m => m.ProjectERPTaskId == projectERPTaskId);
      if (projectERPLabelId != null)
        query = query.Where(m => m.ProjectERPLabelId == projectERPLabelId);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPTaskLabelLog
    public ProjectERPTaskLabelLog AddProjectERPTaskLabelLog(
        int projectERPTaskId,
        short projectERPLabelId)
    {

      var projectERPTaskLabelLog = repository.Create<ProjectERPTaskLabelLog>();
      projectERPTaskLabelLog.ProjectERPTaskId = projectERPTaskId;
      projectERPTaskLabelLog.ProjectERPLabelId = projectERPLabelId;
      repository.Add(projectERPTaskLabelLog);
      return projectERPTaskLabelLog;
    }

    #endregion

    #region EditProjectERPTaskLabelLog

    public ProjectERPTaskLabelLog EditProjectERPTaskLabelLog(
     int projectERPTaskId,
     short projectERPLabelId,
     byte[] rowVersion)
    {

      var projectERPTaskLabelLog = GetProjectERPTaskLabelLog(projectERPTaskId: projectERPTaskId);

      if (projectERPTaskId != null)
        projectERPTaskLabelLog.ProjectERPTaskId = projectERPTaskId;
      if (projectERPLabelId != null)
        projectERPTaskLabelLog.ProjectERPLabelId = projectERPLabelId;

      repository.Update(entity: projectERPTaskLabelLog, rowVersion: projectERPTaskLabelLog.RowVersion);
      return projectERPTaskLabelLog;
    }
    #endregion

    #region DeleteProjectERPTaskLabelLog
    public void DeleteProjectERPTaskLabelLog(int projectERPTaskId, short projectERPLabelId)
    {

      var projectERPTaskLabelLog = GetProjectERPTaskLabelLog(projectERPTaskId: projectERPTaskId, projectERPLabelId: projectERPLabelId);
      repository.Delete(projectERPTaskLabelLog);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPTaskLabelLogResult> SortProjectERPTaskLabelLogResult(
        IQueryable<ProjectERPTaskLabelLogResult> query, SortInput<ProjectERPTaskLabelLogSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPTaskLabelLogSortType.ProjectERPTaskId:
          return query.OrderBy(a => a.ProjectERPTaskId, type.SortOrder);
        case ProjectERPTaskLabelLogSortType.ProjectERPLabelId:
          return query.OrderBy(a => a.ProjectERPLabelId, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPTaskLabelLogResult> SearchProjectERPTaskLabelLogResult(
        IQueryable<ProjectERPTaskLabelLogResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                    item.ProjectERPTaskId.ToString().Contains(searchText)
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
    public IQueryable<ProjectERPTaskLabelLogComboResult> ToProjectERPTaskLabelLogComboResultQuery(IQueryable<ProjectERPTaskLabelLog> query)
    {
      var result = from projectERPTaskLabelLog in query
                   select new ProjectERPTaskLabelLogComboResult()
                   {
                     ProjectERPLabelId = projectERPTaskLabelLog.ProjectERPLabelId,
                     ProjectERPTaskId = projectERPTaskLabelLog.ProjectERPTaskId
                   };
      return result;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPTaskLabelLog, ProjectERPTaskLabelLogResult>> ToProjectERPTaskLabelLogResult =
         projectERPTaskLabelLog => new ProjectERPTaskLabelLogResult()
         {
           ProjectERPTaskId = projectERPTaskLabelLog.ProjectERPTaskId,
           ProjectERPLabelId = projectERPTaskLabelLog.ProjectERPLabelId,
           RowVersion = projectERPTaskLabelLog.RowVersion
         };
    #endregion
  }
}
