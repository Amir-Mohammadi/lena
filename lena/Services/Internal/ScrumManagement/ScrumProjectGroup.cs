using lena.Models.Common;
using lena.Domains;
using System;
using System.Globalization;
using System.Linq;
using lena.Models;
using lena.Domains.Enums;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ScrumManagement.Exception;
using lena.Models.Common;
using lena.Models.ScrumManagement.ScrumProjectGroup;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement
{
  public partial class ScrumManagement
  {
    #region Get Or Add
    public ScrumProjectGroup GetOrAddCommonScrumProjectGroup(short departmentId)
    {
      var scrumProjectGroup = repository.Create<ScrumProjectGroup>();
      #region Get ScrumProjectGroup
      scrumProjectGroup.DepartmentId = departmentId;
      GenerateCode(scrumProjectGroup);
      scrumProjectGroup = GetScrumProjectGroups(code: scrumProjectGroup.Code)
               .FirstOrDefault();
      var department = App.Internals.ApplicationBase.GetDepartment(departmentId);
      #endregion
      #region  Add ScrumProjectGroup
      if (scrumProjectGroup == null)
      {
        scrumProjectGroup = AddScrumProjectGroup(
                      scrumProjectGroup: null,
                      name: $"فعالیت کلی {department.Name}",
                      description: "",
                      color: "",
                      departmentId: departmentId,
                      estimatedTime: 0,
                      isCommit: false
                  );
      }
      #endregion
      return scrumProjectGroup;
    }
    #endregion
    public ScrumProjectGroup AddScrumProjectGroup(ScrumProjectGroup scrumProjectGroup,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit)
    {
      scrumProjectGroup = scrumProjectGroup ?? repository.Create<ScrumProjectGroup>();
      var retValue = AddScrumEntity(scrumEntity: scrumProjectGroup,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit,
                baseEntityId: null
                );
      return retValue as ScrumProjectGroup;
    }
    public ScrumProjectGroup EditScrumProjectGroup(byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null)
    {
      var scrumProjectGroup = GetScrumProjectGroup(id: id);
      return
                EditScrumProjectGroup(
                        scrumProjectGroup: scrumProjectGroup,
                        rowVersion: rowVersion,
                        code: code,
                        name: name,
                        description: description,
                        color: color,
                        departmentId: departmentId,
                        estimatedTime: estimatedTime,
                        isCommit: isCommit,
                        isDelete: isDelete);
    }
    public ScrumProjectGroup EditScrumProjectGroup(ScrumProjectGroup scrumProjectGroup,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null)
    {
      var retValue = EditScrumEntity(rowVersion: rowVersion,
                    scrumEntity: scrumProjectGroup,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete);
      return retValue as ScrumProjectGroup;
    }
    public IQueryable<ScrumProjectGroup> GetScrumProjectGroups(
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<int> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null
        )
    {
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
      var scrumProjectGroupQuery = from item in scrumEntitiesQuery.OfType<ScrumProjectGroup>()
                                   select item;
      return scrumProjectGroupQuery;
    }
    public ScrumProjectGroup GetScrumProjectGroup(int id)
    {
      var scrumProjectGroup = GetScrumProjectGroups(id: id).SingleOrDefault();
      if (scrumProjectGroup == null)
        throw new ScrumProjectGroupNotFoundException(id);
      return scrumProjectGroup;
    }
    public void DeleteScrumProjectGroup(int id)
    {
      var scrumProjectGroup = GetScrumProjectGroup(id);
      repository.Delete(scrumProjectGroup);
    }
    public IQueryable<ScrumProjectGroupResult> SearchScrumProjectGroupResult(IQueryable<ScrumProjectGroupResult> query, string search)
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
    public IOrderedQueryable<ScrumProjectGroupResult> SortScrumProjectGroupResult(IQueryable<ScrumProjectGroupResult> query, SortInput<ScrumProjectGroupSortType> sort)
    {
      switch (sort.SortType)
      {
        case ScrumProjectGroupSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ScrumProjectGroupSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ScrumProjectGroupSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ScrumProjectGroupSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ScrumProjectGroupResult> ToScrumProjectGroupResultQuery(IQueryable<ScrumProjectGroup> query)
    {
      var resultQuery = from scrumProjectGroup in query
                        let department = scrumProjectGroup.Department
                        select new ScrumProjectGroupResult
                        {
                          Id = scrumProjectGroup.Id,
                          Code = scrumProjectGroup.Code,
                          Name = scrumProjectGroup.Name,
                          Description = scrumProjectGroup.Description,
                          Color = scrumProjectGroup.Color,
                          IsCommit = scrumProjectGroup.IsCommit,
                          IsDelete = scrumProjectGroup.IsDelete,
                          DepartmentId = department.Id,
                          DepartmentName = department.Name,
                          EstimatedTime = scrumProjectGroup.EstimatedTime,
                          RowVersion = scrumProjectGroup.RowVersion,
                        };
      return resultQuery;
    }
    public ScrumProjectGroupResult ToScrumProjectGroupResult(ScrumProjectGroup scrumProjectGroup)
    {
      var department = scrumProjectGroup.Department;
      var result = new ScrumProjectGroupResult
      {
        Id = scrumProjectGroup.Id,
        Code = scrumProjectGroup.Code,
        Name = scrumProjectGroup.Name,
        Description = scrumProjectGroup.Description,
        Color = scrumProjectGroup.Color,
        IsCommit = scrumProjectGroup.IsCommit,
        IsDelete = scrumProjectGroup.IsDelete,
        DepartmentId = department.Id,
        DepartmentName = department.Name,
        EstimatedTime = scrumProjectGroup.EstimatedTime,
        RowVersion = scrumProjectGroup.RowVersion,
      };
      return result;
    }
    public ScrumProjectGroup ArchiveScrumProjectGroup(byte[] rowVersion, int id)
    {
      return EditScrumProjectGroup(id: id, rowVersion: rowVersion, isDelete: true);
    }
    public ScrumProjectGroup RestoreScrumProjectGroup(byte[] rowVersion, int id)
    {
      return EditScrumProjectGroup(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}