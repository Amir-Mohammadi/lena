using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using lena.Models;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectWork;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement
{
  public partial class ProjectManagement
  {
    public ProjectWork AddProjectWork(
        ProjectWork projectWork,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int projectStepId,
        int? baseEntityId)
    {
      projectWork = projectWork ?? repository.Create<ProjectWork>();
      var retValue = App.Internals.ScrumManagement.AddScrumBackLog(
                scrumBackLog: projectWork,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit,
                scrumSprintId: projectStepId,
                baseEntityId: baseEntityId
                );
      return retValue as ProjectWork;
    }
    public ProjectWork EditProjectWork(byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> projectId = null
        )
    {
      var projectWork = GetProjectWork(id: id);
      return EditProjectWork(
                    rowVersion: rowVersion,
                    projectWork: projectWork,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete,
                    projectStepId: projectId);
    }
    public ProjectWork EditProjectWork(
        byte[] rowVersion,
        ProjectWork projectWork,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> projectStepId = null)
    {
      var retValue = App.Internals.ScrumManagement.EditScrumBackLog(
                    rowVersion: rowVersion,
                    scrumBackLog: projectWork,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete,
                    scrumSprintId: projectStepId
                    );
      return retValue as ProjectWork;
    }
    public IQueryable<TResult> GetProjectWorks<TResult>(
        Expression<Func<ProjectWork, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<int> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> projectStepId = null,
        TValue<int?> baseEntityId = null)
    {
      var scrumEntitiesQuery = App.Internals.ScrumManagement.GetScrumBackLogs(
                selector: e => e,
                id: id,
                code: code,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit,
                isDelete: isDelete,
                scrumSprintId: projectStepId,
                baseEntityId: baseEntityId);
      var projectWorkQuery = from item in scrumEntitiesQuery.OfType<ProjectWork>()
                             select item;
      return projectWorkQuery.Select(selector);
    }
    public ProjectWork GetProjectWork(int id) => GetProjectWork(selector: e => e, id: id);
    public TResult GetProjectWork<TResult>(
        Expression<Func<ProjectWork, TResult>> selector,
        int id)
    {
      var projectWork = GetProjectWorks(
                    selector: selector,
                    id: id)
                .SingleOrDefault();
      if (projectWork == null)
        throw new ProjectWorkNotFoundException(id);
      return projectWork;
    }
    public void DeleteProjectWork(int id)
    {
      var projectWork = GetProjectWork(id);
      repository.Delete(projectWork);
    }
    public IQueryable<ProjectWorkResult> SearchProjectWorkResult(IQueryable<ProjectWorkResult> query, string search)
    {
      if (string.IsNullOrEmpty(search))
        return query;
      return from item in query
             where
             item.Code.Contains(search) ||
             item.Name.Contains(search) ||
             item.Description.Contains(search) ||
             item.DepartmentName.Contains(search)
             select item;
    }
    public IOrderedQueryable<ProjectWorkResult> SortProjectWorkResult(IQueryable<ProjectWorkResult> query, SortInput<ProjectWorkSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProjectWorkSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ProjectWorkSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ProjectWorkSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ProjectWorkSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        case ProjectWorkSortType.ProjectStepName:
          return query.OrderBy(a => a.ProjectStepName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ProjectWorkResult> ToProjectWorkResultQuery(IQueryable<ProjectWork> query)
    {
      var resultQuery = from projectWork in query
                        let department = projectWork.Department
                        let projectStep = projectWork.ScrumSprint as ProjectStep
                        select new ProjectWorkResult
                        {
                          Id = projectWork.Id,
                          Code = projectWork.Code,
                          Name = projectWork.Name,
                          Description = projectWork.Description,
                          Color = projectWork.Color,
                          IsCommit = projectWork.IsCommit,
                          IsDelete = projectWork.IsDelete,
                          DepartmentId = department.Id,
                          DepartmentName = department.Name,
                          ProjectStepId = projectWork.ScrumSprintId,
                          ProjectStepName = projectStep.Name,
                          EstimatedTime = projectWork.EstimatedTime,
                          RowVersion = projectWork.RowVersion,
                        };
      return resultQuery;
    }
    public ProjectWorkResult ToProjectWorkResult(ProjectWork projectWork)
    {
      var department = projectWork.Department;
      var projectStep = projectWork.ScrumSprint as ProjectStep;
      var result = new ProjectWorkResult
      {
        Id = projectWork.Id,
        Code = projectWork.Code,
        Name = projectWork.Name,
        Description = projectWork.Description,
        Color = projectWork.Color,
        IsCommit = projectWork.IsCommit,
        IsDelete = projectWork.IsDelete,
        DepartmentId = department.Id,
        DepartmentName = department.Name,
        EstimatedTime = projectWork.EstimatedTime,
        ProjectStepId = projectWork.ScrumSprintId,
        ProjectStepName = projectStep.Name,
        RowVersion = projectWork.RowVersion,
      };
      return result;
    }
    public ProjectWork ArchiveProjectWork(byte[] rowVersion, int id)
    {
      return EditProjectWork(id: id, rowVersion: rowVersion, isDelete: true);
    }
    public ProjectWork RestoreProjectWork(byte[] rowVersion, int id)
    {
      return EditProjectWork(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}