//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityAssurance.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.QualityAssurance.EmployeeEvaluationPeriodItem;
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
    public EmployeeEvaluationPeriodItem AddEmployeeEvaluationPeriodItem(
        int employeeEvaluationPeriodId,
        int evaluationCategoryId,
        int coefficient)
    {

      var employeeEvaluationPeriodItem = repository.Create<EmployeeEvaluationPeriodItem>();
      employeeEvaluationPeriodItem.EmployeeEvaluationPeriodId = employeeEvaluationPeriodId;
      employeeEvaluationPeriodItem.EvaluationCategoryId = evaluationCategoryId;
      employeeEvaluationPeriodItem.Coefficient = coefficient;


      repository.Add(employeeEvaluationPeriodItem);
      return employeeEvaluationPeriodItem;
    }
    #endregion

    #region Edit
    public EmployeeEvaluationPeriodItem EditEmployeeEvaluationPeriodItem(
      int employeeEvaluationPeriodId,
      int evaluationCategoryId,
      byte[] rowVersion,
      TValue<int> coefficient = null)
    {

      var employeeEvaluationPeriodItem = GetEmployeeEvaluationPeriodItem(
                employeeEvaluationPeriodId: employeeEvaluationPeriodId,
                evaluationCategoryId: evaluationCategoryId);

      return EditEmployeeEvaluationPeriodItem(
                employeeEvaluationPeriodItem: employeeEvaluationPeriodItem,
                rowVersion: rowVersion,
                coefficient: coefficient);
    }
    public EmployeeEvaluationPeriodItem EditEmployeeEvaluationPeriodItem(
        EmployeeEvaluationPeriodItem employeeEvaluationPeriodItem,
        byte[] rowVersion,
        TValue<int> coefficient = null)
    {


      if (coefficient != null)
        employeeEvaluationPeriodItem.Coefficient = coefficient;


      repository.Update(employeeEvaluationPeriodItem, rowVersion);
      return employeeEvaluationPeriodItem;
    }
    #endregion

    #region Get
    public EmployeeEvaluationPeriodItem GetEmployeeEvaluationPeriodItem(
        int employeeEvaluationPeriodId,
        int evaluationCategoryId) => GetEmployeeEvaluationPeriodItem(
            selector: e => e,
            employeeEvaluationPeriodId: employeeEvaluationPeriodId,
            evaluationCategoryId: evaluationCategoryId);

    public TResult GetEmployeeEvaluationPeriodItem<TResult>(
        Expression<Func<EmployeeEvaluationPeriodItem, TResult>> selector,
        TValue<int> employeeEvaluationPeriodId = null,
        TValue<int> evaluationCategoryId = null)
    {

      var employeeEvaluationPeriodItem = GetEmployeeEvaluationPeriodItems(
                selector: selector,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId,
               evaluationCategoryId: evaluationCategoryId)

                .FirstOrDefault();
      if (employeeEvaluationPeriodItem == null)
        throw new EmployeeEvaluationPeriodItemNotFoundException();

      return employeeEvaluationPeriodItem;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetEmployeeEvaluationPeriodItems<TResult>(
        Expression<Func<EmployeeEvaluationPeriodItem, TResult>> selector,
        TValue<int> employeeEvaluationPeriodId = null,
        TValue<int> evaluationCategoryId = null)
    {

      var employeeEvaluationPeriodItems = repository.GetQuery<EmployeeEvaluationPeriodItem>();

      if (employeeEvaluationPeriodId != null)
        employeeEvaluationPeriodItems = employeeEvaluationPeriodItems.Where(x => x.EmployeeEvaluationPeriodId == employeeEvaluationPeriodId);

      if (evaluationCategoryId != null)
        employeeEvaluationPeriodItems = employeeEvaluationPeriodItems.Where(x => x.EvaluationCategoryId == evaluationCategoryId);

      return employeeEvaluationPeriodItems.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<EmployeeEvaluationPeriodItem, EmployeeEvaluationPeriodItemResult>> ToEmployeeEvaluationPeriodItemResult =
       employeeEvaluationPeriodItem => new EmployeeEvaluationPeriodItemResult
       {
         EvaluationCategoryId = employeeEvaluationPeriodItem.EvaluationCategoryId,
         EmployeeEvaluationPeriodId = employeeEvaluationPeriodItem.EmployeeEvaluationPeriodId,
         EmployeeEvaluationPeriodTitle = employeeEvaluationPeriodItem.EmployeeEvaluationPeriod.Title,
         EvaluationCategoryTitle = employeeEvaluationPeriodItem.EvaluationCategory.Title,
         Coefficient = employeeEvaluationPeriodItem.Coefficient,
         RowVersion = employeeEvaluationPeriodItem.RowVersion
       };
    #endregion

    #region DeleteEmployeeEvaluationPeriod


    public void DeleteEmployeeEvaluationPeriodItem(
        int employeeEvaluationPeriodId,
        int evaluationCategoryId)
    {

      var employeeEvaluationPeriodItem = GetEmployeeEvaluationPeriodItem(
               employeeEvaluationPeriodId: employeeEvaluationPeriodId,
               evaluationCategoryId: evaluationCategoryId);


      DeleteEmployeeEvaluationPeriodItem(employeeEvaluationPeriodItem: employeeEvaluationPeriodItem);

    }

    public void DeleteEmployeeEvaluationPeriodItem(EmployeeEvaluationPeriodItem employeeEvaluationPeriodItem)
    {

      repository.Delete(employeeEvaluationPeriodItem);
    }
    #endregion
  }
}
