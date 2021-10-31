using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityAssurance.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.StaticData;
using lena.Models;
using lena.Models.Common;
using lena.Models.QualityAssurance.EmployeeEvaluation;
using lena.Models.QualityAssurance.EmployeeEvaluationItem;
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
    public EmployeeEvaluation AddEmployeeEvaluation(
        int employeeId,
        int employeeEvaluationPeriodId)
    {

      var employeeEvaluation = repository.Create<EmployeeEvaluation>();
      employeeEvaluation.EmployeeId = employeeId;
      employeeEvaluation.EmployeeEvaluationPeriodId = employeeEvaluationPeriodId;
      employeeEvaluation.CreatedDateTime = DateTime.UtcNow;
      employeeEvaluation.CreatedUserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(employeeEvaluation);
      return employeeEvaluation;
    }
    #endregion

    #region Get
    public EmployeeEvaluation GetEmployeeEvaluation(int id) => GetEmployeeEvaluation(selector: e => e, id: id);
    public TResult GetEmployeeEvaluation<TResult>(
        Expression<Func<EmployeeEvaluation, TResult>> selector,
        int id)
    {

      var employeeEvaluation = GetEmployeeEvaluations(
                selector: selector,
                id: id).FirstOrDefault();
      if (employeeEvaluation == null)
        throw new EmployeeEvaluationNotFoundException(id: id);
      return employeeEvaluation;
    }


    public EmployeeEvaluation GetEmployeeEvaluationByPeriodId(
        int employeeId,
        int employeeEvaluationPeriodId) =>
        GetEmployeeEvaluationByPeriodId(
            selector: e => e,
            employeeId: employeeId,
            employeeEvaluationPeriodId);

    public TResult GetEmployeeEvaluationByPeriodId<TResult>(
       Expression<Func<EmployeeEvaluation, TResult>> selector,
       int employeeId,
       int employeeEvaluationPeriodId)
    {

      var employeeEvaluation = GetEmployeeEvaluations(
                selector: selector,
                employeeId: employeeId,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId)


                .FirstOrDefault();

      return employeeEvaluation;
    }

    #endregion

    #region GetCurrentEmployeeEvaluation

    public TResult GetCurrentEmployeeEvaluation<TResult>(
       Expression<Func<EmployeeEvaluation, TResult>> selector,
       int employeeId,
       int employeeEvaluationPeriodId)
    {

      var employeeEvaluation = GetEmployeeEvaluationByPeriodId(
                selector: selector,
                employeeId: employeeId,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId);

      if (employeeEvaluation == null)
      {
        throw new EmployeeEvaluationNotFoundException();
      }
      return employeeEvaluation;
    }

    #endregion

    #region Gets
    public IQueryable<TResult> GetEmployeeEvaluations<TResult>(
        Expression<Func<EmployeeEvaluation, TResult>> selector,
        TValue<int> id = null,
        TValue<int> employeeId = null,
        TValue<int> excludedEmployeeId = null,
        TValue<int[]> employeeIds = null,
        TValue<int> employeeEvaluationPeriodId = null)
    {

      var employeeEvaluation = repository.GetQuery<EmployeeEvaluation>();
      if (id != null)
        employeeEvaluation = employeeEvaluation.Where(x => x.Id == id);
      if (employeeId != null)
        employeeEvaluation = employeeEvaluation.Where(x => x.EmployeeId == employeeId);
      if (excludedEmployeeId != null)
        employeeEvaluation = employeeEvaluation.Where(x => x.EmployeeId != excludedEmployeeId);
      if (employeeIds != null)
        employeeEvaluation = employeeEvaluation.Where(x => employeeIds.Value.Contains(x.EmployeeId));
      if (employeeEvaluationPeriodId != null)
        employeeEvaluation = employeeEvaluation.Where(x => x.EmployeeEvaluationPeriodId == employeeEvaluationPeriodId);

      return employeeEvaluation.Select(selector);
    }
    #endregion

    #region AddEmployeeEvaluationProcess
    public void AddEmployeeEvaluationProcess(
        int employeeId,
        int employeeEvaluationPeriodId,
        SaveEmployeeEvaluationItemInput saveEmployeeEvaluationItemInput)
    {

      ValidateEmployeeEvaluationPeriodActivation(employeeEvaluationPeriodId: employeeEvaluationPeriodId);

      var evaluationCategory = GetEvaluationCategory(id: saveEmployeeEvaluationItemInput.EvaluationCategoryId);

      var userEvaluationCategoryItems = saveEmployeeEvaluationItemInput.SaveEmployeeEvaluationItemDetailInputs.Where(m => m.Score != Score.None);

      var joinEvaluationCategoryItems = from evaluationCategoryItem in evaluationCategory.EvaluationCategoryItems
                                        join userEvaluationCategoryItem in userEvaluationCategoryItems on evaluationCategoryItem.Id equals userEvaluationCategoryItem.EvaluationCategoryItemId into tempUserEvaluationCategoryItem
                                        from tUserEvaluationCategoryItem in tempUserEvaluationCategoryItem.DefaultIfEmpty()
                                        select new
                                        {
                                          Id = evaluationCategoryItem.Id,
                                          EvaluationCategoryItem = tUserEvaluationCategoryItem
                                        };


      if (joinEvaluationCategoryItems.Any(x => x.EvaluationCategoryItem == null))
      {
        throw new YouShouldAnswerAllQuestionException();
      }

      var employeeEvaluation = GetEmployeeEvaluationByPeriodId(
               employeeId: employeeId,
               employeeEvaluationPeriodId: employeeEvaluationPeriodId);

      if (employeeEvaluation != null)
      {
        throw new EmployeeEvaluationExistForThisMonthException(employeeId: employeeId);
      }

      employeeEvaluation = AddEmployeeEvaluation(
                employeeId: employeeId,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId);

      var employeeEvaluationItem = AddEmployeeEvaluationItem(
                employeeEvaluationId: employeeEvaluation.Id,
                evaluationCategoryId: saveEmployeeEvaluationItemInput.EvaluationCategoryId,
                description: saveEmployeeEvaluationItemInput.Description);

      foreach (var item in saveEmployeeEvaluationItemInput.SaveEmployeeEvaluationItemDetailInputs)
      {
        AddEmployeeEvaluationItemDetail(
                  employeeEvaluationId: employeeEvaluationItem.EmployeeEvaluationId,
                  evaluationCategoryId: employeeEvaluationItem.EvaluationCategoryId,
                  evaluationCategoryItemId: item.EvaluationCategoryItemId,
                  score: item.Score);
      }
    }
    #endregion

    #region EditEmployeeEvaluationProcess
    public void EditEmployeeEvaluationProcess(
        int employeeEvaluationId,
        SaveEmployeeEvaluationItemInput saveEmployeeEvaluationItemInput)
    {


      var employeeEvaluation = GetEmployeeEvaluation(
                id: employeeEvaluationId);

      var evaluationCategory = GetEvaluationCategory(id: saveEmployeeEvaluationItemInput.EvaluationCategoryId);

      var userEvaluationCategoryItems = saveEmployeeEvaluationItemInput.SaveEmployeeEvaluationItemDetailInputs.Where(m => m.Score != Score.None);

      var joinEvaluationCategoryItems = from evaluationCategoryItem in evaluationCategory.EvaluationCategoryItems
                                        join userEvaluationCategoryItem in userEvaluationCategoryItems on evaluationCategoryItem.Id equals userEvaluationCategoryItem.EvaluationCategoryItemId into tempUserEvaluationCategoryItem
                                        from tUserEvaluationCategoryItem in tempUserEvaluationCategoryItem.DefaultIfEmpty()
                                        select new
                                        {
                                          Id = evaluationCategoryItem.Id,
                                          EvaluationCategoryItem = tUserEvaluationCategoryItem
                                        };


      if (joinEvaluationCategoryItems.Any(x => x.EvaluationCategoryItem == null))
      {
        throw new YouShouldAnswerAllQuestionException();
      }

      ValidateEmployeeEvaluationPeriodActivation(employeeEvaluationPeriodId: employeeEvaluation.EmployeeEvaluationPeriodId);

      var employeeEvaluationItem = GetCurrentEmployeeEvaluationItem(
                employeeEvaluationId: employeeEvaluationId,
                evaluationCategoryId: saveEmployeeEvaluationItemInput.EvaluationCategoryId);

      if (employeeEvaluationItem != null && employeeEvaluationItem.Status == EmployeeEvaluationStatus.Permanent)
      {
        throw new CannotChangePermanentEmployeeEvaluationException(id: employeeEvaluation.Id);
      }

      var isNewItem = false;
      if (employeeEvaluationItem == null)
      {
        employeeEvaluationItem = AddEmployeeEvaluationItem(
                  employeeEvaluationId: employeeEvaluationId,
                  evaluationCategoryId: saveEmployeeEvaluationItemInput.EvaluationCategoryId,
                  description: saveEmployeeEvaluationItemInput.Description);
        isNewItem = true;
      }
      else
      {
        employeeEvaluationItem = EditEmployeeEvaluationItem(
                  employeeEvaluationItem: employeeEvaluationItem,
                  rowVersion: saveEmployeeEvaluationItemInput.RowVersion,
                  description: saveEmployeeEvaluationItemInput.Description);
      }


      foreach (var item in saveEmployeeEvaluationItemInput.SaveEmployeeEvaluationItemDetailInputs)
      {
        if (isNewItem)
        {
          AddEmployeeEvaluationItemDetail(
                   employeeEvaluationId: employeeEvaluationItem.EmployeeEvaluationId,
                   evaluationCategoryId: employeeEvaluationItem.EvaluationCategoryId,
                   evaluationCategoryItemId: item.EvaluationCategoryItemId, score: item.Score);
        }

        else
        {
          EditEmployeeEvaluationItemDetail(
                   employeeEvaluationId: employeeEvaluationItem.EmployeeEvaluationId,
                   evaluationCategoryId: employeeEvaluationItem.EvaluationCategoryId,
                   evaluationCategoryItemId: item.EvaluationCategoryItemId,
                   score: item.Score,
                   rowVersion: item.RowVersion);
        }

      }
    }
    #endregion

    #region GetValidEmployeeIdsToEvaluate
    public int[] GetValidEmployeeIdsToEvaluate()
    {


      #region Check Confirm Permission And GetEmployeeId
      var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
              actionName: StaticActionName.EvaluateAllEmployee,
              actionParameters: null);

      if (checkPermissionResult.AccessType == AccessType.Denied)
      {
        var employeeIdList = new List<int>();

        var currentEmployee = App.Providers.Security.CurrentLoginData;

        var empolyee = App.Internals.UserManagement.GetEmployee(
                  e => new
                  {
                    Id = e.Id,
                    IsAdmin = e.OrganizationPost == null ? false : e.OrganizationPost.IsAdmin,
                    DepartmentId = e.DepartmentId
                  },
                  id: currentEmployee.UserEmployeeId.Value);

        if (empolyee.IsAdmin)
        {
          employeeIdList = App.Internals.UserManagement.GetEmployees(e => e.Id, departmentId: empolyee.DepartmentId)

               .Where(m => m != currentEmployee.UserEmployeeId)
               .ToList();


        }
        return employeeIdList.ToArray();
      }
      else
      {
        return null;
      }
      #endregion

    }
    #endregion

    #region ValidateEmployeeEvaluationDetermine
    public void ValidateEmployeeEvaluationDetermine(int employeeId)
    {

      var currentEmployeeId = App.Providers.Security.CurrentLoginData.UserEmployeeId;

      var employeeIds = GetValidEmployeeIdsToEvaluate();


      if (employeeIds != null)
      {
        if (!employeeIds.Any(m => m == employeeId) || employeeIds.Any(m => m == currentEmployeeId))
        {
          throw new CannotEvaluateThisEmployeeException(employeeId: employeeId);
        }
      }
      else
      {
        if (employeeId == currentEmployeeId)
          throw new CannotEvaluateThisEmployeeException(employeeId: employeeId);
      }




    }
    #endregion

    #region GetEmployeeEvaluationDeterminePrepare
    public IQueryable<EmployeeEvaluationPrepareResult> GetEmployeeEvaluationDeterminePrepare(
        int employeeId,
        int employeeEvaluationPeriodId,
        int evaluationCategoryId)
    {

      var evaluationCategoryItems = GetEvaluationCategoryItems(
                selector: e => e,
                evaluationCategoryId: evaluationCategoryId,
                isArchive: false);



      var employeeEvaluationItemDetails = GetEmployeeEvaluationItemDetails(
                selector: e => e,
                employeeId: employeeId,
                employeeEvaluationPeriodId: employeeEvaluationPeriodId);



      var result = from evaluationCategoryItem in evaluationCategoryItems
                   join employeeEvaluationItemDetail in employeeEvaluationItemDetails on evaluationCategoryItem.Id equals employeeEvaluationItemDetail.EvaluationCategoryItemId into tempEmployeeEvaluationItemDetail
                   from tEmployeeEvaluationItemDetail in tempEmployeeEvaluationItemDetail.DefaultIfEmpty()
                   select new EmployeeEvaluationPrepareResult
                   {
                     EmployeeEvalutionId = tEmployeeEvaluationItemDetail.EmployeeEvaluationId,
                     EvaluationCateogryItemId = evaluationCategoryItem.Id,
                     Question = evaluationCategoryItem.Question,
                     Score = tEmployeeEvaluationItemDetail.Score,
                     EmployeeEvaluationDescription = tEmployeeEvaluationItemDetail.EmployeeEvaluationItem.Description,
                     EmployeeEvaluationRowVersion = tEmployeeEvaluationItemDetail.EmployeeEvaluationItem.RowVersion,
                     RowVersion = tEmployeeEvaluationItemDetail.RowVersion
                   };


      return result;
    }
    #endregion

    #region GetFullEmployeeEvaluations
    public IQueryable<EmployeeEvaluationFullResult> GetFullEmployeeEvaluations(
        TValue<int> employeeId = null,
        TValue<int> employeeEvaluationPeriodId = null)
    {


      var currentEmployeeId = App.Providers.Security.CurrentLoginData.UserEmployeeId;

      var validEmployeeIds = GetValidEmployeeIdsToEvaluate();


      var employeeEvaluations = GetEmployeeEvaluations(
                e => e, employeeIds: validEmployeeIds,
                employeeId: employeeId,
                excludedEmployeeId: currentEmployeeId);

      var employeeEvaluationItems = GetEmployeeEvaluationItems(
                selector: e => e);

      var employeeEvaluationPeriodItems = GetEmployeeEvaluationPeriodItems(
                selector: e => e);

      var fullEmployeeEvaluations = from employeeEvaluation in employeeEvaluations
                                    from employeeEvaluationItem in employeeEvaluation.EmployeeEvaluationItems
                                    from employeeEvaluationItemDetail in employeeEvaluationItem.EmployeeEvaluationItemDetails
                                    select new
                                    {
                                      EployeeEvaluationId = employeeEvaluation.Id,
                                      EvaluationCategoryId = employeeEvaluationItemDetail.EvaluationCategoryId,
                                      Score = employeeEvaluationItemDetail.Score
                                    };


      var groupFullEmployeeEvaluations = from fullEmployeeEvaluation in fullEmployeeEvaluations
                                         group fullEmployeeEvaluation by new { EployeeEvaluationId = fullEmployeeEvaluation.EployeeEvaluationId, EvaluationCategoryId = fullEmployeeEvaluation.EvaluationCategoryId } into g
                                         select new
                                         {
                                           EployeeEvaluationId = g.Key.EployeeEvaluationId,
                                           EvaluationCategoryId = g.Key.EvaluationCategoryId,
                                           TotalScore = (g.Sum(i => (double)i.Score) / (g.Count() * 5)) * 100
                                         };


      var employeeEvaluationDetails = from groupFullEmployeeEvaluation in groupFullEmployeeEvaluations
                                      join employeeEvaluation in employeeEvaluations on groupFullEmployeeEvaluation.EployeeEvaluationId equals employeeEvaluation.Id
                                      join employeeEvaluationItem in employeeEvaluationItems on
                                            new { eployeeEvaluationId = groupFullEmployeeEvaluation.EployeeEvaluationId, evaluationCategoryId = groupFullEmployeeEvaluation.EvaluationCategoryId }
                                            equals
                                            new { eployeeEvaluationId = employeeEvaluationItem.EmployeeEvaluationId, evaluationCategoryId = employeeEvaluationItem.EvaluationCategoryId }
                                      join employeeEvaluationPeriodItem in employeeEvaluationPeriodItems on
                                            new { employeeEvaluation.EmployeeEvaluationPeriodId, groupFullEmployeeEvaluation.EvaluationCategoryId }
                                            equals
                                            new { employeeEvaluationPeriodItem.EmployeeEvaluationPeriodId, employeeEvaluationPeriodItem.EvaluationCategoryId }
                                      select new
                                      {
                                        EployeeEvaluationId = groupFullEmployeeEvaluation.EployeeEvaluationId,
                                        EvaluationCategoryId = groupFullEmployeeEvaluation.EvaluationCategoryId,
                                        EvaluationCategoryTitle = employeeEvaluationPeriodItem.EvaluationCategory.Title,
                                        TotalScore = groupFullEmployeeEvaluation.TotalScore,
                                        Status = employeeEvaluationItem.Status,
                                        PermanentDateTime = employeeEvaluationItem.PermanentDateTime,
                                        CreatedDateTime = employeeEvaluationItem.DateTime,
                                        Coefficient = employeeEvaluationPeriodItem.Coefficient,
                                        TotalPercentage = (groupFullEmployeeEvaluation.TotalScore / 100) * employeeEvaluationPeriodItem.Coefficient,
                                        RowVersion = employeeEvaluationItem.RowVersion

                                      };


      var groupedEmployeeEvaluationDetails = from employeeEvaluationDetail in employeeEvaluationDetails
                                             group employeeEvaluationDetail by employeeEvaluationDetail.EployeeEvaluationId into g
                                             select new
                                             {
                                               EployeeEvaluationId = g.Key,
                                               EmployeeEvaluationItems = g.Select(m =>
                                                     new EmployeeEvaluationGroupItem
                                                     {
                                                       EmployeeEvaluationId = m.EployeeEvaluationId,
                                                       EvaluationCategoryId = m.EvaluationCategoryId,
                                                       EvaluationCategoryTitle = m.EvaluationCategoryTitle,
                                                       TotalScore = m.TotalScore,
                                                       Coefficient = m.Coefficient,
                                                       TotalPercentage = m.TotalPercentage,
                                                       Status = m.Status,
                                                       PermanentDateTime = m.PermanentDateTime,
                                                       CreatedDateTime = m.CreatedDateTime,
                                                       RowVersion = m.RowVersion
                                                     })
                                             };



      var joinFullEmployeeEvaluations = from groupedEmployeeEvaluationDetail in groupedEmployeeEvaluationDetails
                                        join employeeEvaluation in employeeEvaluations on groupedEmployeeEvaluationDetail.EployeeEvaluationId equals employeeEvaluation.Id
                                        select new
                                        {
                                          Id = employeeEvaluation.Id,
                                          EmployeeId = employeeEvaluation.EmployeeId,
                                          EmployeeEvaluationPeriodId = employeeEvaluation.EmployeeEvaluationPeriodId,
                                          EmployeeEvaluationPeriodStatus = employeeEvaluation.EmployeeEvaluationPeriod.Status,
                                          CreatedDateTime = employeeEvaluation.CreatedDateTime,
                                          CreatedUserId = employeeEvaluation.CreatedUserId,
                                          EmployeeEvaluationItems = groupedEmployeeEvaluationDetail.EmployeeEvaluationItems,
                                          RowVersion = employeeEvaluation.RowVersion
                                        };

      var employees = App.Internals.UserManagement.GetEmployees(
                e => e, ids: validEmployeeIds,
                id: employeeId,
                excludedId: currentEmployeeId,
                isActive: true);

      var employeeEvaluationPeriods = GetEmployeeEvaluationPeriods(
                e => e,
                id: employeeEvaluationPeriodId);

      var result = from employee in employees
                   from employeeEvaluationPeriod in employeeEvaluationPeriods
                   join joinFullEmployeeEvaluation in joinFullEmployeeEvaluations on
                         new { employeeId = employee.Id, employeeEvaluationPeriodId = employeeEvaluationPeriod.Id }
                         equals
                         new { employeeId = joinFullEmployeeEvaluation.EmployeeId, employeeEvaluationPeriodId = joinFullEmployeeEvaluation.EmployeeEvaluationPeriodId } into tempJoinFullEmployeeEvaluation
                   from tJoinFullEmployeeEvaluation in tempJoinFullEmployeeEvaluation.DefaultIfEmpty()
                   select new EmployeeEvaluationFullResult
                   {
                     Id = tJoinFullEmployeeEvaluation.Id,
                     EmployeeId = employee.Id,
                     EmployeeCode = employee.Code,
                     DepartmentName = employee.Department.Name,
                     EmployeeFullName = employee.FirstName + " " + employee.LastName,
                     OrganizationPostId = employee.OrgnizationPostId,
                     OrganizationPostTitle = employee.OrganizationPost.Title,
                     EmployeeEvaluationPeriodId = employeeEvaluationPeriod.Id,
                     EmployeeEvaluationPeriodTitle = employeeEvaluationPeriod.Title,
                     CreatedDateTime = tJoinFullEmployeeEvaluation.CreatedDateTime,
                     EmployeeEvaluationPeriodStatus = employeeEvaluationPeriod.Status,
                     EmployeeEvaluationGroupItems = tJoinFullEmployeeEvaluation.EmployeeEvaluationItems,
                     RowVersion = tJoinFullEmployeeEvaluation.RowVersion

                   };


      return result;
    }
    #endregion

    #region ToResult
    public Expression<Func<EmployeeEvaluation, EmployeeEvaluationResult>> ToEmployeeEvaluationResult =
       employeeEvaluation => new EmployeeEvaluationResult
       {
         Id = employeeEvaluation.Id,
         EmployeeId = employeeEvaluation.EmployeeId,
         EmployeeEvaluationPeriodId = employeeEvaluation.EmployeeEvaluationPeriodId
       };
    #endregion

    #region Search
    public IQueryable<EmployeeEvaluationFullResult> SearchEmployeeEvaluation(IQueryable<EmployeeEvaluationFullResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.EmployeeFullName.Contains(searchText) ||
            item.EmployeeEvaluationPeriodTitle.Contains(searchText) ||
            item.OrganizationPostTitle.Contains(searchText));

      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<EmployeeEvaluationFullResult> SortEmployeeEvaluationFullResult(IQueryable<EmployeeEvaluationFullResult> query,
        SortInput<EmployeeEvaluationSortType> sort)
    {
      switch (sort.SortType)
      {
        case EmployeeEvaluationSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case EmployeeEvaluationSortType.EmployeeCode:
          return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case EmployeeEvaluationSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case EmployeeEvaluationSortType.EmployeeEvaluationPeriodTitle:
          return query.OrderBy(a => a.EmployeeEvaluationPeriodTitle, sort.SortOrder);
        case EmployeeEvaluationSortType.OrganizationPostTitle:
          return query.OrderBy(a => a.OrganizationPostTitle, sort.SortOrder);
        case EmployeeEvaluationSortType.CreatedDateTime:
          return query.OrderBy(a => a.CreatedDateTime, sort.SortOrder);
        case EmployeeEvaluationSortType.FinalDateTime:
          return query.OrderBy(a => a.FinalDateTime, sort.SortOrder);
        case EmployeeEvaluationSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case EmployeeEvaluationSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region GetEmployeeEvaluationEmployeFilter
    public IQueryable<EmployeeComboResult> GetEmployeeEvaluationEmployeFilter()
    {

      var currentEmployeeId = App.Providers.Security.CurrentLoginData.UserEmployeeId;

      var validEmployeeIds = GetValidEmployeeIdsToEvaluate();

      var employees = App.Internals.UserManagement.GetEmployees(e => e, ids: validEmployeeIds, excludedId: currentEmployeeId);

      var employeeComboResult = from employee in employees
                                select new EmployeeComboResult
                                {
                                  Id = employee.Id,
                                  UserId = employee.User.Id,
                                  EmployeeCode = employee.Code,
                                  FirstName = employee.FirstName,
                                  LastName = employee.LastName
                                };
      return employeeComboResult;

    }
    #endregion

    #region EmployeeEvaluationPermanentRegistration
    public void EmployeeEvaluationPermanentRegistrationProcess(int employeeEvaluationId, int evaluationCategoryId, byte[] rowVersion)
    {

      var currentUserId = App.Providers.Security.CurrentLoginData.UserId;

      var employeeEvaluationItem = GetEmployeeEvaluationItem(
                employeeEvaluationId: employeeEvaluationId,
                evaluationCategoryId: evaluationCategoryId);

      EditEmployeeEvaluationItem(
                employeeEvaluationItem: employeeEvaluationItem,
                rowVersion: rowVersion,
                status: EmployeeEvaluationStatus.Permanent,
                permanentDateTime: DateTime.UtcNow);
    }
    #endregion


  }
}
