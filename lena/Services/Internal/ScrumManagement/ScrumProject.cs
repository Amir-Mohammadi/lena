using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using lena.Models;
using lena.Domains.Enums;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ScrumManagement.Exception;
using lena.Models.Common;
using lena.Models.ScrumManagement.ScrumProject;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement
{
  public partial class ScrumManagement
  {
    public ScrumProject AddScrumProject(ScrumProject scrumProject,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int scrumProjectGroupId,
        int? baseEntityId)
    {


      scrumProject = scrumProject ?? repository.Create<ScrumProject>();
      scrumProject.ScrumProjectGroupId = scrumProjectGroupId;
      var retValue = AddScrumEntity(scrumEntity: scrumProject,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    baseEntityId: baseEntityId);
      return retValue as ScrumProject;
    }
    public ScrumProject EditScrumProject(byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> scrumProjectGroupId = null
        )
    {

      var scrumProject = GetScrumProject(id: id);
      return
                EditScrumProject(
                        rowVersion: rowVersion,
                        scrumProject: scrumProject,
                        code: code,
                        name: name,
                        description: description,
                        color: color,
                        departmentId: departmentId,
                        estimatedTime: estimatedTime,
                        isCommit: isCommit,
                        isDelete: isDelete,
                        scrumProjectGroupId: scrumProjectGroupId);
    }
    public ScrumProject EditScrumProject(
        byte[] rowVersion,
        ScrumProject scrumProject,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> scrumProjectGroupId = null)
    {


      if (scrumProjectGroupId != null)
        scrumProject.ScrumProjectGroupId = scrumProjectGroupId;
      var retValue = EditScrumEntity(rowVersion: rowVersion,
                    scrumEntity: scrumProject,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete);
      return retValue as ScrumProject;
    }
    public IQueryable<ScrumProject> GetScrumProjects(
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<int> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int> scrumProjectGroupId = null)
    {


      var isScrumProjectGroupIdNull = scrumProjectGroupId == null;
      var scrumEntitiesQuery = GetScrumEntities(
                id: id,
                code: code,
                name: name,
                description: description,
                color: color,
                isCommit: isCommit,
                estimatedTime: estimatedTime,
                departmentId: departmentId,
                isDelete: isDelete,
                isArchive: isArchive
                );
      var scrumProjectQuery = from item in scrumEntitiesQuery.OfType<ScrumProject>()
                              where (isScrumProjectGroupIdNull || item.ScrumProjectGroupId == scrumProjectGroupId)
                              select item;
      return scrumProjectQuery;
    }
    public ScrumProject GetScrumProject(int id)
    {

      var scrumProject = GetScrumProjects(id: id).SingleOrDefault();
      if (scrumProject == null)
        throw new ScrumProjectNotFoundException(id);
      return scrumProject;
    }
    public void DeleteScrumProject(int id)
    {

      var scrumProject = GetScrumProject(id);
      repository.Delete(scrumProject);
    }
    public IQueryable<ScrumProjectResult> SearchScrumProjectResult(IQueryable<ScrumProjectResult> query, string search)
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
    public IOrderedQueryable<ScrumProjectResult> SortScrumProjectResult(IQueryable<ScrumProjectResult> query, SortInput<ScrumProjectSortType> sort)
    {
      switch (sort.SortType)
      {
        case ScrumProjectSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ScrumProjectSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ScrumProjectSortType.Text:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case ScrumProjectSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ScrumProjectSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        case ScrumProjectSortType.ScrumProjectGroupName:
          return query.OrderBy(a => a.ScrumProjectGroupName, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ScrumProjectResult> ToScrumProjectResultQuery(IQueryable<ScrumProject> query)
    {
      var resultQuery = from scrumProject in query
                        let department = scrumProject.Department
                        let scrumProjectGroup = scrumProject.ScrumProjectGroup
                        select new ScrumProjectResult
                        {
                          Id = scrumProject.Id,
                          Code = scrumProject.Code,
                          Name = scrumProject.Name,
                          Description = scrumProject.Description,
                          Color = scrumProject.Color,
                          IsCommit = scrumProject.IsCommit,
                          IsDelete = scrumProject.IsDelete,
                          DepartmentId = department.Id,
                          DepartmentName = department.Name,
                          ScrumProjectGroupId = scrumProject.ScrumProjectGroupId,
                          ScrumProjectGroupName = scrumProjectGroup.Name,
                          EstimatedTime = scrumProject.EstimatedTime,
                          RowVersion = scrumProject.RowVersion,
                        };
      return resultQuery;
    }
    public ScrumProjectResult ToScrumProjectResult(ScrumProject scrumProject)
    {
      var department = scrumProject.Department;
      var scrumProjectGroup = scrumProject.ScrumProjectGroup;
      var result = new ScrumProjectResult
      {
        Id = scrumProject.Id,
        Code = scrumProject.Code,
        Name = scrumProject.Name,
        Description = scrumProject.Description,
        Color = scrumProject.Color,
        IsCommit = scrumProject.IsCommit,
        IsDelete = scrumProject.IsDelete,
        DepartmentId = department.Id,
        DepartmentName = department.Name,
        EstimatedTime = scrumProject.EstimatedTime,
        ScrumProjectGroupId = scrumProject.ScrumProjectGroupId,
        ScrumProjectGroupName = scrumProjectGroup.Name,
        RowVersion = scrumProject.RowVersion,
      };
      return result;
    }
    public ScrumProject ArchiveScrumProject(byte[] rowVersion, int id)
    {
      return EditScrumProject(id: id, rowVersion: rowVersion, isDelete: true);
    }
    public ScrumProject RestoreScrumProject(byte[] rowVersion, int id)
    {
      return EditScrumProject(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}
