//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityAssurance.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.QualityAssurance.EmployeeEvaluationItemDetail;
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
    public EmployeeEvaluationItemDetail AddEmployeeEvaluationItemDetail(
        int employeeEvaluationId,
        int evaluationCategoryId,
        int evaluationCategoryItemId,
        Score score)
    {
      var employeeEvaluationItemDetail = repository.Create<EmployeeEvaluationItemDetail>();
      employeeEvaluationItemDetail.EmployeeEvaluationId = employeeEvaluationId;
      employeeEvaluationItemDetail.EvaluationCategoryId = evaluationCategoryId;
      employeeEvaluationItemDetail.EvaluationCategoryItemId = evaluationCategoryItemId;
      employeeEvaluationItemDetail.Score = score;
      repository.Add(employeeEvaluationItemDetail);
      return employeeEvaluationItemDetail;
    }
    #endregion
    #region Edit
    public EmployeeEvaluationItemDetail EditEmployeeEvaluationItemDetail(
      int employeeEvaluationId,
      int evaluationCategoryId,
      int evaluationCategoryItemId,
      byte[] rowVersion,
      TValue<Score> score = null)
    {
      var employeeEvaluationItemDetail = GetEmployeeEvaluationItemDetail(
                employeeEvaluationId: employeeEvaluationId,
                evaluationCategoryId: evaluationCategoryId,
                evaluationCategoryItemId: evaluationCategoryItemId);
      return EditEmployeeEvaluationItemDetail(
                employeeEvaluationItemDatail: employeeEvaluationItemDetail,
                rowVersion: rowVersion,
                score: score);
    }
    public EmployeeEvaluationItemDetail EditEmployeeEvaluationItemDetail(
        EmployeeEvaluationItemDetail employeeEvaluationItemDatail,
        byte[] rowVersion,
        TValue<Score> score = null)
    {
      if (score != null)
        employeeEvaluationItemDatail.Score = score;
      repository.Update(employeeEvaluationItemDatail, rowVersion);
      return employeeEvaluationItemDatail;
    }
    #endregion
    #region Get
    public EmployeeEvaluationItemDetail GetEmployeeEvaluationItemDetail(int employeeEvaluationId, int evaluationCategoryId, int evaluationCategoryItemId) =>
    GetEmployeeEvaluationItemDetail(selector: e => e, employeeEvaluationId: employeeEvaluationId, evaluationCategoryId: evaluationCategoryId, evaluationCategoryItemId: evaluationCategoryItemId);
    public TResult GetEmployeeEvaluationItemDetail<TResult>(
        Expression<Func<EmployeeEvaluationItemDetail, TResult>> selector,
        int employeeEvaluationId,
        int evaluationCategoryId,
        int evaluationCategoryItemId)
    {
      var employeeEvaluationItemDetail = GetEmployeeEvaluationItemDetails(
                selector: selector,
                employeeEvaluationId: employeeEvaluationId,
                evaluationCategoryId: evaluationCategoryId,
                evaluationCategoryItemId: evaluationCategoryItemId).FirstOrDefault();
      if (employeeEvaluationItemDetail == null)
        throw new EmployeeEvaluationItemDetailNotFoundException(
                  employeeEvaluationId: employeeEvaluationId,
                  evaluationCategoryId: evaluationCategoryId,
                  evaluationCategoryItemId: evaluationCategoryItemId);
      return employeeEvaluationItemDetail;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetEmployeeEvaluationItemDetails<TResult>(
        Expression<Func<EmployeeEvaluationItemDetail, TResult>> selector,
        TValue<int> employeeEvaluationId = null,
        TValue<int> evaluationCategoryId = null,
        TValue<int> evaluationCategoryItemId = null,
        TValue<int> employeeId = null,
        TValue<int> employeeEvaluationPeriodId = null)
    {
      var employeeEvaluationItemDetails = repository.GetQuery<EmployeeEvaluationItemDetail>();
      if (employeeEvaluationId != null)
        employeeEvaluationItemDetails = employeeEvaluationItemDetails.Where(x => x.EmployeeEvaluationId == employeeEvaluationId);
      if (evaluationCategoryId != null)
        employeeEvaluationItemDetails = employeeEvaluationItemDetails.Where(x => x.EvaluationCategoryId == evaluationCategoryId);
      if (evaluationCategoryItemId != null)
        employeeEvaluationItemDetails = employeeEvaluationItemDetails.Where(x => x.EvaluationCategoryItemId == evaluationCategoryItemId);
      if (employeeId != null)
        employeeEvaluationItemDetails = employeeEvaluationItemDetails.Where(x => x.EmployeeEvaluationItem.EmployeeEvaluation.EmployeeId == employeeId);
      if (employeeEvaluationPeriodId != null)
        employeeEvaluationItemDetails = employeeEvaluationItemDetails.Where(x => x.EmployeeEvaluationItem.EmployeeEvaluation.EmployeeEvaluationPeriodId == employeeEvaluationPeriodId);
      return employeeEvaluationItemDetails.Select(selector);
    }
    #endregion
    #region GetFullEmployeeEvaluationItemDetails
    public IQueryable<EmployeeEvaluationItemDetailResult> GetFullEmployeeEvaluationItemDetails(int employeeEvaluationId, int employeeEvaluationPeriodId)
    {
      var employeeEvaluations = GetEmployeeEvaluations(
                selector: e => e,
                id: employeeEvaluationId);
      var employeeEvaluationItemDetails = GetEmployeeEvaluationItemDetails(
                selector: e => e,
                employeeEvaluationId: employeeEvaluationId);
      var employeeEvaluationPeriodItems = GetEmployeeEvaluationPeriodItems(
                selector: e => e,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId);
      var result = from employeeEvaluationItemDetail in employeeEvaluationItemDetails
                   join employeeEvaluation in employeeEvaluations on employeeEvaluationItemDetail.EmployeeEvaluationId equals employeeEvaluation.Id
                   join employeeEvaluationPeriodItem in employeeEvaluationPeriodItems
                          on new { employeeEvaluationItemDetail.EvaluationCategoryId, employeeEvaluation.EmployeeEvaluationPeriodId }
                          equals
                         new { employeeEvaluationPeriodItem.EvaluationCategoryId, employeeEvaluationPeriodItem.EmployeeEvaluationPeriodId }
                   select new EmployeeEvaluationItemDetailResult
                   {
                     EmployeeEvaluationId = employeeEvaluationItemDetail.EmployeeEvaluationId,
                     EvaluationCategoryId = employeeEvaluationItemDetail.EvaluationCategoryId,
                     EvaluationScore = employeeEvaluationItemDetail.Score,
                     EvaluationCategoryItemId = employeeEvaluationItemDetail.EvaluationCategoryItemId,
                     EvaluationCategoryTitle = employeeEvaluationItemDetail.EvaluationCategoryItem.EvaluationCategory.Title,
                     EmployeeEvaluationItemDescription = employeeEvaluationItemDetail.EmployeeEvaluationItem.Description,
                     EmployeeEvaluationItemDateTime = employeeEvaluationItemDetail.EmployeeEvaluationItem.DateTime,
                     EvaluationQuestion = employeeEvaluationItemDetail.EvaluationCategoryItem.Question,
                     Coefficient = employeeEvaluationPeriodItem.Coefficient,
                     EmployeeEvaluationItemEmployeeFullName = employeeEvaluationItemDetail.EmployeeEvaluationItem.User.Employee.FirstName + " " + employeeEvaluationItemDetail.EmployeeEvaluationItem.User.Employee.LastName,
                   };
      return result;
    }
    #endregion
  }
}