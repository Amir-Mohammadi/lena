using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityAssurance.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityAssurance.EmployeeEvaluationPeriod;
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
    public EmployeeEvaluationPeriod AddEmployeeEvaluationPeriod(
        string title,
        DateTime fromDateTime,
        DateTime toDateTime)
    {
      var employeeEvaluationPeriod = repository.Create<EmployeeEvaluationPeriod>();
      employeeEvaluationPeriod.Title = title;
      employeeEvaluationPeriod.FromDateTime = fromDateTime;
      employeeEvaluationPeriod.ToDateTime = toDateTime;
      employeeEvaluationPeriod.CreatedDateTime = DateTime.UtcNow;
      employeeEvaluationPeriod.IsActive = true;
      employeeEvaluationPeriod.Status = EmployeeEvaluationPeriodStatus.NotAction;
      employeeEvaluationPeriod.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(employeeEvaluationPeriod);
      return employeeEvaluationPeriod;
    }
    #endregion
    #region Edit
    public EmployeeEvaluationPeriod EditEmployeeEvaluationPeriod(
      int id,
      byte[] rowVersion,
      TValue<string> title = null,
      TValue<bool> isActive = null,
      TValue<DateTime> fromDateTime = null,
      TValue<DateTime> toDateTime = null,
      TValue<EmployeeEvaluationPeriodStatus> status = null)
    {
      var employeeEvaluationPeriod = GetEmployeeEvaluationPeriod(id: id);
      return EditEmployeeEvaluationPeriod(
                employeeEvaluationPeriod: employeeEvaluationPeriod,
                rowVersion: rowVersion,
                title: title,
                isActive: isActive,
                fromDateTime: fromDateTime,
                toDateTime: toDateTime,
                status: status);
    }
    public EmployeeEvaluationPeriod EditEmployeeEvaluationPeriod(
        EmployeeEvaluationPeriod employeeEvaluationPeriod,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<DateTime> fromDateTime = null,
        TValue<bool> isActive = null,
        TValue<DateTime> toDateTime = null,
        TValue<EmployeeEvaluationPeriodStatus> status = null)
    {
      if (title != null)
        employeeEvaluationPeriod.Title = title;
      if (fromDateTime != null)
        employeeEvaluationPeriod.FromDateTime = fromDateTime;
      if (toDateTime != null)
        employeeEvaluationPeriod.ToDateTime = toDateTime;
      if (status != null)
        employeeEvaluationPeriod.Status = status;
      if (isActive != null)
        employeeEvaluationPeriod.IsActive = isActive;
      repository.Update(employeeEvaluationPeriod, rowVersion);
      return employeeEvaluationPeriod;
    }
    #endregion
    #region AddProcess
    public void AddEmployeeEvaluationPeriodProcess(
        string title,
        DateTime fromDateTime,
        DateTime toDateTime,
        AddEmployeeEvaluationPeriodItemInput[] addEmployeeEvaluationPeriodItemInputs)
    {
      var existEmployeeEvaluationPeriod = GetEmployeeEvaluationPeriodByName(title: title);
      if (existEmployeeEvaluationPeriod != null)
      {
        throw new EmployeeEvaluationPeriodExistByThisNameException(title: title);
      }
      if (!addEmployeeEvaluationPeriodItemInputs.Any())
      {
        throw new TheEmployeeEvaluationPeriodShouldHaveEvaluationCategoryException();
      }
      var sumCoefficient = addEmployeeEvaluationPeriodItemInputs.Sum(m => m.Coefficient);
      if (sumCoefficient > 100 || sumCoefficient < 100)
      {
      }
      var employeeEvaluationPeriod = AddEmployeeEvaluationPeriod(
                title: title.Trim(),
                fromDateTime: fromDateTime,
                toDateTime: toDateTime);
      foreach (var item in addEmployeeEvaluationPeriodItemInputs)
      {
        AddEmployeeEvaluationPeriodItem(
                  employeeEvaluationPeriodId: employeeEvaluationPeriod.Id,
                  evaluationCategoryId: item.EvaluationCategoryId,
                  coefficient: item.Coefficient);
      }
    }
    #endregion
    #region EditProcess
    public void EditEmployeeEvaluationPeriodProcess(
        int id,
        byte[] rowVersion,
        string title,
        DateTime? fromDateTime,
        DateTime? toDateTime,
        bool? isActive,
        AddEmployeeEvaluationPeriodItemInput[] addEmployeeEvaluationPeriodItemInputs,
        EditEmployeeEvaluationPeriodItemInput[] editEmployeeEvaluationPeriodItemInputs,
        DeleteEmployeeEvaluationPeriodItemInput[] deleteEmployeeEvaluationPeriodItemInputs
        )
    {
      var existEmployeeEvaluationPeriod = GetEmployeeEvaluationPeriodByName(title: title);
      if (existEmployeeEvaluationPeriod != null)
      {
        if (existEmployeeEvaluationPeriod.Id != id)
          throw new EmployeeEvaluationPeriodExistByThisNameException(title: title);
      }
      var items = addEmployeeEvaluationPeriodItemInputs.Select(
                m => new
                {
                  Coefficient = m.Coefficient,
                  EvaluationCategoryId = m.EvaluationCategoryId
                }).Union(
                editEmployeeEvaluationPeriodItemInputs.Select(m => new
                {
                  Coefficient = m.Coefficient,
                  EvaluationCategoryId = m.EvaluationCategoryId
                }));
      if (!items.Any())
      {
        throw new TheEmployeeEvaluationPeriodShouldHaveEvaluationCategoryException();
      }
      var sumCoefficient = items.Sum(m => m.Coefficient);
      if (sumCoefficient > 100 || sumCoefficient < 100)
      {
        throw new TheTotalSumOfCoefficientShouldBeOneHundredException();
      }
      var employeeEvaluationPeriod = GetEmployeeEvaluationPeriod(id: id);
      if (employeeEvaluationPeriod.Status != EmployeeEvaluationPeriodStatus.NotAction)
      {
        throw new YouCanEditEmployeeEvaluationPeriodInNotActionStatusException(id: id);
      }
      EditEmployeeEvaluationPeriod(
                employeeEvaluationPeriod: employeeEvaluationPeriod,
                rowVersion: rowVersion,
                title: title,
                isActive: isActive,
                fromDateTime: fromDateTime,
                toDateTime: toDateTime);
      foreach (var item in addEmployeeEvaluationPeriodItemInputs)
      {
        AddEmployeeEvaluationPeriodItem(
                  employeeEvaluationPeriodId: employeeEvaluationPeriod.Id,
                  evaluationCategoryId: item.EvaluationCategoryId,
                  coefficient: item.Coefficient);
      }
      foreach (var item in editEmployeeEvaluationPeriodItemInputs)
      {
        EditEmployeeEvaluationPeriodItem(
                  employeeEvaluationPeriodId: item.EmployeeEvaluationPeriodId,
                  evaluationCategoryId: item.EvaluationCategoryId,
                  rowVersion: item.RowVersion,
                  coefficient: item.Coefficient);
      }
      foreach (var item in deleteEmployeeEvaluationPeriodItemInputs)
      {
        DeleteEmployeeEvaluationPeriodItem(
                  employeeEvaluationPeriodId: item.EmployeeEvaluationPeriodId,
                  evaluationCategoryId: item.EvaluationCategoryId);
      }
    }
    #endregion
    #region Get By Id
    public EmployeeEvaluationPeriod GetEmployeeEvaluationPeriod(int id) => GetEmployeeEvaluationPeriod(selector: e => e, id: id);
    public TResult GetEmployeeEvaluationPeriod<TResult>(
        Expression<Func<EmployeeEvaluationPeriod, TResult>> selector,
        int id)
    {
      var employeeEvaluationPeriod = GetEmployeeEvaluationPeriods(
                selector: selector,
                id: id)
                .FirstOrDefault();
      if (employeeEvaluationPeriod == null)
        throw new EmployeeEvaluationPeriodNotFoundException(id: id);
      return employeeEvaluationPeriod;
    }
    #endregion
    #region Get By Name
    public EmployeeEvaluationPeriod GetEmployeeEvaluationPeriodByName(string title) => GetEmployeeEvaluationPeriodByName(selector: e => e, title: title);
    public TResult GetEmployeeEvaluationPeriodByName<TResult>(
        Expression<Func<EmployeeEvaluationPeriod, TResult>> selector,
        string title)
    {
      var employeeEvaluationPeriod = GetEmployeeEvaluationPeriods(
                selector: selector,
                title: title)
                .FirstOrDefault();
      return employeeEvaluationPeriod;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetEmployeeEvaluationPeriods<TResult>(
        Expression<Func<EmployeeEvaluationPeriod, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<bool> isActive = null,
        TValue<EmployeeEvaluationPeriodStatus> status = null)
    {
      var employeeEvaluationPeriods = repository.GetQuery<EmployeeEvaluationPeriod>();
      if (id != null)
        employeeEvaluationPeriods = employeeEvaluationPeriods.Where(x => x.Id == id);
      if (title != null)
        employeeEvaluationPeriods = employeeEvaluationPeriods.Where(x => x.Title == title.Value.Trim());
      if (status != null)
        employeeEvaluationPeriods = employeeEvaluationPeriods.Where(x => x.Status == status);
      if (isActive != null)
        employeeEvaluationPeriods = employeeEvaluationPeriods.Where(x => x.IsActive == isActive);
      return employeeEvaluationPeriods.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<EmployeeEvaluationPeriod, EmployeeEvaluationPeriodComboResult>> ToEmployeeEvaluationPeriodComboResult =
       employeeEvaluationPeriod => new EmployeeEvaluationPeriodComboResult
       {
         Id = employeeEvaluationPeriod.Id,
         Title = employeeEvaluationPeriod.Title
       };
    public Expression<Func<EmployeeEvaluationPeriod, EmployeeEvaluationPeriodResult>> ToEmployeeEvaluationPeriodResult =
       employeeEvaluationPeriod => new EmployeeEvaluationPeriodResult
       {
         Id = employeeEvaluationPeriod.Id,
         Title = employeeEvaluationPeriod.Title,
         FromDateTime = employeeEvaluationPeriod.FromDateTime,
         ToDateTime = employeeEvaluationPeriod.ToDateTime,
         EmployeeFullName = employeeEvaluationPeriod.User.Employee.FirstName + " " + employeeEvaluationPeriod.User.Employee.LastName,
         CreatedDateTime = employeeEvaluationPeriod.CreatedDateTime,
         Status = employeeEvaluationPeriod.Status,
         IsActive = employeeEvaluationPeriod.IsActive,
         RowVersion = employeeEvaluationPeriod.RowVersion
       };
    #endregion
    #region Search
    public IQueryable<EmployeeEvaluationPeriodResult> SearchEmployeeEvaluationPeriod(IQueryable<EmployeeEvaluationPeriodResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.EmployeeFullName.Contains(searchText) ||
            item.Title.Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<EmployeeEvaluationPeriodResult> SortEmployeeEvaluationPeriodResult(IQueryable<EmployeeEvaluationPeriodResult> query,
        SortInput<EmployeeEvaluationPeriodSortType> sort)
    {
      switch (sort.SortType)
      {
        case EmployeeEvaluationPeriodSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case EmployeeEvaluationPeriodSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case EmployeeEvaluationPeriodSortType.FromDateTime:
          return query.OrderBy(a => a.FromDateTime, sort.SortOrder);
        case EmployeeEvaluationPeriodSortType.ToDateTime:
          return query.OrderBy(a => a.ToDateTime, sort.SortOrder);
        case EmployeeEvaluationPeriodSortType.CreatedDateTime:
          return query.OrderBy(a => a.CreatedDateTime, sort.SortOrder);
        case EmployeeEvaluationPeriodSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case EmployeeEvaluationPeriodSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ActiveEmployeeEvaluationPeriod
    public void ActiveEmployeeEvaluationPeriod(int id, byte[] rowVersion)
    {
      EditEmployeeEvaluationPeriod(rowVersion: rowVersion, id: id, status: EmployeeEvaluationPeriodStatus.Evaluating);
    }
    #endregion
    #region EnableOrDisableEmployeeEvaluationPeriod
    public void EnableOrDisableEmployeeEvaluationPeriod(int id, byte[] rowVersion, bool? isActive)
    {
      EditEmployeeEvaluationPeriod(rowVersion: rowVersion, id: id, isActive: isActive);
    }
    #endregion
    #region DeactiveEmployeeEvaluationPeriod
    public void DeactiveEmployeeEvaluationPeriod(int id, byte[] rowVersion)
    {
      EditEmployeeEvaluationPeriod(rowVersion: rowVersion, id: id, status: EmployeeEvaluationPeriodStatus.Finished);
    }
    #endregion
    #region DeleteEmployeeEvaluationPeriod
    public void DeleteEmployeeEvaluationPeriodProcess(int id)
    {
      var employeeEvaluationPeriod = GetEmployeeEvaluationPeriod(id: id);
      if (employeeEvaluationPeriod.EmployeeEvaluations.Any())
      {
        throw new CannotDeleteEmployeeEvaluationPeriodException(id: id);
      }
      DeleteEmployeeEvaluationPeriod(employeeEvaluationPeriod: employeeEvaluationPeriod);
    }
    public void DeleteEmployeeEvaluationPeriod(int id)
    {
      var employeeEvaluationPeriod = GetEmployeeEvaluationPeriod(id: id);
      DeleteEmployeeEvaluationPeriod(employeeEvaluationPeriod: employeeEvaluationPeriod);
    }
    public void DeleteEmployeeEvaluationPeriod(EmployeeEvaluationPeriod employeeEvaluationPeriod)
    {
      repository.Delete(employeeEvaluationPeriod);
    }
    #endregion
    #region ValidateEmployeeEvaluationPeriodActivation
    public void ValidateEmployeeEvaluationPeriodActivation(int employeeEvaluationPeriodId)
    {
      var employeeEvaluationPeriod = GetEmployeeEvaluationPeriod(id: employeeEvaluationPeriodId);
      if (employeeEvaluationPeriod.Status == EmployeeEvaluationPeriodStatus.NotAction)
      {
        throw new EmployeeEvaluationPeriodIsNotStartedException(id: employeeEvaluationPeriodId);
      }
      if (employeeEvaluationPeriod.Status == EmployeeEvaluationPeriodStatus.Finished)
      {
        throw new EmployeeEvaluationPeriodIsFinishedException(id: employeeEvaluationPeriodId);
      }
    }
    #endregion
  }
}