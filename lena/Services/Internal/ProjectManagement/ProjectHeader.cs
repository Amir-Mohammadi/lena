using System;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectHeader;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement
{
  public partial class ProjectManagement
  {
    public ProjectHeader AddProjectHeader(
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int ownerCustomerId,
        int? stuffId)
    {
      var projectHeader = repository.Create<ProjectHeader>();
      projectHeader.OwnerCustomerId = ownerCustomerId;
      if (stuffId != null)
        projectHeader.Stuff = App.Internals.SaleManagement.GetStuff(stuffId.Value);
      var retValue = App.Internals.ScrumManagement.AddScrumProjectGroup(
                scrumProjectGroup: projectHeader,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit
            );
      return retValue as ProjectHeader;
    }
    public ProjectHeader EditProjectHeader(byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> ownerCustomerId = null,
        TValue<int?> stuffId = null
    )
    {
      var projectHeader = GetProjectHeader(id: id);
      return EditProjectHeader(
                    projectHeader: projectHeader,
                    rowVersion: rowVersion,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete,
                    ownerCustomerId: ownerCustomerId,
                    stuffId: stuffId);
    }
    public ProjectHeader EditProjectHeader(
        ProjectHeader projectHeader,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> ownerCustomerId = null,
        TValue<int?> stuffId = null)
    {
      if (ownerCustomerId != null)
        projectHeader.OwnerCustomerId = ownerCustomerId;
      if (stuffId != null)
      {
        if (stuffId.Value != null)
          projectHeader.Stuff = App.Internals.SaleManagement.GetStuff(stuffId.Value.Value);
        else if (projectHeader.Stuff != null)
        {
          projectHeader.Stuff.ProjectHeader = null;
          projectHeader.Stuff = null;
        }
      }
      var retValue = App.Internals.ScrumManagement.EditScrumProjectGroup(
                    rowVersion: rowVersion,
                    scrumProjectGroup: projectHeader,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete);
      return retValue as ProjectHeader;
    }
    public IQueryable<ProjectHeader> GetProjectHeaders(
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
        TValue<int> ownerCustomerId = null,
        TValue<int> stuffId = null
    )
    {
      var isOwnerCustomerIdNull = ownerCustomerId == null;
      var isStuffIdNull = stuffId == null;
      var scrumEntitiesQuery = App.Internals.ScrumManagement.GetScrumEntities(
                id: id,
                code: code,
                name: name,
                description: description,
                color: color,
                isCommit: isCommit,
                estimatedTime: estimatedTime,
                departmentId: departmentId,
                isDelete: isDelete,
                isArchive: isArchive);
      var projectHeaderQuery = from item in scrumEntitiesQuery.OfType<ProjectHeader>()
                               where (isOwnerCustomerIdNull || item.OwnerCustomerId == ownerCustomerId) &&
                                         (isStuffIdNull || item.Stuff.Id == stuffId)
                               select item;
      return projectHeaderQuery;
    }
    public ProjectHeader GetProjectHeader(int id)
    {
      var projectHeader = GetProjectHeaders(id: id).SingleOrDefault();
      if (projectHeader == null)
        throw new ProjectHeaderNotFoundException(id);
      return projectHeader;
    }
    public void DeleteProjectHeader(int id)
    {
      var projectHeader = GetProjectHeader(id);
      repository.Delete(projectHeader);
    }
    public IQueryable<ProjectHeaderResult> SearchProjectHeader(IQueryable<ProjectHeaderResult> query, string search)
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
    public IOrderedQueryable<ProjectHeaderResult> SortProjectHeaderResult(IQueryable<ProjectHeaderResult> query, SortInput<ProjectHeaderSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProjectHeaderSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ProjectHeaderSortType.Color:
          return query.OrderBy(a => a.Color, sort.SortOrder);
        case ProjectHeaderSortType.IsDelete:
          return query.OrderBy(a => a.IsDelete, sort.SortOrder);
        case ProjectHeaderSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ProjectHeaderSortType.OwnerCustomerCode:
          return query.OrderBy(a => a.OwnerCustomerCode, sort.SortOrder);
        case ProjectHeaderSortType.OwnerCustomerName:
          return query.OrderBy(a => a.OwnerCustomerName, sort.SortOrder);
        case ProjectHeaderSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ProjectHeaderSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ProjectHeaderSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ProjectHeaderSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ProjectHeaderResult> ToProjectHeaderResultQuery(IQueryable<ProjectHeader> query)
    {
      var resultQuery = from projectHeader in query
                        let department = projectHeader.Department
                        let ownerCustomer = projectHeader.OwnerCustomer
                        let stuff = projectHeader.Stuff
                        select new ProjectHeaderResult
                        {
                          Id = projectHeader.Id,
                          Code = projectHeader.Code,
                          Name = projectHeader.Name,
                          Description = projectHeader.Description,
                          Color = projectHeader.Color,
                          IsCommit = projectHeader.IsCommit,
                          IsDelete = projectHeader.IsDelete,
                          DepartmentId = department.Id,
                          DepartmentName = department.Name,
                          EstimatedTime = projectHeader.EstimatedTime,
                          OwnerCustomerId = ownerCustomer.Id,
                          OwnerCustomerCode = ownerCustomer.Code,
                          OwnerCustomerName = ownerCustomer.Name,
                          StuffId = stuff.Id,
                          StuffName = stuff.Name,
                          StuffCode = stuff.Code,
                          RowVersion = projectHeader.RowVersion,
                        };
      return resultQuery;
    }
    public IQueryable<ProjectHeaderResult> SearchProjectHeaderResult(IQueryable<ProjectHeaderResult> query, string searchText
        , AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from projectHeader in query
                where projectHeader.Code.Contains(searchText) ||
                    projectHeader.Name.Contains(searchText) ||
                    projectHeader.OwnerCustomerName.Contains(searchText) ||
                    projectHeader.OwnerCustomerCode.Contains(searchText) ||
                    projectHeader.StuffName.Contains(searchText) ||
                    projectHeader.StuffCode.Contains(searchText) ||
                    projectHeader.DepartmentName.Contains(searchText) ||
                    projectHeader.Description.Contains(searchText)
                select projectHeader;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public ProjectHeaderResult ToProjectHeaderResult(ProjectHeader projectHeader)
    {
      var department = projectHeader.Department;
      var ownerCustomer = projectHeader.OwnerCustomer;
      var stuff = projectHeader.Stuff;
      var result = new ProjectHeaderResult
      {
        Id = projectHeader.Id,
        Code = projectHeader.Code,
        Name = projectHeader.Name,
        Description = projectHeader.Description,
        Color = projectHeader.Color,
        IsCommit = projectHeader.IsCommit,
        IsDelete = projectHeader.IsDelete,
        DepartmentId = department.Id,
        DepartmentName = department.Name,
        EstimatedTime = projectHeader.EstimatedTime,
        OwnerCustomerId = projectHeader.OwnerCustomerId,
        OwnerCustomerName = ownerCustomer.Name,
        StuffId = stuff?.Id,
        StuffName = stuff?.Name,
        StuffCode = stuff?.Code,
        RowVersion = projectHeader.RowVersion,
      };
      return result;
    }
    public ProjectHeader ArchiveProjectHeader(byte[] rowVersion, int id)
    {
      return EditProjectHeader(id: id, rowVersion: rowVersion, isDelete: true);
    }
    public ProjectHeader RestoreProjectHeader(byte[] rowVersion, int id)
    {
      return EditProjectHeader(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}