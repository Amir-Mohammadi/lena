using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPLabelLog;
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
    public ProjectERPLabelLog GetProjectERPLabelLog(int projectERPId) => GetProjectERPLabelLog(selector: e => e, projectERPId: projectERPId);
    public TResult GetProjectERPLabelLog<TResult>(
        Expression<Func<ProjectERPLabelLog, TResult>> selector,
        int projectERPId)
    {

      var projectERPLabelLog = GetProjectERPLabelLogs(selector: selector,
                projectERPId: projectERPId).FirstOrDefault();
      if (projectERPLabelLog == null)
        throw new ProjectERPLabelLogNotFoundException(id: projectERPId);
      return projectERPLabelLog;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPLabelLogs<TResult>(
        Expression<Func<ProjectERPLabelLog, TResult>> selector,
        TValue<int> projectERPId = null,
        TValue<int> projectERPLabelId = null)
    {

      var query = repository.GetQuery<ProjectERPLabelLog>();
      if (projectERPId != null)
        query = query.Where(m => m.ProjectERPId == projectERPId);
      if (projectERPLabelId != null)
        query = query.Where(m => m.ProjectERPLabelId == projectERPLabelId);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPLabelLog
    public ProjectERPLabelLog AddProjectERPLabelLog(
        int projectERPId,
        short projectERPLabelId)
    {

      var projectERPLabelLog = repository.Create<ProjectERPLabelLog>();
      projectERPLabelLog.ProjectERPId = projectERPId;
      projectERPLabelLog.ProjectERPLabelId = projectERPLabelId;
      repository.Add(projectERPLabelLog);
      return projectERPLabelLog;
    }

    #endregion

    #region EditProjectERPLabelLog

    public ProjectERPLabelLog EditProjectERPLabelLog(
     int projectERPId,
     short projectERPLabelId,
     byte[] rowVersion)
    {

      var projectERPLabelLog = GetProjectERPLabelLog(projectERPId: projectERPId);

      if (projectERPId != null)
        projectERPLabelLog.ProjectERPId = projectERPId;
      if (projectERPLabelId != null)
        projectERPLabelLog.ProjectERPLabelId = projectERPLabelId;

      repository.Update(entity: projectERPLabelLog, rowVersion: projectERPLabelLog.RowVersion);
      return projectERPLabelLog;
    }
    #endregion

    #region DeleteProjectERPLabelLog
    public void DeleteProjectERPLabelLog(int projectERPId)
    {

      var projectERPLabelLog = GetProjectERPLabelLog(projectERPId: projectERPId);
      repository.Delete(projectERPLabelLog);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPLabelLogResult> SortProjectERPLabelLogResult(
        IQueryable<ProjectERPLabelLogResult> query, SortInput<ProjectERPLabelLogSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPLabelLogSortType.ProjectERPId:
          return query.OrderBy(a => a.ProjectERPId, type.SortOrder);
        case ProjectERPLabelLogSortType.ProjectERPLabelId:
          return query.OrderBy(a => a.ProjectERPLabelId, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPLabelLogResult> SearchProjectERPLabelLogResult(
        IQueryable<ProjectERPLabelLogResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                    item.ProjectERPId.ToString().Contains(searchText)
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
    public IQueryable<ProjectERPLabelLogComboResult> ToProjectERPLabelLogComboResultQuery(IQueryable<ProjectERPLabelLog> query)
    {
      var result = from projectERPLabelLog in query
                   select new ProjectERPLabelLogComboResult()
                   {
                     ProjectERPLabelId = projectERPLabelLog.ProjectERPLabelId,
                     ProjectERPId = projectERPLabelLog.ProjectERPId
                   };
      return result;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPLabelLog, ProjectERPLabelLogResult>> ToProjectERPLabelLogResult =
         projectERPLabelLog => new ProjectERPLabelLogResult()
         {
           ProjectERPId = projectERPLabelLog.ProjectERPId,
           ProjectERPLabelId = projectERPLabelLog.ProjectERPLabelId,
           RowVersion = projectERPLabelLog.RowVersion
         };
    #endregion
  }
}
