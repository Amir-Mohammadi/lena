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
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.ScrumManagement.ScrumSprint;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement
{
  public partial class ScrumManagement
  {
    public ScrumSprint AddScrumSprint(ScrumSprint scrumSprint,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int scrumProjectId,
        int? baseEntityId)
    {


      scrumSprint = scrumSprint ?? repository.Create<ScrumSprint>();
      scrumSprint.ScrumProjectId = scrumProjectId;
      var retValue = AddScrumEntity(scrumEntity: scrumSprint,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit,
                baseEntityId: baseEntityId
                );
      return retValue as ScrumSprint;
    }
    public ScrumSprint EditScrumSprint(
        byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> scrumProjectId = null
        )
    {

      var scrumSprint = GetScrumSprint(id: id);
      return
                EditScrumSprint(
                        rowVersion: rowVersion,
                        scrumSprint: scrumSprint,
                        code: code,
                        name: name,
                        description: description,
                        color: color,
                        departmentId: departmentId,
                        estimatedTime: estimatedTime,
                        isCommit: isCommit,
                        isDelete: isDelete,
                        scrumProjectId: scrumProjectId);
    }
    public ScrumSprint EditScrumSprint(
        byte[] rowVersion,
        ScrumSprint scrumSprint,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> scrumProjectId = null)
    {


      if (scrumProjectId != null)
        scrumSprint.ScrumProjectId = scrumProjectId;
      var retValue = EditScrumEntity(rowVersion: rowVersion,
                    scrumEntity: scrumSprint,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete);
      return retValue as ScrumSprint;
    }
    public IQueryable<ScrumSprint> GetScrumSprints(
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
        TValue<int> scrumProjectId = null,
        TValue<int?> baseEntityId = null)
    {

      var isScrumProjectIdNull = scrumProjectId == null;
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
                isArchive: isArchive,
                baseEntityId: baseEntityId);
      var scrumSprintQuery = from item in scrumEntitiesQuery.OfType<ScrumSprint>()
                             where (isScrumProjectIdNull || item.ScrumProjectId == scrumProjectId)
                             select item;
      return scrumSprintQuery;
    }
    public ScrumSprint GetScrumSprint(int id)
    {

      var scrumSprint = GetScrumSprints(id: id).SingleOrDefault();
      if (scrumSprint == null)
        throw new ScrumSprintNotFoundException(id);
      return scrumSprint;
    }
    public void DeleteScrumSprint(int id)
    {

      var scrumSprint = GetScrumSprint(id);
      repository.Delete(scrumSprint);
    }
    public IQueryable<ScrumSprintResult> SearchScrumSprintResult(IQueryable<ScrumSprintResult> query, string search)
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
    public IOrderedQueryable<ScrumSprintResult> SortScrumSprintResult(IQueryable<ScrumSprintResult> query, SortInput<ScrumSprintSortType> sort)
    {
      switch (sort.SortType)
      {
        case ScrumSprintSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ScrumSprintSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ScrumSprintSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ScrumSprintSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        case ScrumSprintSortType.ScrumProjectName:
          return query.OrderBy(a => a.ScrumProjectName, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ScrumSprintResult> ToScrumSprintResultQuery(IQueryable<ScrumSprint> query)
    {
      var resultQuery = from scrumSprint in query
                        let department = scrumSprint.Department
                        let scrumProject = scrumSprint.ScrumProject
                        select new ScrumSprintResult
                        {
                          Id = scrumSprint.Id,
                          Code = scrumSprint.Code,
                          Name = scrumSprint.Name,
                          Description = scrumSprint.Description,
                          Color = scrumSprint.Color,
                          IsCommit = scrumSprint.IsCommit,
                          IsDelete = scrumSprint.IsDelete,
                          DepartmentId = department.Id,
                          DepartmentName = department.Name,
                          ScrumProjectId = scrumSprint.ScrumProjectId,
                          ScrumProjectName = scrumProject.Name,
                          EstimatedTime = scrumSprint.EstimatedTime,
                          RowVersion = scrumSprint.RowVersion,
                        };
      return resultQuery;
    }
    public ScrumSprintResult ToScrumSprintResult(ScrumSprint scrumSprint)
    {
      var department = scrumSprint.Department;
      var scrumProject = scrumSprint.ScrumProject;
      var result = new ScrumSprintResult
      {
        Id = scrumSprint.Id,
        Code = scrumSprint.Code,
        Name = scrumSprint.Name,
        Description = scrumSprint.Description,
        Color = scrumSprint.Color,
        IsCommit = scrumSprint.IsCommit,
        IsDelete = scrumSprint.IsDelete,
        DepartmentId = department.Id,
        DepartmentName = department.Name,
        EstimatedTime = scrumSprint.EstimatedTime,
        ScrumProjectId = scrumSprint.ScrumProjectId,
        ScrumProjectName = scrumProject.Name,
        RowVersion = scrumSprint.RowVersion,
      };
      return result;
    }
    public ScrumSprint ArchiveScrumSprint(byte[] rowVersion, int id)
    {
      return EditScrumSprint(id: id, rowVersion: rowVersion, isDelete: true);
    }
    public ScrumSprint RestoreScrumSprint(byte[] rowVersion, int id)
    {
      return EditScrumSprint(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}
