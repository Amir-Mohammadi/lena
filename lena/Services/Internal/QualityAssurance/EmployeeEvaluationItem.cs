using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityAssurance.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.QualityAssurance.EmployeeEvaluationItem;
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
    public EmployeeEvaluationItem AddEmployeeEvaluationItem(
        int employeeEvaluationId,
        int evaluationCategoryId,
        string description)
    {

      var employeeEvaluationItem = repository.Create<EmployeeEvaluationItem>();
      employeeEvaluationItem.EmployeeEvaluationId = employeeEvaluationId;
      employeeEvaluationItem.EvaluationCategoryId = evaluationCategoryId;
      employeeEvaluationItem.Description = description;
      employeeEvaluationItem.DateTime = DateTime.UtcNow;
      employeeEvaluationItem.UserId = App.Providers.Security.CurrentLoginData.UserId;
      employeeEvaluationItem.Status = EmployeeEvaluationStatus.Temporary;
      repository.Add(employeeEvaluationItem);
      return employeeEvaluationItem;
    }
    #endregion

    #region Edit
    public EmployeeEvaluationItem EditEmployeeEvaluationItem(
      int employeeEvaluationId,
      int evaluationCategoryId,
      byte[] rowVersion,
      TValue<string> description = null,
      TValue<DateTime> permanentDateTime = null,
      TValue<EmployeeEvaluationStatus> status = null)
    {

      var employeeEvaluationItem = GetEmployeeEvaluationItem(
                employeeEvaluationId: employeeEvaluationId,
                evaluationCategoryId: evaluationCategoryId);

      return EditEmployeeEvaluationItem(
                employeeEvaluationItem: employeeEvaluationItem,
                rowVersion: rowVersion,
                description: description,
                status: status);
    }
    public EmployeeEvaluationItem EditEmployeeEvaluationItem(
        EmployeeEvaluationItem employeeEvaluationItem,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<DateTime> permanentDateTime = null,
        TValue<EmployeeEvaluationStatus> status = null)
    {

      employeeEvaluationItem.DateTime = DateTime.UtcNow;
      employeeEvaluationItem.UserId = App.Providers.Security.CurrentLoginData.UserId;

      if (description != null)
        employeeEvaluationItem.Description = description;
      if (status != null)
        employeeEvaluationItem.Status = status;
      if (permanentDateTime != null)
        employeeEvaluationItem.PermanentDateTime = permanentDateTime;
      repository.Update(employeeEvaluationItem, rowVersion);
      return employeeEvaluationItem;
    }
    #endregion

    #region Get
    public EmployeeEvaluationItem GetEmployeeEvaluationItem(int employeeEvaluationId, int evaluationCategoryId) =>
    GetEmployeeEvaluationItem(selector: e => e, employeeEvaluationId: employeeEvaluationId, evaluationCategoryId: evaluationCategoryId);
    public TResult GetEmployeeEvaluationItem<TResult>(
        Expression<Func<EmployeeEvaluationItem, TResult>> selector,
        int employeeEvaluationId,
        int evaluationCategoryId)
    {

      var employeeEvaluationItem = GetEmployeeEvaluationItems(
                selector: selector,
                employeeEvaluationId: employeeEvaluationId,
                evaluationCategoryId: evaluationCategoryId).FirstOrDefault();
      if (employeeEvaluationItem == null)
        throw new EmployeeEvaluationItemNotFoundException(employeeEvaluationId: employeeEvaluationId, evaluationCategoryItemId: evaluationCategoryId);
      return employeeEvaluationItem;
    }


    #endregion

    #region Gets
    public IQueryable<TResult> GetEmployeeEvaluationItems<TResult>(
        Expression<Func<EmployeeEvaluationItem, TResult>> selector,
        TValue<int> userId = null,
        TValue<int> employeeEvaluationId = null,
        TValue<int> evaluationCategoryId = null,
        TValue<EmployeeEvaluationStatus> status = null)
    {

      var employeeEvaluationItems = repository.GetQuery<EmployeeEvaluationItem>();

      if (userId != null)
        employeeEvaluationItems = employeeEvaluationItems.Where(x => x.UserId == userId);
      if (employeeEvaluationId != null)
        employeeEvaluationItems = employeeEvaluationItems.Where(x => x.EmployeeEvaluationId == employeeEvaluationId);
      if (evaluationCategoryId != null)
        employeeEvaluationItems = employeeEvaluationItems.Where(x => x.EvaluationCategoryId == evaluationCategoryId);
      if (status != null)
        employeeEvaluationItems = employeeEvaluationItems.Where(x => x.Status == status);

      return employeeEvaluationItems.Select(selector);
    }
    #endregion

    #region GetCurrentEmployeeEvaluationItem
    public EmployeeEvaluationItem GetCurrentEmployeeEvaluationItem(int employeeEvaluationId, int evaluationCategoryId)
    {


      var employeeEvaluationItem = GetEmployeeEvaluationItems(
                selector: e => e,
                employeeEvaluationId: employeeEvaluationId,
                evaluationCategoryId: evaluationCategoryId);

      return employeeEvaluationItem.FirstOrDefault();
    }
    #endregion

    #region ToResult
    public Expression<Func<EmployeeEvaluationItem, EmployeeEvaluationItemResult>> ToEmployeeEvaluationItemResult =
       employeeEvaluationItem => new EmployeeEvaluationItemResult
       {
         EmployeeEvaluationId = employeeEvaluationItem.EmployeeEvaluationId,
         EvaluationCategoryId = employeeEvaluationItem.EvaluationCategoryId,
         Status = employeeEvaluationItem.Status,
         Description = employeeEvaluationItem.Description,
         RowVersion = employeeEvaluationItem.RowVersion
       };
    #endregion

  }
}
