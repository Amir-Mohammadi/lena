using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityAssurance.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityAssurance.EvaluationCategory;
using lena.Models.QualityAssurance.EvaluationCategoryItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Add
    public EvaluationCategory AddEvaluationCategory(
        string title,
        short? departmentId)
    {

      var evaluationCategory = repository.Create<EvaluationCategory>();
      evaluationCategory.Title = title;
      evaluationCategory.DepartmentId = departmentId;
      repository.Add(evaluationCategory);
      return evaluationCategory;
    }
    #endregion


    #region AddProcess
    public void AddEvaluationCategoryProcess(
        string title,
        short? departmentId,
        AddEvaluationCategoryItemInput[] addEvaluationCategoryItemInput)
    {

      #region AddEvaluationCategory
      var evaluationCategory = AddEvaluationCategory(
          title: title,
          departmentId: departmentId);
      #endregion

      #region AddEvaluationCategoryItem

      foreach (var item in addEvaluationCategoryItemInput)
      {
        AddEvaluationCategoryItem(evaluationCategoryId: evaluationCategory.Id, question: item.Question);
      }
      #endregion

    }
    #endregion


    #region EditProcess
    public void EditEvaluationCategoryProcess(
        int id,
        byte[] rowVersion,
        string title,
        short? departmentId,
        AddEvaluationCategoryItemInput[] addEvaluationCategoryItemInput,
        EditEvaluationCategoryItemInput[] EditEvaluationCategoryItemInput,
        int[] evaluationCategoryItemDeleteIds)
    {

      #region GetEvaluationCategory
      var evaluationCategory = GetEvaluationCategory(id: id);
      #endregion

      #region EditEvaluationCategory
      EditEvaluationCategory(
          evaluationCategory: evaluationCategory,
          rowVersion: rowVersion,
          title: title,
          departmentId: departmentId);
      #endregion

      var items = evaluationCategory.EvaluationCategoryItems.ToList();
      #region AddEvaluationCategoryItem

      foreach (var item in addEvaluationCategoryItemInput)
      {
        AddEvaluationCategoryItem(evaluationCategoryId: evaluationCategory.Id, question: item.Question);
      }




      foreach (var item in EditEvaluationCategoryItemInput)
      {
        var evaluationCategoryItem = items.FirstOrDefault(i => i.Id == item.Id);

        if (evaluationCategoryItem != null)
        {
          if (item.IsArchive != evaluationCategoryItem.IsArchive || item.Question != evaluationCategoryItem.Question)
            EditEvaluationCategoryItem(evaluationCategoryItem: evaluationCategoryItem, rowVersion: item.RowVersion, question: item.Question, isArchive: item.IsArchive);
        }

      }

      foreach (var item in evaluationCategoryItemDeleteIds)
      {
        var evaluationCategoryItem = items.FirstOrDefault(i => i.Id == item);
        if (evaluationCategoryItem != null)
        {
          DeleteEvaluationCategoryItem(evaluationCategoryItem: evaluationCategoryItem);
        }

      }


      #endregion

    }
    #endregion


    #region Edit

    public EvaluationCategory EditEvaluationCategory(
      int id,
      byte[] rowVersion,
      TValue<string> title = null,
      TValue<short?> departmentId = null)
    {

      var evaluationCategory = GetEvaluationCategory(id: id);
      return EditEvaluationCategory(
                evaluationCategory: evaluationCategory,
                rowVersion: rowVersion,
                title: title,
                departmentId: departmentId);
    }
    public EvaluationCategory EditEvaluationCategory(
        EvaluationCategory evaluationCategory,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<short?> departmentId = null)
    {

      evaluationCategory.DepartmentId = departmentId;

      if (title != null)
        evaluationCategory.Title = title;




      repository.Update(evaluationCategory, rowVersion);
      return evaluationCategory;
    }
    #endregion


    #region Get
    public EvaluationCategory GetEvaluationCategory(int id) => GetEvaluationCategory(selector: e => e, id: id);
    public TResult GetEvaluationCategory<TResult>(
        Expression<Func<EvaluationCategory, TResult>> selector,
        int id)
    {

      var evaluationCategory = GetEvaluationCategories(
                selector: selector,
                id: id).FirstOrDefault();
      if (evaluationCategory == null)
        throw new RecordNotFoundException(id, typeof(EvaluationCategory));
      return evaluationCategory;
    }
    #endregion


    #region Gets
    public IQueryable<TResult> GetEvaluationCategories<TResult>(
        Expression<Func<EvaluationCategory, TResult>> selector,
        TValue<int> id = null,
         TValue<int[]> ids = null,
        TValue<int?> departmentId = null)
    {

      var evaluationCategories = repository.GetQuery<EvaluationCategory>();
      if (id != null)
        evaluationCategories = evaluationCategories.Where(x => x.Id == id);
      if (ids != null)
        evaluationCategories = evaluationCategories.Where(x => ids.Value.Contains(x.Id));
      if (departmentId != null)
        evaluationCategories = evaluationCategories.Where(x => x.DepartmentId == departmentId);

      return evaluationCategories.Select(selector);
    }
    #endregion


    #region Delete
    public void DeleteEvaluationCategoryProcess(int id)
    {

      var evaluationCategory = GetEvaluationCategory(id: id);

      if (evaluationCategory.EvaluationCategoryItems.Count == 0)
        DeleteEvaluationCategory(evaluationCategory: evaluationCategory);
      else
        throw new CannotDeleteEvaluationCategoryException(id: id);
    }

    public void DeleteEvaluationCategory(EvaluationCategory evaluationCategory)
    {

      repository.Delete(evaluationCategory);
    }
    #endregion


    #region ToResult
    public Expression<Func<EvaluationCategory, EvaluationCategoryResult>> ToEvaluationCategoryResult =
        evaluationCategory => new EvaluationCategoryResult
        {
          Id = evaluationCategory.Id,
          Title = evaluationCategory.Title,
          DepartmentId = evaluationCategory.DepartmentId,
          DepartmentName = evaluationCategory.Department.Name,
          RowVersion = evaluationCategory.RowVersion
        };
    #endregion


    #region Search
    public IQueryable<EvaluationCategoryResult> SearchEvaluationCategory(IQueryable<EvaluationCategoryResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems

        )
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Title.Contains(searchText) ||
            item.DepartmentName.Contains(searchText));

      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion


    #region Sort
    public IOrderedQueryable<EvaluationCategoryResult> SortEvaluationCategoryResult(IQueryable<EvaluationCategoryResult> query,
        SortInput<EvaluationCategorySortType> sort)
    {
      switch (sort.SortType)
      {
        case EvaluationCategorySortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case EvaluationCategorySortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case EvaluationCategorySortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region GetValidEvaluationCategories
    public IQueryable<EvaluationCategoryResult> GetValidEvaluationCategories(int employeeEvaluationPeriodId)
    {

      var evaluationCategoryIds = GetEmployeeEvaluationPeriodItems(
                e => e.EvaluationCategoryId,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId)

                .ToList()
                .ToArray();

      int? departmentId = App.Providers.Security.CurrentLoginData.DepartmentId;

      var validEvaluationCategories = GetEvaluationCategories(selector: e => e, ids: evaluationCategoryIds);

      validEvaluationCategories = validEvaluationCategories.Where(m => m.DepartmentId == departmentId || m.DepartmentId == null);

      return from validEvaluationCategory in validEvaluationCategories
             select new EvaluationCategoryResult
             {
               Id = validEvaluationCategory.Id,
               Title = validEvaluationCategory.Title,
               DepartmentId = validEvaluationCategory.DepartmentId,
               DepartmentName = validEvaluationCategory.Department.Name,
               RowVersion = validEvaluationCategory.RowVersion
             };

    }
    #endregion
  }
}
