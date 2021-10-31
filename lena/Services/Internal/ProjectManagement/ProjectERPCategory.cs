using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPCategory;
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
    public ProjectERPCategory GetProjectERPCategory(int id) => GetProjectERPCategory(selector: e => e, id: id);
    public TResult GetProjectERPCategory<TResult>(
        Expression<Func<ProjectERPCategory, TResult>> selector,
        int id)
    {

      var projectERPCategory = GetProjectERPCategories(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPCategory == null)
        throw new ProjectERPCategoryNotFoundException(id: id);
      return projectERPCategory;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPCategories<TResult>(
        Expression<Func<ProjectERPCategory, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<string> description = null
    )
    {

      var query = repository.GetQuery<ProjectERPCategory>();
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

    #region AddProjectERPCategory

    public ProjectERPCategory AddProjectERPCategory(
        string name,
        bool isActive,
        string description)
    {

      var projectERPCategory = repository.Create<ProjectERPCategory>();
      projectERPCategory.Name = name;
      projectERPCategory.IsActive = isActive;
      projectERPCategory.Description = description;
      projectERPCategory.CreateDateTime = DateTime.UtcNow;
      repository.Add(projectERPCategory);
      return projectERPCategory;
    }

    #endregion

    #region EditProjectERPCategory

    public ProjectERPCategory EditProjectERPCategory(
     int id,
     byte[] rowVersion,
     TValue<string> name = null,
     TValue<bool> isActive = null,
     TValue<string> description = null)
    {

      var projectERPCategory = GetProjectERPCategory(id: id);
      if (name != null)
        projectERPCategory.Name = name;
      if (isActive != null)
        projectERPCategory.IsActive = isActive;
      if (description != null)
        projectERPCategory.Description = description;

      repository.Update(entity: projectERPCategory, rowVersion: projectERPCategory.RowVersion);
      return projectERPCategory;
    }
    #endregion

    #region DeleteProjectERPCategory
    public void DeleteProjectERPCategory(int id)
    {

      var projectERPCategory = GetProjectERPCategory(id: id);
      repository.Delete(projectERPCategory);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPCategoryResult> SortProjectERPCategoryResult(
        IQueryable<ProjectERPCategoryResult> query, SortInput<ProjectERPCategorySortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPCategorySortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPCategoryResult> SearchProjectERPCategoryResult(
        IQueryable<ProjectERPCategoryResult> query,
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
    public IQueryable<ProjectERPCategoryComboResult> ToProjectERPCategoryComboResultQuery(IQueryable<ProjectERPCategory> query)
    {
      var result = from projectERPCategory in query
                   select new ProjectERPCategoryComboResult()
                   {
                     Name = projectERPCategory.Name,
                     IsActive = projectERPCategory.IsActive,
                     Description = projectERPCategory.Description
                   };
      return result;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPCategory, ProjectERPCategoryResult>> ToProjectERPCategoryResult =
         projectERPCategory => new ProjectERPCategoryResult()
         {
           Id = projectERPCategory.Id,
           Name = projectERPCategory.Name,
           IsActive = projectERPCategory.IsActive,
           Description = projectERPCategory.Description,
           RowVersion = projectERPCategory.RowVersion
         };
    #endregion
  }
}
