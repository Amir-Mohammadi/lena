using lena.Models.Common;
using lena.Domains;
using System;
using System.Globalization;
using System.Linq;
using lena.Models;
using lena.Domains.Enums;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Models.ProjectManagement.Project;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement
{
  public partial class ProjectManagement
  {
    #region Get Or Add
    public Project GetOrAddCommonProject(short departmentId)
    {
      var project = repository.Create<Project>();
      #region Get Project
      project.DepartmentId = departmentId;
      App.Internals.ScrumManagement.GenerateCode(project);
      project = GetProjects(code: project.Code)
                .FirstOrDefault();
      #endregion
      #region Add Project
      if (project == null)
      {
        var department = App.Internals.ApplicationBase.GetDepartment(departmentId); ; var scrumProjectGroup = App.Internals.ScrumManagement.GetOrAddCommonScrumProjectGroup(departmentId);
        var year = new PersianCalendar().GetYear(DateTime.Now.ToUniversalTime());
        project = App.Internals.ProjectManagement.AddProject(
                      project: null,
                      name: $"فعالیت کلی {department.Name} سال {year:0000}",
                      description: "",
                      color: "",
                      departmentId: departmentId,
                      estimatedTime: 0,
                      isCommit: false,
                      projectHeaderId: scrumProjectGroup.Id,
                      baseEntityId: null);
      }
      #endregion
      return project;
    }
    #endregion
    public Project AddProject(Project project,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int projectHeaderId,
        int? baseEntityId)
    {
      project = project ?? repository.Create<Project>();
      project.ScrumProjectGroupId = projectHeaderId;
      var retValue = App.Internals.ScrumManagement.AddScrumProject(
                    scrumProject: project,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    scrumProjectGroupId: projectHeaderId,
                    baseEntityId: baseEntityId);
      return retValue as Project;
    }
    public Project EditProject(byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> projectHeaderId = null
        )
    {
      var project = GetProject(id: id);
      return
                EditProject(
                        project: project,
                        rowVersion: rowVersion,
                        code: code,
                        name: name,
                        description: description,
                        color: color,
                        departmentId: departmentId,
                        estimatedTime: estimatedTime,
                        isCommit: isCommit,
                        isDelete: isDelete,
                        projectHeaderId: projectHeaderId);
    }
    public Project EditProject(
        byte[] rowVersion,
        Project project,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> projectHeaderId = null)
    {
      var retValue = App.Internals.ScrumManagement.EditScrumProject(
                    rowVersion: rowVersion,
                    scrumProject: project,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete,
                    scrumProjectGroupId: projectHeaderId
                    );
      return retValue as Project;
    }
    public IQueryable<Project> GetProjects(
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<int> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> projectHeaderId = null)
    {
      var scrumEntitiesQuery = App.Internals.ScrumManagement.GetScrumProjects(
                id: id,
                code: code,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit,
                isDelete: isDelete,
                scrumProjectGroupId: projectHeaderId);
      var projectQuery = from item in scrumEntitiesQuery.OfType<Project>()
                         select item;
      return projectQuery;
    }
    public Project GetProject(int id)
    {
      var project = GetProjects(id: id).SingleOrDefault();
      if (project == null)
        throw new ProjectNotFoundException(id);
      return project;
    }
    public void DeleteProject(int id)
    {
      var project = GetProject(id);
      repository.Delete(project);
    }
    public IQueryable<ProjectResult> SearchProjectResult(IQueryable<ProjectResult> query, string search)
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
    public IOrderedQueryable<ProjectResult> SortProjectResult(IQueryable<ProjectResult> query, SortInput<ProjectSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProjectSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ProjectSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ProjectSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ProjectSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        case ProjectSortType.ProjectHeaderName:
          return query.OrderBy(a => a.ProjectHeaderName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ProjectResult> ToProjectResultQuery(IQueryable<Project> query)
    {
      var resultQuery = from project in query
                        let department = project.Department
                        let projectHeader = project.ScrumProjectGroup as ProjectHeader
                        select new ProjectResult
                        {
                          Id = project.Id,
                          Code = project.Code,
                          Name = project.Name,
                          Description = project.Description,
                          Color = project.Color,
                          IsCommit = project.IsCommit,
                          IsDelete = project.IsDelete,
                          DepartmentId = department.Id,
                          DepartmentName = department.Name,
                          ProjectHeaderId = project.ScrumProjectGroupId,
                          ProjectHeaderName = projectHeader.Name,
                          EstimatedTime = project.EstimatedTime,
                          RowVersion = project.RowVersion,
                        };
      return resultQuery;
    }
    public ProjectResult ToProjectResult(Project project)
    {
      var department = project.Department;
      var projectHeader = project.ScrumProjectGroup as ProjectHeader;
      var result = new ProjectResult
      {
        Id = project.Id,
        Code = project.Code,
        Name = project.Name,
        Description = project.Description,
        Color = project.Color,
        IsCommit = project.IsCommit,
        IsDelete = project.IsDelete,
        DepartmentId = department.Id,
        DepartmentName = department.Name,
        EstimatedTime = project.EstimatedTime,
        ProjectHeaderId = project.ScrumProjectGroupId,
        ProjectHeaderName = projectHeader.Name,
        RowVersion = project.RowVersion,
      };
      return result;
    }
    public Project ArchiveProject(byte[] rowVersion, int id)
    {
      return EditProject(id: id, rowVersion: rowVersion, isDelete: true);
    }
    public Project RestoreProject(byte[] rowVersion, int id)
    {
      return EditProject(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}