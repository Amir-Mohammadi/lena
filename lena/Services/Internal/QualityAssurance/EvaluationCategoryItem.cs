using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityAssurance.Exception;
using lena.Models.Common;
using lena.Domains;
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
    public EvaluationCategoryItem AddEvaluationCategoryItem(
        int evaluationCategoryId,
        string question)
    {

      var evaluationCategoryItem = repository.Create<EvaluationCategoryItem>();
      evaluationCategoryItem.EvaluationCategoryId = evaluationCategoryId;
      evaluationCategoryItem.Question = question;
      repository.Add(evaluationCategoryItem);
      return evaluationCategoryItem;
    }
    #endregion

    #region Edit

    public EvaluationCategoryItem EditEvaluationCategoryItem(
      int id,
      byte[] rowVersion,
      TValue<string> question = null,
      TValue<int> evaluationCategoryId = null,
      TValue<bool> isArchive = null)
    {

      var evaluationCategoryItem = GetEvaluationCategoryItem(id: id);
      return EditEvaluationCategoryItem(
                evaluationCategoryItem: evaluationCategoryItem,
                rowVersion: rowVersion,
                evaluationCategoryId: evaluationCategoryId,
                isArchive: isArchive,
                question: question);
    }
    public EvaluationCategoryItem EditEvaluationCategoryItem(
        EvaluationCategoryItem evaluationCategoryItem,
        byte[] rowVersion,
        TValue<string> question = null,
        TValue<int> evaluationCategoryId = null,
        TValue<bool> isArchive = null)
    {

      if (question != null)
        evaluationCategoryItem.Question = question;
      if (evaluationCategoryId != null)
        evaluationCategoryItem.EvaluationCategoryId = evaluationCategoryId;
      if (isArchive != null)
        evaluationCategoryItem.IsArchive = isArchive;
      repository.Update(evaluationCategoryItem, rowVersion);
      return evaluationCategoryItem;
    }
    #endregion

    #region Get
    public EvaluationCategoryItem GetEvaluationCategoryItem(int id) => GetEvaluationCategoryItem(selector: e => e, id: id);
    public TResult GetEvaluationCategoryItem<TResult>(
        Expression<Func<EvaluationCategoryItem, TResult>> selector,
        int id)
    {

      var evaluationCategoryItem = GetEvaluationCategoryItems(
                selector: selector,
                id: id).FirstOrDefault();
      if (evaluationCategoryItem == null)
        throw new RecordNotFoundException(id, typeof(EvaluationCategoryItem));
      return evaluationCategoryItem;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetEvaluationCategoryItems<TResult>(
        Expression<Func<EvaluationCategoryItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int> evaluationCategoryId = null,
        TValue<bool> isArchive = null)
    {

      var evaluationCategoryItems = repository.GetQuery<EvaluationCategoryItem>();
      if (id != null)
        evaluationCategoryItems = evaluationCategoryItems.Where(x => x.Id == id);
      if (evaluationCategoryId != null)
        evaluationCategoryItems = evaluationCategoryItems.Where(i => i.EvaluationCategoryId == evaluationCategoryId);
      if (isArchive != null)
        evaluationCategoryItems = evaluationCategoryItems.Where(i => i.IsArchive == isArchive);

      return evaluationCategoryItems.Select(selector);
    }
    #endregion

    #region Delete
    public void DeleteEvaluationCategoryItem(int id)
    {

      var evaluationCategoryItem = GetEvaluationCategoryItem(id: id);

      DeleteEvaluationCategoryItem(evaluationCategoryItem: evaluationCategoryItem);
    }

    public void DeleteEvaluationCategoryItem(EvaluationCategoryItem evaluationCategoryItem)
    {

      repository.Delete(evaluationCategoryItem);
    }


    public void ValidateDeleteEvaluationCategoryItem(int id)
    {


      var employeeEvaluationItemCount = GetEvaluationCategoryItem(
                e => e.EmployeeEvaluationItemDetails.Count,
                id: id);

      if (employeeEvaluationItemCount > 0)
      {
        throw new CannotDeleteEvaluationCategoryItemException(id: id);
      }

    }
    #endregion

    #region ToResult
    public Expression<Func<EvaluationCategoryItem, EvaluationCategoryItemResult>> ToEvaluationCategoryItemResult =
        evaluationCategoryItem => new EvaluationCategoryItemResult
        {
          Id = evaluationCategoryItem.Id,
          EvaluationCategoryId = evaluationCategoryItem.EvaluationCategoryId,
          Question = evaluationCategoryItem.Question,
          IsArchive = evaluationCategoryItem.IsArchive,
          RowVersion = evaluationCategoryItem.RowVersion,

        };
    #endregion
  }
}
