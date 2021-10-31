using lena.Models.Common;
using lena.Domains;
using System;
using System.Globalization;
using System.Linq;
using lena.Models;
using lena.Domains.Enums;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectStep;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement
{
  public partial class ProjectManagement
  {
    #region Get Or Add
    public ProjectStep GetOrAddCommonProjectStep(short departmentId)
    {
      var projectStep = repository.Create<ProjectStep>();
      #region Get ProjectStep
      projectStep.DepartmentId = departmentId;
      App.Internals.ScrumManagement.GenerateCode(projectStep);
      projectStep = GetProjectSteps(code: projectStep.Code)
                .FirstOrDefault();
      #endregion
      #region Add Project
      if (projectStep == null)
      {
        var department = App.Internals.ApplicationBase.GetDepartment(departmentId); ; var scrumProject = App.Internals.ProjectManagement.GetOrAddCommonProject(departmentId);
        var year = new PersianCalendar().GetYear(DateTime.Now.ToUniversalTime());
        var month = new PersianCalendar().GetMonth(DateTime.Now.ToUniversalTime());
        projectStep = App.Internals.ProjectManagement.AddProjectStep(
                      projectStep: null,
                      name: $"فعالیت کلی {department.Name} سال {year:0000} ماه {month:00}",
                      description: "",
                      color: "",
                      departmentId: departmentId,
                      estimatedTime: 0,
                      isCommit: false,
                      projectId: scrumProject.Id,
                      baseEntityId: null);
      }
      #endregion
      return projectStep;
    }
    #endregion
    public ProjectStep AddProjectStep(
        ProjectStep projectStep,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int projectId,
        int? baseEntityId)
    {
      projectStep = projectStep ?? repository.Create<ProjectStep>();
      var retValue = App.Internals.ScrumManagement.AddScrumSprint(
                scrumSprint: projectStep,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit,
                scrumProjectId: projectId,
                baseEntityId: baseEntityId
                );
      return retValue as ProjectStep;
    }
    public ProjectStep EditProjectStep(byte[] rowVersion,
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
      var projectStep = GetProjectStep(id: id);
      return
                EditProjectStep(
                        projectStep: projectStep,
                        rowVersion: rowVersion,
                        code: code,
                        name: name,
                        description: description,
                        color: color,
                        departmentId: departmentId,
                        estimatedTime: estimatedTime,
                        isCommit: isCommit,
                        isDelete: isDelete,
                        projectId: projectId);
    }
    public ProjectStep EditProjectStep(ProjectStep projectStep,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> projectId = null)
    {
      var retValue = App.Internals.ScrumManagement.EditScrumSprint(
                    rowVersion: rowVersion,
                    scrumSprint: projectStep,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete,
                    scrumProjectId: projectId);
      return retValue as ProjectStep;
    }
    public IQueryable<ProjectStep> GetProjectSteps(
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<int> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> projectId = null,
        TValue<int?> baseEntityId = null)
    {
      var scrumEntitiesQuery = App.Internals.ScrumManagement.GetScrumSprints(
                id: id,
                code: code,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit,
                isDelete: isDelete,
                scrumProjectId: projectId,
                baseEntityId: baseEntityId);
      var projectStepQuery = from item in scrumEntitiesQuery.OfType<ProjectStep>()
                             select item;
      return projectStepQuery;
    }
    public ProjectStep GetProjectStep(int id)
    {
      var projectStep = GetProjectSteps(id: id).SingleOrDefault();
      if (projectStep == null)
        throw new RecordNotFoundException(id, typeof(ProjectStep));
      return projectStep;
    }
    public void DeleteProjectStep(int id)
    {
      var projectStep = GetProjectStep(id);
      repository.Delete(projectStep);
    }
    public IQueryable<ProjectStepResult> SearchProjectStepResult(IQueryable<ProjectStepResult> query, string search)
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
    public IOrderedQueryable<ProjectStepResult> SortProjectStepResult(IQueryable<ProjectStepResult> query, SortInput<ProjectStepSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProjectStepSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ProjectStepSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ProjectStepSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ProjectStepSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        case ProjectStepSortType.ProjectName:
          return query.OrderBy(a => a.ProjectName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ProjectStepResult> ToProjectStepResultQuery(IQueryable<ProjectStep> query)
    {
      var resultQuery = from projectStep in query
                        let department = projectStep.Department
                        let project = projectStep.ScrumProject as Project
                        select new ProjectStepResult
                        {
                          Id = projectStep.Id,
                          Code = projectStep.Code,
                          Name = projectStep.Name,
                          Description = projectStep.Description,
                          Color = projectStep.Color,
                          IsCommit = projectStep.IsCommit,
                          IsDelete = projectStep.IsDelete,
                          DepartmentId = department.Id,
                          DepartmentName = department.Name,
                          ProjectId = projectStep.ScrumProjectId,
                          ProjectName = project.Name,
                          EstimatedTime = projectStep.EstimatedTime,
                          RowVersion = projectStep.RowVersion,
                        };
      return resultQuery;
    }
    public ProjectStepResult ToProjectStepResult(ProjectStep projectStep)
    {
      var department = projectStep.Department;
      var project = projectStep.ScrumProject as Project;
      var result = new ProjectStepResult
      {
        Id = projectStep.Id,
        Code = projectStep.Code,
        Name = projectStep.Name,
        Description = projectStep.Description,
        Color = projectStep.Color,
        IsCommit = projectStep.IsCommit,
        IsDelete = projectStep.IsDelete,
        DepartmentId = department.Id,
        DepartmentName = department.Name,
        EstimatedTime = projectStep.EstimatedTime,
        ProjectId = projectStep.ScrumProjectId,
        ProjectName = project.Name,
        RowVersion = projectStep.RowVersion,
      };
      return result;
    }
    public ProjectStep ArchiveProjectStep(byte[] rowVersion, int id)
    {
      return EditProjectStep(id: id, rowVersion: rowVersion, isDelete: true);
    }
    public ProjectStep RestoreProjectStep(byte[] rowVersion, int id)
    {
      return EditProjectStep(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}