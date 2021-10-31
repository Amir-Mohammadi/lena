using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
//using System.Web.UI.WebControls;
using lena.Services.Common;
using lena.Services.Common.Helpers;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ReportManagement.Exception;
using lena.Services.Internals.Reports.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Reports;
using lena.Models.Reports.ReportPrintSettings;
using Stimulsoft.Report;
using Stimulsoft.Report.Print;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ReportManagement
{
  public partial class ReportManagement
  {
    #region Add
    public Report AddReport(
        string name,
        string description)
    {

      var report = repository.Create<Report>();
      report.Name = name;
      report.Description = description;
      repository.Add(report);
      return report;
    }
    #endregion
    #region AddProcess
    public Report AddReportProcess(
        string name,
        string description,
        string apiUrl,
        string reportContent,
        bool isPublished,
        string cultureName,
        bool isForExport,
        StimulExportFormat? exportFormat,
        bool isBarcodeTemplate)
    {

      #region Check Exist Report
      var reportIsExist = GetReports(selector: e => e,
              name: name)

          .Any();
      if (reportIsExist)
        throw new ReportIsExistException(name);
      #endregion
      #region AddReport
      var report = AddReport(
              name: name,
              description: description);
      #endregion
      #region AddReportVersion
      AddReportVersion(
              apiUrl: apiUrl,
              reportContent: reportContent,
              isPublished: isPublished,
              reportId: report.Id,
              cultureName: cultureName,
              isForExport: isForExport,
              exportFormat: exportFormat,
              isBarcodeTemplate: isBarcodeTemplate);
      #endregion
      return report;
    }
    #endregion
    #region Edit
    public Report EditReport(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<string> description = null)
    {

      var report = GetReport(id: id);
      return EditReport(
                    report: report,
                    rowVersion: rowVersion,
                    name: name,
                    description: description);
    }
    public Report EditReport(
    Report report,
    byte[] rowVersion,
            TValue<string> name = null,
            TValue<string> description = null)
    {

      if (name != null)
      {
        #region Check Exist Report
        var reportIsExist = GetReports(
                selector: e => e,
                name: name)


            .Any(i => i.Id != report.Id);
        if (reportIsExist)
          throw new ReportIsExistException(name);
        #endregion
      }
      if (name != null)
        report.Name = name;
      if (description != null)
        report.Description = description;
      repository.Update(rowVersion: rowVersion, entity: report);
      return report;
    }
    #endregion
    #region EditProcess
    public Report EditReportProcess(
        int reportId,
        byte[] reportRrowVersion,
        int reportVersionId,
        byte[] reportVersionRrowVersion,
        bool isNewVersion,
        string name,
        string description,
        string reportContent,
        string apiUrl,
        bool isPublished,
        string cultureName,
        bool isForExport,
        StimulExportFormat? exportFormat,
        bool isBarcodeTemplate)
    {

      #region EditReport
      var report = EditReport(id: reportId,
              rowVersion: reportRrowVersion,
              name: name,
              description: description);
      #endregion
      #region SaveReportVersion
      if (isNewVersion)
      {
        #region AddReportVersion
        AddReportVersionProcess(
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
      else
      {
        #region EditReportVersion
        EditReportVersionProcess(
                id: reportVersionId,
                rowVersion: reportVersionRrowVersion,
                apiUrl: apiUrl,
                reportContent: reportContent,
                isPublished: isPublished,
                reportId: report.Id,
                cultureName: cultureName,
                isForExport: isForExport,
                exportFormat: exportFormat,
                isBarcodeTemplate: isBarcodeTemplate);
        #endregion
      }
      #endregion
      return report;
    }
    #endregion
    #region GetReports
    public IQueryable<TResult> GetReports<TResult>(
        Expression<Func<Report, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null)
    {

      var query = repository.GetQuery<Report>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (description != null)
        query = query.Where(i => i.Description == description);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public TResult GetReport<TResult>(
        Expression<Func<Report, TResult>> selector,
        int id)
    {

      var record = GetReports(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (record == null)
        throw new ReportNotFoundException(id: id);
      return record;
    }
    public Report GetReport(int id) => GetReport(selector: e => e, id: id);
    #endregion
    #region GetByName
    public TResult GetReport<TResult>(
        Expression<Func<Report, TResult>> selector,
        string name)
    {

      var record = GetReports(
                    selector: selector,
                    name: name)


                .FirstOrDefault();
      if (record == null)
        throw new ReportNotFoundException(name: name);
      return record;
    }
    public Report GetReport(string name) => GetReport(selector: e => e, name: name);
    #endregion
    #region Delete
    public void DeleteReport(int id)
    {

      var report = GetReport(id: id);
      repository.Delete(report);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ReportResult> SortReportResult(
        IQueryable<ReportResult> query,
    SortInput<ReportSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReportSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ReportSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ReportResult> SearchReportResult(
        IQueryable<ReportResult> query,
    string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.Name.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<Report, ReportResult>> ToReportResult =
        report => new ReportResult
        {
          Id = report.Id,
          Name = report.Name,
          Description = report.Description,
          RowVersion = report.RowVersion
        };
    #endregion
    #region Get StiReport
    public StiReport GetStiReport(
        string reportName,
        object apiParams,
        string apiUrl,
        KeyValueInput[] reportParams,
        bool isForExport,
        StimulExportFormat? exportFormat)
    {

      #region GetReportVersionByReportName
      var reportVersion = GetPublishedReportVersionByReportName(
          reportName: reportName,
          isForExport: isForExport,
          exportFormat: exportFormat,
          throwExceptionIfNotExist: false);
      #endregion
      #region GetReportData
      var invokeResult = ApiHelper.InvokeApiAction(
          apiRelativeUrl: apiUrl ?? reportVersion.ApiUrl,
          parameters: apiParams);
      if (invokeResult == null || invokeResult.Content == null)
        throw new ApiInvokeException(invokeResult);
      #endregion
      #region CreateReport and RegData
      StiReport report = null;
      if (reportVersion == null)
        report = ReportHelper.GenerateReport(invokeResult.Content.Data.GetType(), parameters: reportParams);
      else
      {
        report = ReportHelper.GenerateBaseReport(reportVersion?.CultureName ?? null);
        report.LoadFromJson(reportVersion.ReportContent);
      }
      foreach (var comp in report.GetComponents())
      {
        var cmp = comp as Stimulsoft.Report.Components.StiText;
        if (cmp != null)
        {
          var fontName = cmp.Font.Name.ToLower();
          if (fontName == "tahoma" || fontName == "arial" || fontName == "b nazanin")
            cmp.Font = new System.Drawing.Font("IRANSans", cmp.Font.Size, cmp.Font.Style);
        }
      }
      report.CalculationMode = StiCalculationMode.Compilation;
      for (int i = 0; i < report.Pages.Count; i++)
      {
        report.Pages[i].UnlimitedBreakable = false;
        report.Pages[i].UnlimitedHeight = isForExport;
      }
      ReportHelper.RegisterData(report: report, data: invokeResult.Content.Data, parameters: reportParams);
      report.Compile();
      report.Render(false);
      #endregion
      return report;
    }
    public StiReport GetStiReport(
  string reportName,
  object reportData,
  KeyValueInput[] reportParams)
    {

      #region GetReportVersionByReportName
      var reportVersion = GetPublishedReportVersionByReportName(
          reportName: reportName,
          throwExceptionIfNotExist: false);
      #endregion
      #region CreateReport and RegData
      StiReport report = null;
      if (reportVersion == null)
        report = ReportHelper.GenerateReport(reportData.GetType(), parameters: reportParams);
      else
      {
        report = ReportHelper.GenerateBaseReport(reportVersion?.CultureName ?? null);
        report.LoadFromJson(reportVersion.ReportContent);
      }
      report.CalculationMode = StiCalculationMode.Compilation;
      ReportHelper.RegisterData(report: report, data: reportData, parameters: reportParams);
      report.Compile();
      report.Render(false);
      #endregion
      return report;
    }
    #endregion
    #region Get ReportRender
    public ReportRenderResult GetReportRender(
        string reportName,
        object apiParams,
        string apiUrl,
        KeyValueInput[] reportParams)
    {

      var result = new ReportRenderResult();
      #region GetReportVersionByReportName
      var reportVersion = GetPublishedReportVersionByReportName(
          reportName: reportName,
          throwExceptionIfNotExist: false);
      #endregion
      if (reportVersion != null)
      {
        #region GetReportSetting
        var reportSetting = GetReportPrintSettingByReportName(
                selector: ToReportPrintSettingResult,
                reportName: reportName,
                throwExceptionIfNotExist: false);
        #endregion
        #region GetRenderType
        result.RenderType = ReportRenderType.Preview;
        if (reportSetting != null && reportSetting.ShowPreview == false && reportSetting.ShowPrintDialog == false)
          result.RenderType = ReportRenderType.DirectPrint;
        #endregion
        #region ReturnIfIsDirectPrint
        if (result.RenderType == ReportRenderType.DirectPrint)
          return result;
        #endregion
      }
      #region GetStiReport
      var report = GetStiReport(
              reportName: reportName,
              apiParams: apiParams,
              apiUrl: apiUrl,
              isForExport: false,
              exportFormat: null,
              reportParams: reportParams);
      #endregion
      result.ReportRenderContent = report.SaveDocumentJsonToString();
      return result;
    }
    #endregion
    #region GetReportExportFile
    public byte[] GetReportExportFile(
    string reportName,
    object apiParams,
    string apiUrl,
    StimulExportFormat exportFormat,
    KeyValueInput[] reportParams)
    {

      #region GetStiReport
      var stiReport = GetStiReport(reportName: reportName,
              apiParams: apiParams,
              apiUrl: apiUrl,
              reportParams: reportParams,
              isForExport: true,
              exportFormat: exportFormat);
      #endregion
      #region Change ReportForExport
      StiOptions.Export.Excel.ColumnsRightToLeft = true;
      stiReport.Compile();
      stiReport.Render(false);
      #endregion
      #region GetExportFormat
      StiExportFormat stiExportFormat;
      if (!Enum.TryParse(exportFormat.ToString(), out stiExportFormat))
        throw new System.Exception("Report export format is not valid");
      #endregion
      #region GetExportStream
      var stream = new MemoryStream();
      stiReport.ExportDocument(stiExportFormat, stream);
      #endregion
      return stream.ToArray();
    }
    #endregion
    #region Print
    public bool Print(
    string reportName,
    string apiUrl,
    object apiParams,
    int printerId,
    int numberOfCopies,
    KeyValueInput[] reportParams)
    {

      #region GetPrinter
      var printer = App.Internals.PrinterManagment.GetPrinter(printerId);
      var printerName = PrinterHelper.CheckPrinterConfiguration(printer.NameInSystem, printer.NetworkAddress);
      if (string.IsNullOrEmpty(printerName))
        throw new PrinterNotConfiguredInServerException(printerName: printer.NameInSystem);
      #endregion
      #region GetStiReport
      var report = GetStiReport(
              reportName: reportName,
              apiParams: apiParams,
              apiUrl: apiUrl,
              isForExport: false,
              exportFormat: null,
              reportParams: reportParams);
      #endregion
      #region Set PrinterSetting and Print
      Stimulsoft.Report.StiOptions.Engine.BarcodeDpiMultiplierFactor = 6;
      var setting = new PrinterSettings
      {
        PrinterName = printerName,
        Copies = (short)numberOfCopies
      };
      report.Print(false, setting);
      #endregion
      return true;
    }
    public bool Print(
        string reportName,
        object reportData,
        int printerId,
        int numberOfCopies,
        KeyValueInput[] reportParams)
    {

      #region GetPrinter
      var printer = App.Internals.PrinterManagment.GetPrinter(printerId);
      var printerName = PrinterHelper.CheckPrinterConfiguration(printer.NameInSystem, printer.NetworkAddress);
      if (string.IsNullOrEmpty(printerName))
        throw new PrinterNotConfiguredInServerException(printerName: printer.NameInSystem);
      #endregion
      #region GetStiReport
      var report = GetStiReport(
              reportName: reportName,
              reportData: reportData,
              reportParams: reportParams);
      #endregion
      #region Set PrinterSetting and Print
      StiOptions.Engine.BarcodeDpiMultiplierFactor = 6;
      var setting = new PrinterSettings()
      {
        PrinterName = printerName,
        Copies = (byte)numberOfCopies
      };
      report.Print(false, setting);
      #endregion
      return true;
    }
    #endregion
  }
}