using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Runtime.Remoting;
using lena.Models;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectWorkItem;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement
{
  public partial class ProjectManagement
  {
    public ProjectWorkItem AddProjectWorkItem(
        ProjectWorkItem projectWorkItem,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int scrumTaskTypeId,
        int? userId,
        long spentTime,
        long remainedTime,
        ScrumTaskStep scrumTaskStep,
        int projectWorkId,
        int? baseEntityId)
    {
      projectWorkItem = projectWorkItem ?? repository.Create<ProjectWorkItem>();
      var retValue = App.Internals.ScrumManagement.AddScrumTaskProcess(
                    scrumTask: projectWorkItem,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    scrumTaskTypeId: scrumTaskTypeId,
                    userId: userId,
                    spentTime: spentTime,
                    remainedTime: remainedTime,
                    scrumTaskStep: scrumTaskStep,
                    scrumBackLogId: projectWorkId,
                    baseEntityId: baseEntityId);
      return retValue as ProjectWorkItem;
    }
    public ProjectWorkItem EditProjectWorkItem(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<int> scrumTaskTypeId = null,
        TValue<int?> userId = null,
        TValue<long> spentTime = null,
        TValue<long> remainedTime = null,
        TValue<ScrumTaskStep> scrumTaskStep = null,
        TValue<int> projectWorkId = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null)
    {
      var projectWorkItem = GetProjectWorkItem(id: id);
      return
                EditProjectWorkItem(
                        projectWorkItem: projectWorkItem,
                        rowVersion: rowVersion,
                        code: code,
                        name: name,
                        description: description,
                        color: color,
                        departmentId: departmentId,
                        estimatedTime: estimatedTime,
                        isCommit: isCommit,
                        scrumTaskTypeId: scrumTaskTypeId,
                        userId: userId,
                        spentTime: spentTime,
                        remainedTime: remainedTime,
                        scrumTaskStep: scrumTaskStep,
                        projectWorkId: projectWorkId,
                        isDelete: isDelete,
                        isArchive: isArchive);
    }
    public ProjectWorkItem EditProjectWorkItem(
        ProjectWorkItem projectWorkItem,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<int> scrumTaskTypeId = null,
        TValue<int?> userId = null,
        TValue<long> spentTime = null,
        TValue<long> remainedTime = null,
        TValue<ScrumTaskStep> scrumTaskStep = null,
        TValue<int> projectWorkId = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null)
    {
      var retValue = App.Internals.ScrumManagement.EditScrumTask(
                    scrumTask: projectWorkItem,
                    rowVersion: rowVersion,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    scrumTaskTypeId: scrumTaskTypeId,
                    userId: userId,
                    spentTime: spentTime,
                    remainedTime: remainedTime,
                    scrumTaskStep: ScrumTaskStep.Done,
                    scrumBackLogId: projectWorkId,
                    isDelete: isDelete,
                    isArchive: isArchive);
      return retValue as ProjectWorkItem;
    }
    public IQueryable<ProjectWorkItem> GetProjectWorkItems(
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<int> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<int> scrumTaskTypeId = null,
        TValue<int?> userId = null,
        TValue<long> spentTime = null,
        TValue<long> remainedTime = null,
        TValue<ScrumTaskStep> scrumTaskStep = null,
        TValue<int> projectWorkId = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int?> baseEntityId = null)
    {
      var scrumEntitiesQuery = App.Internals.ScrumManagement.GetScrumTasks(
                    selector: e => e,
                    id: id,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    scrumTaskTypeId: scrumTaskTypeId,
                    userId: userId,
                    spentTime: spentTime,
                    remainedTime: remainedTime,
                    scrumTaskStep: scrumTaskStep,
                    scrumBackLogId: projectWorkId,
                    isDelete: isDelete,
                    isArchive: isArchive,
                    baseEntityId: baseEntityId);
      var projectWorkItemQuery = from item in scrumEntitiesQuery.OfType<ProjectWorkItem>()
                                 select item;
      return projectWorkItemQuery;
    }
    public ProjectWorkItem GetProjectWorkItem(int id)
    {
      var projectWorkItem = GetProjectWorkItems(id: id).SingleOrDefault();
      if (projectWorkItem == null)
        throw new ProjectWorkItemNotFoundException(id);
      return projectWorkItem;
    }
    public void DeleteProjectWorkItem(int id)
    {
      var projectWorkItem = GetProjectWorkItem(id);
      repository.Delete(projectWorkItem);
    }
    public IQueryable<ProjectWorkItemResult> SearchProjectWorkItemResult(IQueryable<ProjectWorkItemResult> query, string search)
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
    public IOrderedQueryable<ProjectWorkItemResult> SortProjectWorkItemResult(IQueryable<ProjectWorkItemResult> query, SortInput<ProjectWorkItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProjectWorkItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ProjectWorkItemSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ProjectWorkItemSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ProjectWorkItemSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        case ProjectWorkItemSortType.ProjectWorkName:
          return query.OrderBy(a => a.ProjectWorkName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ProjectWorkItemResult> ToProjectWorkItemResultQuery(IQueryable<ProjectWorkItem> query)
    {
      var userId = App.Providers.Security.CurrentLoginData.UserId;
      var resultQuery = from projectWorkItem in query
                        let department = projectWorkItem.Department
                        let projectWork = projectWorkItem.ScrumBackLog as ProjectWork
                        let scrumTaskType = projectWorkItem.ScrumTaskType
                        let user = projectWorkItem.User
                        let employee = user.Employee
                        select new ProjectWorkItemResult
                        {
                          Id = projectWorkItem.Id,
                          Code = projectWorkItem.Code,
                          Name = projectWorkItem.Name,
                          Description = projectWorkItem.Description,
                          Color = projectWorkItem.Color,
                          DepartmentId = projectWorkItem.DepartmentId,
                          DepartmentName = department.Name,
                          ProjectWorkId = projectWorkItem.ScrumBackLogId,
                          ProjectWorkName = projectWork.Name,
                          UserId = projectWorkItem.UserId,
                          UserName = user.UserName,
                          EmployeeCode = (employee != null) ? employee.Code : "",
                          EmployeeName = (employee != null) ? employee.FirstName + " " + employee.LastName : "",
                          EstimatedTime = projectWorkItem.EstimatedTime,
                          RemainedTime = projectWorkItem.RemainedTime,
                          SpentTime = projectWorkItem.SpentTime,
                          ScrumTaskTypeId = scrumTaskType.Id,
                          ScrumTaskTypeName = scrumTaskType.Name,
                          Status = projectWorkItem.ScrumTaskStep,
                          IsCommit = projectWorkItem.IsCommit,
                          IsDelete = projectWorkItem.IsDelete,
                          IsMyTask = projectWorkItem.UserId == userId,
                          RowVersion = projectWorkItem.RowVersion
                        };
      return resultQuery;
    }
    public ProjectWorkItemResult ToProjectWorkItemResult(ProjectWorkItem projectWorkItem)
    {
      var userId = App.Providers.Security.CurrentLoginData.UserId;
      var department = projectWorkItem.Department;
      var projectWork = projectWorkItem.ScrumBackLog as ProjectWork;
      var scrumTaskType = projectWorkItem.ScrumTaskType;
      var user = projectWorkItem.User;
      var employee = user.Employee;
      var result = new ProjectWorkItemResult
      {
        Id = projectWorkItem.Id,
        Code = projectWorkItem.Code,
        Name = projectWorkItem.Name,
        Description = projectWorkItem.Description,
        Color = projectWorkItem.Color,
        DepartmentId = projectWorkItem.DepartmentId,
        DepartmentName = department.Name,
        ProjectWorkId = projectWorkItem.ScrumBackLogId,
        ProjectWorkName = projectWork.Name,
        UserId = projectWorkItem.UserId,
        UserName = user.UserName,
        EmployeeCode = (employee != null) ? employee.Code : "",
        EmployeeName = (employee != null) ? employee.FirstName + " " + employee.LastName : "",
        EstimatedTime = projectWorkItem.EstimatedTime,
        RemainedTime = projectWorkItem.RemainedTime,
        SpentTime = projectWorkItem.SpentTime,
        ScrumTaskTypeId = scrumTaskType.Id,
        ScrumTaskTypeName = scrumTaskType.Name,
        Status = projectWorkItem.ScrumTaskStep,
        IsCommit = projectWorkItem.IsCommit,
        IsDelete = projectWorkItem.IsDelete,
        IsMyTask = projectWorkItem.UserId == userId,
        RowVersion = projectWorkItem.RowVersion
      };
      return result;
    }
    public ProjectWorkItem ArchiveProjectWorkItem(byte[] rowVersion, int id)
    {
      return EditProjectWorkItem(id: id, rowVersion: rowVersion, isDelete: true);
    }
    public ProjectWorkItem RestoreProjectWorkItem(byte[] rowVersion, int id)
    {
      return EditProjectWorkItem(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}