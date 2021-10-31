using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using Microsoft.EntityFrameworkCore;
//using Parlar.DAL.Functions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.SoftwareWorkReport;
using lena.Models.Planning.SoftwareWorkReportItem;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Add
    public SoftwareWorkReport AddSoftwareWorkReport(
        int employeeId,
        DateTime reportDateTime)
    {
      var softwareWorkReport = repository.Create<SoftwareWorkReport>();
      softwareWorkReport.ReportDateTime = reportDateTime;
      softwareWorkReport.CreatedDateTime = DateTime.UtcNow;
      softwareWorkReport.EmployeeId = employeeId;
      repository.Add(softwareWorkReport);
      return softwareWorkReport;
    }
    #endregion
    #region Add Process
    public SoftwareWorkReport AddSoftwareWorkReportProcess(
        int employeeId,
        DateTime reportDateTime,
        AddSoftwareWorkReportItemInput[] addSoftwareWorkReportItemInputs)
    {
      var loginedEmployeeId = App.Providers.Security.CurrentLoginData.UserEmployeeId;
      if (employeeId != loginedEmployeeId)
      {
        throw new YouDoNotHaveAccessToReportForAnotherUserException();
      }
      var currentWorkReport = GetSoftwareWorkReports(
                selector: e => e,
                employeeId: employeeId,
                reportDateTime: reportDateTime)
                .FirstOrDefault();
      if (currentWorkReport != null)
      {
        throw new ThereIsReportForThisDayException(id: currentWorkReport.Id);
      }
      var softwareWorkReport = AddSoftwareWorkReport(
                employeeId: employeeId,
                reportDateTime: reportDateTime);
      foreach (var item in addSoftwareWorkReportItemInputs)
      {
        AddSoftwareWorkReportItem(
                  softwareWorkReportId: softwareWorkReport.Id,
                  issue: item.Issue,
                  spent: item.Spent,
                  restTimeIssue: item.RestTimeIssue,
                  estimated: item.Estimated);
      }
      return softwareWorkReport;
    }
    #endregion
    #region Edit
    public SoftwareWorkReport EditSoftwareWorkReport(
        byte[] rowVersion,
        int id,
        TValue<int> employeeId = null,
        TValue<DateTime> reportDateTime = null)
    {
      var softwareWorkReport = GetSoftwareWorkReport(id: id);
      EditSoftwareWorkReport(
                softwareWorkReport: softwareWorkReport,
                rowVersion: rowVersion,
                employeeId: employeeId,
                reportDateTime: reportDateTime);
      return softwareWorkReport;
    }
    public SoftwareWorkReport EditSoftwareWorkReport(
        SoftwareWorkReport softwareWorkReport,
        byte[] rowVersion,
        TValue<int> employeeId = null,
        TValue<DateTime> reportDateTime = null)
    {
      if (employeeId != null)
        softwareWorkReport.EmployeeId = employeeId;
      if (reportDateTime != null)
        softwareWorkReport.ReportDateTime = reportDateTime;
      repository.Update(softwareWorkReport, rowVersion);
      return softwareWorkReport;
    }
    #endregion
    #region Edit Process
    public SoftwareWorkReport EditSoftwareWorkReportProcess(
        byte[] rowVersion,
        int id,
        DateTime reportDateTime,
        AddSoftwareWorkReportItemInput[] addSoftwareWorkReportItemInputs,
        EditSoftwareWorkReportItemInput[] editSoftwareWorkReportItemInputs,
        int[] deletedIds)
    {
      var softwareWorkReport = GetSoftwareWorkReport(id: id);
      var loginedEmployeeId = App.Providers.Security.CurrentLoginData.UserEmployeeId;
      if (softwareWorkReport.EmployeeId != loginedEmployeeId)
      {
        throw new YouDoNotHaveAccessToReportForAnotherUserException();
      }
      if (reportDateTime != softwareWorkReport.ReportDateTime)
      {
        var currentWorkReport = GetSoftwareWorkReports(
                selector: e => e,
                employeeId: softwareWorkReport.EmployeeId,
                reportDateTime: reportDateTime)
                .FirstOrDefault();
        if (currentWorkReport != null)
        {
          throw new ThereIsReportForThisDayException(id: id);
        }
      }
      softwareWorkReport = EditSoftwareWorkReport(
                softwareWorkReport: softwareWorkReport,
                       reportDateTime: reportDateTime,
                       rowVersion: rowVersion);
      foreach (var item in addSoftwareWorkReportItemInputs)
      {
        AddSoftwareWorkReportItem(
                 softwareWorkReportId: softwareWorkReport.Id,
                 issue: item.Issue,
                 spent: item.Spent,
                 restTimeIssue: item.RestTimeIssue,
                 estimated: item.Estimated);
      }
      foreach (var item in editSoftwareWorkReportItemInputs)
      {
        EditSoftwareWorkReportItem(
                  id: item.Id,
                  rowVersion: item.RowVersion,
                  spent: item.Spent,
                  estimated: item.Estimated,
                  issue: item.Issue);
      }
      foreach (var item in deletedIds)
      {
        DeleteSoftwareWorkReportItem(id: item);
      }
      return softwareWorkReport;
    }
    #endregion
    #region Get
    public SoftwareWorkReport GetSoftwareWorkReport(int id) => GetSoftwareWorkReport(selector: e => e, id: id);
    public TResult GetSoftwareWorkReport<TResult>(
        Expression<Func<SoftwareWorkReport, TResult>> selector,
        int id)
    {
      var softwareWorkReport = GetSoftwareWorkReports(
                selector: selector,
                id: id).FirstOrDefault();
      if (softwareWorkReport == null)
        throw new SoftwareWorkReportNotFoundException(id);
      return softwareWorkReport;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetSoftwareWorkReports<TResult>(
        Expression<Func<SoftwareWorkReport, TResult>> selector,
        TValue<int> id = null,
        TValue<int> employeeId = null,
        TValue<DateTime> reportDateTime = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null)
    {
      var softwareWorkReports = repository.GetQuery<SoftwareWorkReport>();
      if (id != null)
        softwareWorkReports = softwareWorkReports.Where(x => x.Id == id);
      if (reportDateTime != null)
        softwareWorkReports = softwareWorkReports.Where(i => i.ReportDateTime == reportDateTime);
      if (employeeId != null)
        softwareWorkReports = softwareWorkReports.Where(i => i.EmployeeId == employeeId);
      if (fromDateTime != null)
        softwareWorkReports = softwareWorkReports.Where(i => i.ReportDateTime >= fromDateTime);
      if (toDateTime != null)
        softwareWorkReports = softwareWorkReports.Where(i => i.ReportDateTime <= toDateTime);
      return softwareWorkReports.Select(selector);
    }
    #endregion
    #region Remove SoftwareWorkReport
    public void DeleteSoftwareWorkReport(int id)
    {
      var softwareWorkReport = GetSoftwareWorkReport(id: id);
      DeleteSoftwareWorkReport(softwareWorkReport);
    }
    public void DeleteSoftwareWorkReport(SoftwareWorkReport softwareWorkReport)
    {
      repository.Delete(softwareWorkReport);
    }
    #endregion
    #region Search
    public IQueryable<SoftwareWorkReportFullResult> SearchSoftwareWorkReport(IQueryable<SoftwareWorkReportFullResult> query,
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
    public IOrderedQueryable<SoftwareWorkReportFullResult> SortSoftwareWorkReportResult(IQueryable<SoftwareWorkReportFullResult> query,
        SortInput<SoftwareWorkReportSortType> sort)
    {
      switch (sort.SortType)
      {
        case SoftwareWorkReportSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case SoftwareWorkReportSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case SoftwareWorkReportSortType.CreatedDateTime:
          return query.OrderBy(a => a.CreatedDateTime, sort.SortOrder);
        case SoftwareWorkReportSortType.ReportDateTime:
          return query.OrderBy(a => a.ReportDateTime, sort.SortOrder);
        case SoftwareWorkReportSortType.Issue:
          return query.OrderBy(a => a.Issue, sort.SortOrder);
        case SoftwareWorkReportSortType.Spent:
          return query.OrderBy(a => a.SpentGitFormat, sort.SortOrder);
        case SoftwareWorkReportSortType.Estimated:
          return query.OrderBy(a => a.SpentTimeFormat, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToSoftwareWorkReportResult
    public Expression<Func<SoftwareWorkReport, SoftwareWorkReportResult>> ToSoftwareWorkReportResult =
    softwareWorkReport => new SoftwareWorkReportResult
    {
      Id = softwareWorkReport.Id,
      EmployeeId = softwareWorkReport.EmployeeId,
      EmployeeFullName = softwareWorkReport.Employee.FirstName + " " + softwareWorkReport.Employee.LastName,
      CreatedDateTime = softwareWorkReport.CreatedDateTime,
      ReportDateTime = softwareWorkReport.ReportDateTime,
      RowVersion = softwareWorkReport.RowVersion,
    };
    public IQueryable<SoftwareWorkReportFullResult> ToSoftwareWorkReportFullResult(IQueryable<SoftwareWorkReport> softwareWorkReports)
    {
      var result = from softwareWorkReport in softwareWorkReports
                   from softwareWorkReportItem in softwareWorkReport.SoftwareWorkReportItems
                   select new SoftwareWorkReportFullResult
                   {
                     Id = softwareWorkReport.Id,
                     SoftwareWorkReportItemId = softwareWorkReportItem.Id,
                     EmployeeId = softwareWorkReport.EmployeeId,
                     EmployeeFullName = softwareWorkReport.Employee.FirstName + " " + softwareWorkReport.Employee.LastName,
                     Spent = softwareWorkReportItem.Spent,
                     Estimated = softwareWorkReportItem.Estimated,
                     SpentGitFormat = MyDbFunction.ConvertMinuteToTime(softwareWorkReportItem.Spent, true),
                     SpentTimeFormat = MyDbFunction.ConvertMinuteToTime(softwareWorkReportItem.Spent, false),
                     EstimatedTimeFormat = MyDbFunction.ConvertMinuteToTime(softwareWorkReportItem.Estimated, false),
                     EstimatedGitFormat = MyDbFunction.ConvertMinuteToTime(softwareWorkReportItem.Estimated, true),
                     CreatedDateTime = softwareWorkReport.CreatedDateTime,
                     Issue = softwareWorkReportItem.Issue,
                     ReportDateTime = softwareWorkReport.ReportDateTime,
                     RowVersion = softwareWorkReport.RowVersion,
                     RestTimeIssue = softwareWorkReportItem.RestTimeIssue
                   };
      return result;
    }
    #endregion
  }
}