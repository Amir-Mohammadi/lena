using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPLabel;
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
    public ProjectERPLabel GetProjectERPLabel(int id) => GetProjectERPLabel(selector: e => e, id: id);
    public TResult GetProjectERPLabel<TResult>(
        Expression<Func<ProjectERPLabel, TResult>> selector,
        int id)
    {

      var projectERPLabel = GetProjectERPLabels(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPLabel == null)
        throw new ProjectERPLabelNotFoundException(id: id);
      return projectERPLabel;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPLabels<TResult>(
        Expression<Func<ProjectERPLabel, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<string> color = null,
        TValue<string> description = null)
    {

      var query = repository.GetQuery<ProjectERPLabel>();
      if (name != null)
        query = query.Where(m => m.Name == name);
      if (isActive != null)
        query = query.Where(m => m.IsActive == isActive);
      if (color != null)
        query = query.Where(m => m.Color == color);
      if (description != null)
        query = query.Where(m => m.Description == description);

      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPLabel
    public ProjectERPLabel AddProjectERPLabel(
        string name,
        bool isActive,
        string color,
        string description)
    {

      var projectERPLabel = repository.Create<ProjectERPLabel>();
      projectERPLabel.Name = name;
      projectERPLabel.IsActive = isActive;
      projectERPLabel.Color = color;
      projectERPLabel.Description = description;

      repository.Add(projectERPLabel);
      return projectERPLabel;
    }

    #endregion

    #region EditProjectERPLabel

    public ProjectERPLabel EditProjectERPLabel(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<string> color = null,
        TValue<string> description = null)
    {

      var projectERPLabel = GetProjectERPLabel(id: id);
      if (name != null)
        projectERPLabel.Name = name;
      if (isActive != null)
        projectERPLabel.IsActive = isActive;
      if (color != null)
        projectERPLabel.Color = color;
      if (description != null)
        projectERPLabel.Description = description;

      repository.Update(entity: projectERPLabel, rowVersion: projectERPLabel.RowVersion);
      return projectERPLabel;
    }
    #endregion

    #region DeleteProjectERPLabel
    public void DeleteProjectERPLabel(int id)
    {

      var projectERPLabel = GetProjectERPLabel(id: id);
      repository.Delete(projectERPLabel);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPLabelResult> SortProjectERPLabelResult(
        IQueryable<ProjectERPLabelResult> query, SortInput<ProjectERPLabelSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPLabelSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case ProjectERPLabelSortType.Name:
          return query.OrderBy(a => a.Name, type.SortOrder);
        case ProjectERPLabelSortType.IsActive:
          return query.OrderBy(a => a.IsActive, type.SortOrder);
        case ProjectERPLabelSortType.Description:
          return query.OrderBy(a => a.Description, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPLabelResult> SearchProjectERPLabelResult(
        IQueryable<ProjectERPLabelResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Name.Contains(searchText) ||
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
    public Expression<Func<ProjectERPLabel, ProjectERPLabelResult>> ToProjectERPLabelResult =
         projectERPLabel => new ProjectERPLabelResult()
         {
           Id = projectERPLabel.Id,
           Name = projectERPLabel.Name,
           IsActive = projectERPLabel.IsActive,
           Color = projectERPLabel.Color,
           Description = projectERPLabel.Description,
           RowVersion = projectERPLabel.RowVersion
         };
    #endregion
  }
}
