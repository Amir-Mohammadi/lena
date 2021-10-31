using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Common.Helpers;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ReportManagement.Exception;
using lena.Services.Internals.Reports.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Reports;
using Stimulsoft.Report;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ReportManagement
{
  public partial class ReportManagement
  {
    #region Get
    public ReportVersion GetReportVersion(int id) => GetReportVersion(selector: e => e, id: id);
    public TResult GetReportVersion<TResult>(
        Expression<Func<ReportVersion, TResult>> selector,
        int id)
    {
      var reportVersion = GetReportVersions(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (reportVersion == null)
        throw new ReportVersionNotFoundException(id);
      return reportVersion;
    }
    #endregion
    #region GetByReportName
    public ReportVersion GetPublishedReportVersionByReportName(
      string reportName,
      bool isForExport = false,
      StimulExportFormat? exportFormat = null,
      bool throwExceptionIfNotExist = true)
      =>
        GetPublishedReportVersionByReportName(
            selector: e => e,
            reportName: reportName,
            isForExport: isForExport,
            exportFormat: exportFormat,
            throwExceptionIfNotExist: throwExceptionIfNotExist);
    public TResult GetPublishedReportVersionByReportName<TResult>(
        Expression<Func<ReportVersion, TResult>> selector,
        string reportName,
        bool isForExport = false,
        StimulExportFormat? exportFormat = null,
        bool throwExceptionIfNotExist = true)
    {
      TResult reportVersion = default(TResult);
      if (isForExport == true && exportFormat != null)
      {
        reportVersion = GetReportVersions(selector: selector, reportName: reportName, isForExport: isForExport, exportFormat: exportFormat, isPublished: true)
              .FirstOrDefault();
        if (reportVersion == null)
          reportVersion = GetReportVersions(selector: selector, reportName: reportName, isForExport: isForExport, isPublished: true)
           .FirstOrDefault();
      }
      if (reportVersion == null)
        reportVersion = GetReportVersions(selector: selector, reportName: reportName, isPublished: true)
                  .FirstOrDefault();
      if (reportVersion == null && throwExceptionIfNotExist == true)
        throw new ReportVersionNotFoundException(reportName: reportName);
      return reportVersion;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetReportVersions<TResult>(
            Expression<Func<ReportVersion, TResult>> selector,
            TValue<int> id = null,
            TValue<string> apiUrl = null,
            TValue<string> reportContent = null,
            TValue<bool> isPublished = null,
            TValue<DateTime> creationTime = null,
            TValue<int> creatorUserId = null,
            TValue<int> reportId = null,
            TValue<string> reportName = null,
            TValue<string> cultureName = null,
            TValue<bool> isForExport = null,
            TValue<StimulExportFormat> exportFormat = null,
            TValue<bool> isBarcodeTemplate = null)
    {
      var query = repository.GetQuery<ReportVersion>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (apiUrl != null)
        query = query.Where(x => x.ApiUrl == apiUrl);
      if (reportContent != null)
        query = query.Where(x => x.ReportContent == reportContent);
      if (isPublished != null)
        query = query.Where(x => x.IsPublished == isPublished);
      if (creationTime != null)
        query = query.Where(x => x.CreationTime == creationTime);
      if (creatorUserId != null)
        query = query.Where(x => x.CreatorUserId == creatorUserId);
      if (reportId != null)
        query = query.Where(x => x.ReportId == reportId);
      if (reportName != null)
        query = query.Where(x => x.Report.Name == reportName);
      if (cultureName != null)
        query = query.Where(x => x.CultureName == cultureName);
      if (isForExport != null)
        query = query.Where(x => x.IsForExport == isForExport);
      if (exportFormat != null)
        query = query.Where(x => x.ExportFormat == exportFormat);
      if (isBarcodeTemplate != null)
        query = query.Where(x => x.IsBarcodeTemplate == isBarcodeTemplate);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ReportVersion AddReportVersion(
            string apiUrl,
            string reportContent,
            bool isPublished,
            int reportId,
            string cultureName,
            bool isForExport,
            StimulExportFormat? exportFormat,
            bool isBarcodeTemplate)
    {
      var reportVersion = repository.Create<ReportVersion>();
      reportVersion.ApiUrl = apiUrl;
      reportVersion.ReportContent = reportContent;
      reportVersion.IsPublished = isPublished;
      reportVersion.CreationTime = DateTime.Now.ToUniversalTime();
      reportVersion.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      reportVersion.ReportId = reportId;
      reportVersion.CultureName = cultureName;
      reportVersion.IsForExport = isForExport;
      reportVersion.ExportFormat = exportFormat;
      reportVersion.IsBarcodeTemplate = isBarcodeTemplate;
      repository.Add(reportVersion);
      return reportVersion;
    }
    #endregion
    #region Edit
    public ReportVersion EditReportVersion(
        int id,
        byte[] rowVersion,
        TValue<string> apiUrl = null,
        TValue<string> reportContent = null,
        TValue<bool> isPublished = null,
        TValue<int> reportId = null,
        TValue<bool> isForExport = null,
        TValue<StimulExportFormat> exportFormat = null,
        TValue<bool> isBarcodeTemplate = null)
    {
      var reportVersion = GetReportVersion(id: id);
      return EditReportVersion(
                    reportVersion: reportVersion,
                    rowVersion: rowVersion,
                    apiUrl: apiUrl,
                    reportContent: reportContent,
                    isPublished: isPublished,
                    reportId: reportId,
                    isForExport: isForExport,
                    exportFormat: exportFormat,
                    isBarcodeTemplate: isBarcodeTemplate
                    );
    }
    public ReportVersion EditReportVersion(
        ReportVersion reportVersion,
        byte[] rowVersion,
        TValue<string> apiUrl = null,
        TValue<string> reportContent = null,
        TValue<bool> isPublished = null,
        TValue<int> reportId = null,
        TValue<string> cultureName = null,
         TValue<bool> isForExport = null,
        TValue<StimulExportFormat> exportFormat = null,
        TValue<bool> isBarcodeTemplate = null)
    {
      if (apiUrl != null)
        reportVersion.ApiUrl = apiUrl;
      if (reportContent != null)
        reportVersion.ReportContent = reportContent;
      if (isPublished != null)
        reportVersion.IsPublished = isPublished;
      if (reportId != null)
        reportVersion.ReportId = reportId;
      if (cultureName != null)
        reportVersion.CultureName = cultureName;
      if (isForExport != null)
        reportVersion.IsForExport = isForExport;
      if (exportFormat != null)
        reportVersion.ExportFormat = exportFormat;
      if (isBarcodeTemplate != null)
        reportVersion.IsBarcodeTemplate = isBarcodeTemplate;
      repository.Update(
                rowVersion: rowVersion,
                entity: reportVersion);
      return reportVersion;
    }
    #endregion
    #region Delete
    public void DeleteReportVersionProcess(int id)
    {
      #region GetReportVersion and reportId
      var reportVersion = GetReportVersion(id: id);
      var reportId = reportVersion.ReportId;
      #endregion
      #region DeleteReportVersion
      repository.Delete(reportVersion);
      #endregion
      #region Check Exist ReportVersions if not exits not version for report delete it 
      var reportVersions = GetReportVersions(
          selector: e => e,
          reportId: reportId);
      if (!reportVersions.Any())
        DeleteReport(reportId);
      #endregion
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ReportVersionResult> SortReportVersionResult(
        IQueryable<ReportVersionResult> query,
        SortInput<ReportVersionSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReportVersionSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ReportVersionSortType.ApiUrl:
          return query.OrderBy(a => a.ApiUrl, sort.SortOrder);
        case ReportVersionSortType.IsPublished:
          return query.OrderBy(a => a.IsPublished, sort.SortOrder);
        case ReportVersionSortType.CreatorUserName:
          return query.OrderBy(a => a.CreatorUserName, sort.SortOrder);
        case ReportVersionSortType.CreatorEmployeeFullName:
          return query.OrderBy(a => a.CreatorEmployeeFullName, sort.SortOrder);
        case ReportVersionSortType.ReportName:
          return query.OrderBy(a => a.ReportName, sort.SortOrder);
        case ReportVersionSortType.CultureName:
          return query.OrderBy(a => a.CultureName, sort.SortOrder);
        case ReportVersionSortType.CreationTime:
          return query.OrderBy(a => a.CreationTime, sort.SortOrder);
        case ReportVersionSortType.IsForExport:
          return query.OrderBy(a => a.IsForExport, sort.SortOrder);
        case ReportVersionSortType.ExportFormat:
          return query.OrderBy(a => a.ExportFormat, sort.SortOrder);
        case ReportVersionSortType.IsBarcodeTemplate:
          return query.OrderBy(a => a.IsBarcodeTemplate, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ReportVersionResult> SearchReportVersionResult(
        IQueryable<ReportVersionResult> query,
        string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.ReportName.Contains(searchText) ||
                item.CreatorUserName.Contains(searchText) ||
                item.CreatorEmployeeFullName.Contains(searchText) ||
                item.ApiUrl.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ReportVersion, ReportVersionResult>> ToReportVersionResult =
                reportVersion => new ReportVersionResult
                {
                  Id = reportVersion.Id,
                  ApiUrl = reportVersion.ApiUrl,
                  IsPublished = reportVersion.IsPublished,
                  CreatorUserId = reportVersion.CreatorUserId,
                  CreatorUserName = reportVersion.CreatorUser.UserName,
                  CreatorEmployeeFullName = reportVersion.CreatorUser.Employee.FirstName + " " +
                                              reportVersion.CreatorUser.Employee.LastName,
                  ReportId = reportVersion.ReportId,
                  ReportName = reportVersion.Report.Name,
                  CreationTime = reportVersion.CreationTime,
                  RowVersion = reportVersion.RowVersion,
                  ReportRowVersion = reportVersion.Report.RowVersion,
                  Description = reportVersion.Report.Description,
                  CultureName = reportVersion.CultureName,
                  IsForExport = reportVersion.IsForExport,
                  ExportFormat = reportVersion.ExportFormat,
                  IsBarcodeTemplate = reportVersion.IsBarcodeTemplate
                };
    #endregion
    #region ToFullResult
    public Expression<Func<ReportVersion, FullReportVersionResult>> ToFullReportVersionResult =
        reportVersion => new FullReportVersionResult
        {
          Id = reportVersion.Id,
          ApiUrl = reportVersion.ApiUrl,
          ReportContent = reportVersion.ReportContent,
          IsPublished = reportVersion.IsPublished,
          CreatorUserId = reportVersion.CreatorUserId,
          CreatorUserName = reportVersion.CreatorUser.UserName,
          CreatorEmployeeFullName = reportVersion.CreatorUser.Employee.FirstName + " " +
                                      reportVersion.CreatorUser.Employee.LastName,
          ReportId = reportVersion.ReportId,
          ReportName = reportVersion.Report.Name,
          CreationTime = reportVersion.CreationTime,
          RowVersion = reportVersion.RowVersion,
          ReportRowVersion = reportVersion.Report.RowVersion,
          Description = reportVersion.Report.Description,
          CultureName = reportVersion.CultureName,
          IsForExport = reportVersion.IsForExport,
          ExportFormat = reportVersion.ExportFormat,
          IsBarcodeTemplate = reportVersion.IsBarcodeTemplate
        };
    #endregion
    #region AddProcess
    public ReportVersion AddReportVersionProcess(
        string apiUrl,
        string reportContent,
        bool isPublished,
        int reportId,
        string cultureName,
        bool isForExport,
        StimulExportFormat? exportFormat,
        bool isBarcodeTemplate)
    {
      if (isPublished == true)
      {
        #region Get Published ReportVersion
        var reportVersions = GetReportVersions(selector: e => e,
            reportId: reportId,
            isPublished: true);
        #endregion
        #region Edit Published ReportVersion
        foreach (var reportVersion in reportVersions)
        {
          EditReportVersion(reportVersion: reportVersion,
                        rowVersion: reportVersion.RowVersion,
                        isPublished: false);
        }
        #endregion
      }
      #region AddReportVersion
      return AddReportVersion(
          apiUrl: apiUrl,
          reportContent: reportContent,
          isPublished: isPublished,
          reportId: reportId,
          cultureName: cultureName,
          isForExport: isForExport,
          exportFormat: exportFormat,
          isBarcodeTemplate: isBarcodeTemplate);
      #endregion
    }
    #endregion
    #region EditProcess
    public ReportVersion EditReportVersionProcess(
        int id,
        byte[] rowVersion,
        TValue<string> apiUrl = null,
        TValue<string> reportContent = null,
        TValue<bool> isPublished = null,
        TValue<int> reportId = null,
        TValue<string> cultureName = null,
        TValue<bool> isForExport = null,
        TValue<StimulExportFormat> exportFormat = null,
        TValue<bool> isBarcodeTemplate = null)
    {
      var reportVersion = GetReportVersion(id: id);
      return EditReportVersionProcess(
                    reportVersion: reportVersion,
                    rowVersion: rowVersion,
                    apiUrl: apiUrl,
                    reportContent: reportContent,
                    isPublished: isPublished,
                    reportId: reportId,
                    cultureName: cultureName,
                    isForExport: isForExport,
                    exportFormat: exportFormat,
                    isBarcodeTemplate: isBarcodeTemplate);
    }
    public ReportVersion EditReportVersionProcess(
        ReportVersion reportVersion,
        byte[] rowVersion,
        TValue<string> apiUrl = null,
        TValue<string> reportContent = null,
        TValue<bool> isPublished = null,
        TValue<int> reportId = null,
        TValue<string> cultureName = null,
        TValue<bool> isForExport = null,
        TValue<StimulExportFormat> exportFormat = null,
        TValue<bool> isBarcodeTemplate = null
        )
    {
      if (isPublished != null && isPublished != reportVersion.IsPublished && isPublished == true)
      {
        #region Get Published ReportVersion
        var reportVersions = GetReportVersions(selector: e => e,
            reportId: reportId,
            isPublished: true);
        reportVersions = reportVersions.Where(i => i.Id != reportVersion.Id);
        #endregion
        #region Edit Published ReportVersion
        foreach (var item in reportVersions)
        {
          EditReportVersion(reportVersion: item,
                        rowVersion: item.RowVersion,
                        isPublished: false);
        }
        #endregion
      }
      return EditReportVersion(
                    reportVersion: reportVersion,
                    rowVersion: rowVersion,
                    apiUrl: apiUrl,
                    reportContent: reportContent,
                    isPublished: isPublished,
                    reportId: reportId,
                    cultureName: cultureName,
                    isForExport: isForExport,
                    exportFormat: exportFormat,
                    isBarcodeTemplate: isBarcodeTemplate);
    }
    #endregion
    #region GetByReportName
    public string GenerateReportContent(
    string apiUrl,
    string apiParams,
    KeyValueInput[] reportParams)
    {
      var reportContent = "";
      #region GetReportData
      var invokeResult = ApiHelper.InvokeApiAction(
          apiRelativeUrl: apiUrl,
          parameters: apiParams);
      if (invokeResult == null || invokeResult.Content == null)
        throw new ApiInvokeException(invokeResult);
      #endregion
      #region CreateReport and RegData
      var report = ReportHelper.GenerateReport(invokeResult.Content.Data.GetType(), parameters: reportParams);
      reportContent = report.SaveToString();
      #endregion
      return reportContent;
    }
    #endregion
  }
}