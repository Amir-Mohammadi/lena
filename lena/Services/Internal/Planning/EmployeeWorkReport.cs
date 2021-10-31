using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.StaticData;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.EmployeeWorkReportItem;
using lena.Models.UserManagement.OrganizationPosts;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Add
    public EmployeeWorkReport AddEmployeeWorkReport(
        int employeeId,
        DateTime reportDateTime,
        TValue<int?> projectERPTaskId = null)
    {
      var employeeWorkReport = repository.Create<EmployeeWorkReport>();
      employeeWorkReport.ReportDateTime = reportDateTime;
      employeeWorkReport.EmployeeId = employeeId;
      employeeWorkReport.ProjectERPTaskId = projectERPTaskId;
      employeeWorkReport.UserId = App.Providers.Security.CurrentLoginData.UserId;
      employeeWorkReport.DateTime = DateTime.UtcNow;
      repository.Add(employeeWorkReport);
      return employeeWorkReport;
    }
    #endregion
    #region Add Process
    public EmployeeWorkReport AddEmployeeWorkReportProcess(
        int employeeId,
        int? projectERPTaskId,
        DateTime reportDateTime,
        AddEmployeeWorkReportItemInput[] addEmployeeWorkReportItemInputs)
    {
      var currentWorkReport = GetEmployeeWorkReports(
                selector: e => e,
                employeeId: employeeId,
                projectERPTaskId: projectERPTaskId,
                reportDateTime: reportDateTime)
                .FirstOrDefault();
      if (currentWorkReport != null)
      {
        throw new ThereIsReportForThisDayException(id: currentWorkReport.Id);
      }
      #region Check ProjectERPTask Not Assigend To Employee
      if (projectERPTaskId != null)
      {
        var projectERPTasks = App.Internals.ProjectManagement.GetProjectERPTasks(e => e,
                  id: projectERPTaskId,
                  assigneeEmployeeId: employeeId
                  );
        if (!projectERPTasks.Any())
          throw new ProjectERPTaskNotAssignedToEmployeeException(projectERPTaskId: projectERPTaskId.Value, employeeId: employeeId);
      }
      #endregion
      var thresholdDays = App.Internals.ApplicationSetting.GetThresholdDateValue() * -1;
      var dateTimeNow = DateTime.UtcNow;
      var thresholdDate = dateTimeNow.AddDays(thresholdDays);
      if (reportDateTime < thresholdDate)
      {
        throw new DonNotAbaleToRegisterReportForThePreviousDays();
      }
      //check overlap here
      foreach (var item in addEmployeeWorkReportItemInputs)
      {
        var count = addEmployeeWorkReportItemInputs.Where(
                  m => !(
                    (item.FromTime <= m.FromTime && item.ToTime <= m.FromTime) ||
                    (item.FromTime >= m.ToTime && item.ToTime >= m.ToTime))
                  ).Count();
        if (count > 1)
        {
          throw new ThereIsTimeOverlapException();
        }
      }
      var employeeWorkReport = AddEmployeeWorkReport(
                employeeId: employeeId,
                projectERPTaskId: projectERPTaskId,
                reportDateTime: reportDateTime);
      foreach (var item in addEmployeeWorkReportItemInputs)
      {
        if (item.ToTime < item.FromTime)
        {
          throw new ToTimeCannotBiggerThanFromTimeException();
        }
        AddEmployeeWorkReportItem(
                  employeeWorkReportId: employeeWorkReport.Id,
                  operation: item.Operation,
                  fromTime: item.FromTime,
                  toTime: item.ToTime,
                  description: item.Description);
      }
      return employeeWorkReport;
    }
    #endregion
    #region Edit
    public EmployeeWorkReport EditEmployeeWorkReport(
        byte[] rowVersion,
        int id,
        TValue<short> employeeId = null,
        TValue<int> projectERPTaskId = null,
        TValue<DateTime> reportDateTime = null)
    {
      var employeeWorkReport = GetEmployeeWorkReport(id: id);
      EditEmployeeWorkReport(
                employeeWorkReport: employeeWorkReport,
                rowVersion: rowVersion,
                employeeId: employeeId,
                projectERPTaskId: projectERPTaskId.Value,
                reportDateTime: reportDateTime);
      return employeeWorkReport;
    }
    public EmployeeWorkReport EditEmployeeWorkReport(
        EmployeeWorkReport employeeWorkReport,
        byte[] rowVersion,
        TValue<short> employeeId = null,
        TValue<DateTime> reportDateTime = null,
        TValue<int> projectERPTaskId = null)
    {
      employeeWorkReport.UserId = App.Providers.Security.CurrentLoginData.UserId;
      if (employeeId != null)
        employeeWorkReport.EmployeeId = employeeId;
      if (reportDateTime != null)
        employeeWorkReport.ReportDateTime = reportDateTime;
      if (projectERPTaskId != null)
        employeeWorkReport.ProjectERPTaskId = projectERPTaskId;
      repository.Update(employeeWorkReport, rowVersion);
      return employeeWorkReport;
    }
    #endregion
    #region Edit Process
    public EmployeeWorkReport EditEmployeeWorkReportProcess(
        byte[] rowVersion,
        int id,
        DateTime reportDateTime,
        AddEmployeeWorkReportItemInput[] addEmployeeWorkReportItemInputs,
        EditEmployeeWorkReportItemInput[] editEmployeeWorkReportItemInputs,
        int[] deletedIds,
        TValue<int> projectERPTaskId = null)
    {
      var employeeWorkReport = GetEmployeeWorkReport(id: id);
      if (reportDateTime != employeeWorkReport.ReportDateTime)
      {
        var currentWorkReport = GetEmployeeWorkReports(
                selector: e => e,
                employeeId: employeeWorkReport.EmployeeId,
                projectERPTaskId: projectERPTaskId,
                reportDateTime: reportDateTime)
                .FirstOrDefault();
        if (currentWorkReport != null)
        {
          throw new ThereIsReportForThisDayException(id: id);
        }
      }
      #region Check ProjectERPTask Not Assigend To Employee
      if (projectERPTaskId != null)
      {
        var projectERPTasks = App.Internals.ProjectManagement.GetProjectERPTasks(e => e,
                  id: projectERPTaskId.Value,
                  assigneeEmployeeId: employeeWorkReport.EmployeeId
                  );
        if (!projectERPTasks.Any())
          throw new ProjectERPTaskNotAssignedToEmployeeException(projectERPTaskId: projectERPTaskId.Value, employeeId: employeeWorkReport.EmployeeId);
      }
      #endregion
      var thresholdDays = App.Internals.ApplicationSetting.GetThresholdDateValue() * -1;
      var dateTimeNow = DateTime.UtcNow;
      var thresholdDate = dateTimeNow.AddDays(thresholdDays);
      if (reportDateTime < thresholdDate)
      {
        throw new DonNotAbaleToRegisterReportForThePreviousDays();
      }
      var dateTimes = addEmployeeWorkReportItemInputs.Select(m =>
            new
            {
              FromTime = m.FromTime,
              ToTime = m.ToTime
            }).Union(editEmployeeWorkReportItemInputs.Select(m =>
            new
            {
              FromTime = m.FromTime,
              ToTime = m.ToTime
            }));
      foreach (var item in dateTimes)
      {
        var count = dateTimes.Where(
                  m => !(
                    (item.FromTime <= m.FromTime && item.ToTime <= m.FromTime) ||
                    (item.FromTime >= m.ToTime && item.ToTime >= m.ToTime))
                  ).Count();
        if (count > 1)
        {
          throw new ThereIsTimeOverlapException();
        }
      }
      employeeWorkReport = EditEmployeeWorkReport(
                employeeWorkReport: employeeWorkReport,
                       projectERPTaskId: projectERPTaskId,
                       reportDateTime: reportDateTime,
                       rowVersion: rowVersion);
      foreach (var item in addEmployeeWorkReportItemInputs)
      {
        if (item.ToTime < item.FromTime)
        {
          throw new ToTimeCannotBiggerThanFromTimeException();
        }
        AddEmployeeWorkReportItem(
                  employeeWorkReportId: employeeWorkReport.Id,
                  operation: item.Operation,
                  fromTime: item.FromTime,
                  toTime: item.ToTime,
                  description: item.Description);
      }
      foreach (var item in editEmployeeWorkReportItemInputs)
      {
        if (item.ToTime < item.FromTime)
        {
          throw new ToTimeCannotBiggerThanFromTimeException();
        }
        EditEmployeeWorkReportItem(
                  id: item.Id,
                  rowVersion: item.RowVersion,
                  fromTime: item.FromTime,
                  toTime: item.ToTime,
                  operation: item.Operation,
                  description: item.Description);
      }
      foreach (var item in deletedIds)
      {
        DeleteEmployeeWorkReportItem(id: item);
      }
      return employeeWorkReport;
    }
    #endregion
    #region Get
    public EmployeeWorkReport GetEmployeeWorkReport(int id) => GetEmployeeWorkReport(selector: e => e, id: id);
    public TResult GetEmployeeWorkReport<TResult>(
        Expression<Func<EmployeeWorkReport, TResult>> selector,
        int id)
    {
      var employeeWorkReport = GetEmployeeWorkReports(
                selector: selector,
                id: id).FirstOrDefault();
      if (employeeWorkReport == null)
        throw new EmployeeWorkReportNotFoundException(id);
      return employeeWorkReport;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetEmployeeWorkReports<TResult>(
        Expression<Func<EmployeeWorkReport, TResult>> selector,
        TValue<int> id = null,
        TValue<int> employeeId = null,
        TValue<int> projectERPTaskId = null,
        TValue<int> userId = null,
        TValue<DateTime> reportDateTime = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<short> departmentId = null,
        TValue<int> organizationPostId = null
        )
    {
      var employeeIds = GetAllowedEmployeeIds();
      var employeeWorkReports = repository.GetQuery<EmployeeWorkReport>();
      if (projectERPTaskId != null)
        employeeWorkReports = employeeWorkReports.Where(i => i.ProjectERPTaskId == projectERPTaskId);
      if (employeeIds != null)
        employeeWorkReports = employeeWorkReports.Where(i => employeeIds.Contains(i.EmployeeId));
      if (id != null)
        employeeWorkReports = employeeWorkReports.Where(x => x.Id == id);
      if (employeeId != null)
        employeeWorkReports = employeeWorkReports.Where(i => i.EmployeeId == employeeId);
      if (userId != null)
        employeeWorkReports = employeeWorkReports.Where(i => i.EmployeeId == userId);
      if (fromDateTime != null)
        employeeWorkReports = employeeWorkReports.Where(i => i.ReportDateTime >= fromDateTime);
      if (toDateTime != null)
        employeeWorkReports = employeeWorkReports.Where(i => i.ReportDateTime <= toDateTime);
      if (departmentId != null)
        employeeWorkReports = employeeWorkReports.Where(i => i.Employee.DepartmentId == departmentId);
      if (reportDateTime != null)
        employeeWorkReports = employeeWorkReports.Where(i => i.ReportDateTime == reportDateTime);
      if (organizationPostId != null)
        employeeWorkReports = employeeWorkReports.Where(i => i.Employee.OrgnizationPostId == organizationPostId);
      return employeeWorkReports.Select(selector);
    }
    #endregion
    #region Remove EmployeeWorkReport
    public void RemoveEmployeeWorkReport(int id, byte[] rowVersion)
    {
      var employeeWorkReport = GetEmployeeWorkReport(id: id);
    }
    #endregion
    #region Delete EmployeeWorkReport
    public void DeleteEmployeeWorkReport(int id)
    {
      var employeeWorkReport = GetEmployeeWorkReport(id: id);
      repository.Delete(employeeWorkReport);
    }
    #endregion
    #region Search
    public IQueryable<EmployeeWorkReportResult> SearchEmployeeWorkReport(IQueryable<EmployeeWorkReportResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.EmployeeFullName.Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<EmployeeWorkReportResult> SortEmployeeWorkReportResult(IQueryable<EmployeeWorkReportResult> query,
        SortInput<EmployeeWorkReportSortType> sort)
    {
      switch (sort.SortType)
      {
        case EmployeeWorkReportSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case EmployeeWorkReportSortType.EmployeeCode:
          return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case EmployeeWorkReportSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case EmployeeWorkReportSortType.OrganizationPostTitle:
          return query.OrderBy(a => a.OrganizationPostTitle, sort.SortOrder);
        case EmployeeWorkReportSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case EmployeeWorkReportSortType.RegistrantEmployeeFullName:
          return query.OrderBy(a => a.RegistrantEmployeeFullName, sort.SortOrder);
        case EmployeeWorkReportSortType.ReportDateTime:
          return query.OrderBy(a => a.ReportDateTime, sort.SortOrder);
        case EmployeeWorkReportSortType.TotalEmployeeWorkReportDurationInSecond:
          return query.OrderBy(a => a.TotalEmployeeWorkReportDurationInSecond, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToEmployeeWorkReportResult
    public Expression<Func<EmployeeWorkReport, EmployeeWorkReportResult>> ToEmployeeWorkReportResult =
        employeeWorkReport => new EmployeeWorkReportResult
        {
          Id = employeeWorkReport.Id,
          EmployeeId = employeeWorkReport.Employee.Id,
          EmployeeCode = employeeWorkReport.Employee.Code,
          EmployeeFullName = employeeWorkReport.Employee.FirstName + " " + employeeWorkReport.Employee.LastName,
          DepartmentId = employeeWorkReport.Employee.DepartmentId,
          DepartmentName = employeeWorkReport.Employee.Department.Name,
          OrganizationPostId = employeeWorkReport.Employee.OrgnizationPostId,
          OrganizationPostTitle = employeeWorkReport.Employee.OrganizationPost.Title ?? "تعریف نشده است",
          UserId = employeeWorkReport.UserId,
          RegistrantEmployeeFullName = employeeWorkReport.User.Employee.FirstName + " " + employeeWorkReport.User.Employee.LastName,
          DateTime = employeeWorkReport.DateTime,
          ReportDateTime = employeeWorkReport.ReportDateTime,
          RowVersion = employeeWorkReport.RowVersion
        };
    public IQueryable<EmployeeWorkReportResult> ToEmployeeWorkReportFullResult(IQueryable<EmployeeWorkReport> employeeWorkReports)
    {
      var workItems = from employeeWorkReport in employeeWorkReports
                      from employeeWorkReportItem in employeeWorkReport.EmployeeWorkReportItems
                      select new
                      {
                        EmployeeWorkReportId = employeeWorkReportItem.EmployeeWorkReportId,
                        EmployeeWorkReportDuration = EF.Functions.DateDiffSecond(employeeWorkReportItem.FromTime, employeeWorkReportItem.ToTime)
                      };
      var groupWorkItems = from workItem in workItems
                           group workItem by workItem.EmployeeWorkReportId into g
                           select new
                           {
                             EmployeeWorkReportId = g.Key,
                             TotalEmployeeWorkReportDuration = g.Sum(m => m.EmployeeWorkReportDuration),
                           };
      var result = from employeeWorkReport in employeeWorkReports
                   join groupWorkItem in groupWorkItems on employeeWorkReport.Id equals groupWorkItem.EmployeeWorkReportId
                   select new EmployeeWorkReportResult
                   {
                     Id = employeeWorkReport.Id,
                     EmployeeId = employeeWorkReport.Employee.Id,
                     EmployeeCode = employeeWorkReport.Employee.Code,
                     EmployeeFullName = employeeWorkReport.Employee.FirstName + " " + employeeWorkReport.Employee.LastName,
                     DepartmentId = employeeWorkReport.Employee.DepartmentId,
                     DepartmentName = employeeWorkReport.Employee.Department.Name,
                     OrganizationPostId = employeeWorkReport.Employee.OrgnizationPostId,
                     OrganizationPostTitle = employeeWorkReport.Employee.OrganizationPost.Title ?? "تعریف نشده است",
                     UserId = employeeWorkReport.UserId,
                     RegistrantEmployeeFullName = employeeWorkReport.User.Employee.FirstName + " " + employeeWorkReport.User.Employee.LastName,
                     DateTime = employeeWorkReport.DateTime,
                     ReportDateTime = employeeWorkReport.ReportDateTime,
                     TotalEmployeeWorkReportDurationInSecond = (double)groupWorkItem.TotalEmployeeWorkReportDuration,
                     RowVersion = employeeWorkReport.RowVersion
                   };
      return result;
    }
    #endregion
    #region GetAllowedEmployeeIds
    public int[] GetAllowedEmployeeIds()
    {
      #region Check Confirm Permission And GetEmployeeId
      var employeeIdList = new List<int>();
      var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
                    actionName: StaticActionName.GetAllEmployeeWorkReportList,
                    actionParameters: null);
      if (checkPermissionResult.AccessType == AccessType.Denied)
      {
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
          var departmentIds = App.Internals.ApplicationBase.GetDepartments(recursiveParentDepartmentId: empolyee.DepartmentId)
                    .Select(m => m.Id)
                    .ToArray();
          var employeeIds = App.Internals.UserManagement.GetEmployees(
                    e => e.Id,
                    departmentIds: departmentIds)
                .ToList();
          employeeIdList.AddRange(employeeIds);
        }
        else
        {
          employeeIdList.Add(empolyee.Id);
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
    #region GetAllowedDepartmentIds
    public short[] GetAllowedDepartmentIds()
    {
      #region Check Confirm Permission And GetEmployeeId
      var allowedDepartmentIds = new List<short>();
      var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
                    actionName: StaticActionName.GetAllEmployeeWorkReportList,
                    actionParameters: null);
      if (checkPermissionResult.AccessType == AccessType.Denied)
      {
        var currentEmployee = App.Providers.Security.CurrentLoginData;
        var isAdmin = App.Internals.UserManagement.GetEmployee(e => e.OrganizationPost.IsAdmin, id: currentEmployee.UserEmployeeId.Value);
        if (isAdmin)
        {
          var departmentIds = App.Internals.ApplicationBase.GetDepartments(recursiveParentDepartmentId: currentEmployee.DepartmentId.Value)
                    .Select(m => m.Id)
                    .ToList();
          allowedDepartmentIds.AddRange(departmentIds);
        }
        else
        {
          allowedDepartmentIds.Add(currentEmployee.DepartmentId.Value);
        }
      }
      return allowedDepartmentIds.ToArray();
      #endregion
    }
    #endregion
    #region GetEmployeeWorkReportEmployeFilter
    public IQueryable<EmployeeComboResult> GetEmployeeWorkReportEmployeFilter()
    {
      var employeeIds = GetAllowedEmployeeIds();
      var employees = App.Internals.UserManagement.GetEmployees(e => e, ids: employeeIds);
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
    #region GetEmployeeWorkReportEmployeFilter
    public IQueryable<DepartmentComboResult> GetEmployeeWorkReportDepartmentFilter()
    {
      var employeeIds = GetAllowedEmployeeIds();
      var employees = App.Internals.UserManagement.GetEmployees(e => e, ids: employeeIds);
      var departments = App.Internals.ApplicationBase.GetDepartments();
      var groupEmployees = from employee in employees.Where(m => m.DepartmentId != null)
                           group employee by employee.DepartmentId into g
                           select new
                           {
                             DepartmentId = g.Key
                           };
      var departmentComboResult = from groupEmployee in groupEmployees
                                  join department in departments on groupEmployee.DepartmentId equals department.Id
                                  select new DepartmentComboResult
                                  {
                                    Id = department.Id,
                                    Name = department.Name
                                  };
      return departmentComboResult;
    }
    #endregion
    #region GetEmployeeWorkReportEmployeFilter
    public IQueryable<OrganizationPostComboResult> GetEmployeeWorkReportOrganizationPostFilter()
    {
      var employeeIds = GetAllowedEmployeeIds();
      var employees = App.Internals.UserManagement.GetEmployees(e => e, ids: employeeIds);
      var orgnizationPosts = App.Internals.UserManagement.GetOrganizationPosts(e => e);
      var groupEmployees = from employee in employees.Where(m => m.OrgnizationPostId != null)
                           group employee by employee.OrgnizationPostId into g
                           select new
                           {
                             OrgnizationPostId = g.Key
                           };
      var organizationPostComboResult = from groupEmployee in groupEmployees
                                        join orgnizationPost in orgnizationPosts on groupEmployee.OrgnizationPostId equals orgnizationPost.Id
                                        select new OrganizationPostComboResult
                                        {
                                          Id = orgnizationPost.Id,
                                          Title = orgnizationPost.Title
                                        };
      return organizationPostComboResult;
    }
    #endregion
  }
}