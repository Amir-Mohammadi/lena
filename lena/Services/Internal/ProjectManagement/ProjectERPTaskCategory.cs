using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPTaskCategory;
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
    public ProjectERPTaskCategory GetProjectERPTaskCategory(int id) => GetProjectERPTaskCategory(selector: e => e, id: id);
    public TResult GetProjectERPTaskCategory<TResult>(
        Expression<Func<ProjectERPTaskCategory, TResult>> selector,
        int id)
    {

      var projectERPTaskCategory = GetProjectERPTaskCategories(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPTaskCategory == null)
        throw new ProjectERPTaskCategoryNotFoundException(id: id);
      return projectERPTaskCategory;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPTaskCategories<TResult>(
        Expression<Func<ProjectERPTaskCategory, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<string> description = null
    )
    {

      var query = repository.GetQuery<ProjectERPTaskCategory>();
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

    #region AddProjectERPTaskCategory

    public ProjectERPTaskCategory AddProjectERPTaskCategory(
        string name,
        bool isActive,
        string description)
    {

      var projectERPTaskCategory = repository.Create<ProjectERPTaskCategory>();
      projectERPTaskCategory.Name = name;
      projectERPTaskCategory.IsActive = isActive;
      projectERPTaskCategory.Description = description;
      projectERPTaskCategory.CreateDateTime = DateTime.UtcNow;
      repository.Add(projectERPTaskCategory);
      return projectERPTaskCategory;
    }

    #endregion

    #region EditProjectERPTaskCategory

    public ProjectERPTaskCategory EditProjectERPTaskCategory(
     int id,
     byte[] rowVersion,
     TValue<string> name = null,
     TValue<bool> isActive = null,
     TValue<string> description = null)
    {

      var projectERPTaskCategory = GetProjectERPTaskCategory(id: id);
      if (name != null)
        projectERPTaskCategory.Name = name;
      if (isActive != null)
        projectERPTaskCategory.IsActive = isActive;
      if (description != null)
        projectERPTaskCategory.Description = description;

      repository.Update(entity: projectERPTaskCategory, rowVersion: projectERPTaskCategory.RowVersion);
      return projectERPTaskCategory;
    }
    #endregion

    #region DeleteProjectERPTaskCategory
    public void DeleteProjectERPTaskCategory(int id)
    {

      var projectERPTaskCategory = GetProjectERPTaskCategory(id: id);
      repository.Delete(projectERPTaskCategory);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPTaskCategoryResult> SortProjectERPTaskCategoryResult(
        IQueryable<ProjectERPTaskCategoryResult> query, SortInput<ProjectERPTaskCategorySortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPTaskCategorySortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPTaskCategoryResult> SearchProjectERPTaskCategoryResult(
        IQueryable<ProjectERPTaskCategoryResult> query,
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
    public IQueryable<ProjectERPTaskCategoryComboResult> ToProjectERPTaskCategoryComboResultQuery(IQueryable<ProjectERPTaskCategory> query)
    {
      var result = from projectERPTaskCategory in query
                   select new ProjectERPTaskCategoryComboResult()
                   {
                     Name = projectERPTaskCategory.Name,
                     IsActive = projectERPTaskCategory.IsActive,
                     Description = projectERPTaskCategory.Description
                   };
      return result;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPTaskCategory, ProjectERPTaskCategoryResult>> ToProjectERPTaskCategoryResult =
         projectERPTaskCategory => new ProjectERPTaskCategoryResult()
         {
           Id = projectERPTaskCategory.Id,
           Name = projectERPTaskCategory.Name,
           IsActive = projectERPTaskCategory.IsActive,
           Description = projectERPTaskCategory.Description,
           RowVersion = projectERPTaskCategory.RowVersion
         };
    #endregion
  }
}
