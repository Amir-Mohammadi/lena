using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPDocumentType;
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
    public ProjectERPDocumentType GetProjectERPDocumentType(int id) => GetProjectERPDocumentType(selector: e => e, id: id);
    public TResult GetProjectERPDocumentType<TResult>(
        Expression<Func<ProjectERPDocumentType, TResult>> selector,
        int id)
    {

      var projectERPDocumentType = GetProjectERPDocumentTypes(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPDocumentType == null)
        throw new ProjectERPDocumentTypeNotFoundException(id: id);
      return projectERPDocumentType;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPDocumentTypes<TResult>(
        Expression<Func<ProjectERPDocumentType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<string> description = null
    )
    {

      var query = repository.GetQuery<ProjectERPDocumentType>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (description != null)
        query = query.Where(m => m.Description == description);
      if (isActive != null)
        query = query.Where(m => m.IsActive == isActive);
      if (name != null)
        query = query.Where(m => m.Name == name);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPDocumentType

    public ProjectERPDocumentType AddProjectERPDocumentType(
        string name,
        bool isActive,
        string description)
    {

      var projectERPDocumentType = repository.Create<ProjectERPDocumentType>();
      projectERPDocumentType.Name = name;
      projectERPDocumentType.IsActive = isActive;
      projectERPDocumentType.Description = description;
      projectERPDocumentType.CreateDateTime = DateTime.UtcNow;
      repository.Add(projectERPDocumentType);
      return projectERPDocumentType;
    }

    #endregion

    #region EditProjectERPDocumentType

    public ProjectERPDocumentType EditProjectERPDocumentType(
     int id,
     byte[] rowVersion,
     TValue<string> name = null,
     TValue<bool> isActive = null,
     TValue<string> description = null)
    {

      var projectERPDocumentType = GetProjectERPDocumentType(id: id);
      if (name != null)
        projectERPDocumentType.Name = name;
      if (isActive != null)
        projectERPDocumentType.IsActive = isActive;
      if (description != null)
        projectERPDocumentType.Description = description;

      repository.Update(entity: projectERPDocumentType, rowVersion: projectERPDocumentType.RowVersion);
      return projectERPDocumentType;
    }
    #endregion

    #region DeleteProjectERPDocumentType
    public void DeleteProjectERPDocumentType(int id)
    {

      var projectERPDocumentType = GetProjectERPDocumentType(id: id);
      repository.Delete(projectERPDocumentType);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPDocumentTypeResult> SortProjectERPDocumentTypeResult(
        IQueryable<ProjectERPDocumentTypeResult> query, SortInput<ProjectERPDocumentTypeSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPDocumentTypeSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPDocumentTypeResult> SearchProjectERPDocumentTypeResult(
        IQueryable<ProjectERPDocumentTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                    item.Name.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPDocumentType, ProjectERPDocumentTypeResult>> ToProjectERPDocumentTypeResult =
         projectERPDocumentType => new ProjectERPDocumentTypeResult()
         {
           Id = projectERPDocumentType.Id,
           Name = projectERPDocumentType.Name,
           IsActive = projectERPDocumentType.IsActive,
           Description = projectERPDocumentType.Description,
           RowVersion = projectERPDocumentType.RowVersion
         };
    #endregion
  }
}
