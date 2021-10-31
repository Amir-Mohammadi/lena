using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPEventCategory;
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
    public ProjectERPEventCategory GetProjectERPEventCategory(int id) => GetProjectERPEventCategory(selector: e => e, id: id);
    public TResult GetProjectERPEventCategory<TResult>(
        Expression<Func<ProjectERPEventCategory, TResult>> selector,
        int id)
    {

      var projectERPEventCategory = GetProjectERPEventCategories(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPEventCategory == null)
        throw new ProjectERPEventCategoryNotFoundException(id: id);
      return projectERPEventCategory;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPEventCategories<TResult>(
        Expression<Func<ProjectERPEventCategory, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<string> description = null
    )
    {

      var query = repository.GetQuery<ProjectERPEventCategory>();
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

    #region AddProjectERPEventCategory

    public ProjectERPEventCategory AddProjectERPEventCategory(
        string name,
        bool isActive,
        string description)
    {

      var projectERPEventCategory = repository.Create<ProjectERPEventCategory>();
      projectERPEventCategory.Name = name;
      projectERPEventCategory.IsActive = isActive;
      projectERPEventCategory.Description = description;
      repository.Add(projectERPEventCategory);
      return projectERPEventCategory;
    }

    #endregion

    #region EditProjectERPEventCategory

    public ProjectERPEventCategory EditProjectERPEventCategory(
     int id,
     byte[] rowVersion,
     TValue<string> name = null,
     TValue<bool> isActive = null,
     TValue<string> description = null)
    {

      var projectERPEventCategory = GetProjectERPEventCategory(id: id);
      if (name != null)
        projectERPEventCategory.Name = name;
      if (isActive != null)
        projectERPEventCategory.IsActive = isActive;
      if (description != null)
        projectERPEventCategory.Description = description;

      repository.Update(entity: projectERPEventCategory, rowVersion: projectERPEventCategory.RowVersion);
      return projectERPEventCategory;
    }
    #endregion

    #region DeleteProjectERPEventCategory
    public void DeleteProjectERPEventCategory(int id)
    {

      var projectERPEventCategory = GetProjectERPEventCategory(id: id);
      repository.Delete(projectERPEventCategory);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPEventCategoryResult> SortProjectERPEventCategoryResult(
        IQueryable<ProjectERPEventCategoryResult> query, SortInput<ProjectERPEventCategorySortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPEventCategorySortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPEventCategoryResult> SearchProjectERPEventCategoryResult(
        IQueryable<ProjectERPEventCategoryResult> query,
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

    #region ToComboResult
    public IQueryable<ProjectERPEventCategoryComboResult> ToProjectERPEventCategoryComboResultQuery(IQueryable<ProjectERPEventCategory> query)
    {
      var result = from projectERPEventCategory in query
                   select new ProjectERPEventCategoryComboResult()
                   {
                     Name = projectERPEventCategory.Name,
                     IsActive = projectERPEventCategory.IsActive,
                     Description = projectERPEventCategory.Description
                   };
      return result;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPEventCategory, ProjectERPEventCategoryResult>> ToProjectERPEventCategoryResult =
         projectERPEventCategory => new ProjectERPEventCategoryResult()
         {
           Id = projectERPEventCategory.Id,
           Name = projectERPEventCategory.Name,
           IsActive = projectERPEventCategory.IsActive,
           Description = projectERPEventCategory.Description,
           RowVersion = projectERPEventCategory.RowVersion
         };
    #endregion
  }
}
