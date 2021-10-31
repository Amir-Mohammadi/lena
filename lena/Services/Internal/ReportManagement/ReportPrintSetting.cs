using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
using lena.Services.Internals.ReportManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Reports;
using lena.Models.Reports.ReportPrintSettings;
using System.Threading.Tasks;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ReportManagement
{
  public partial class ReportManagement
  {
    #region Get
    public ReportPrintSetting GetReportPrintSetting(int id) =>
        GetReportPrintSetting(selector: e => e, id: id);
    public TResult GetReportPrintSetting<TResult>(
        Expression<Func<ReportPrintSetting, TResult>> selector,
        int id)
    {
      var reportPrintSetting = GetReportPrintSettings(
                   selector: selector,
                   id: id)

               .FirstOrDefault();
      if (reportPrintSetting == null)
        throw new ReportPrintSettingNotFoundException(id);
      return reportPrintSetting;
    }
    #endregion
    #region Get By ReportId
    public ReportPrintSetting GetReportPrintSettingByReportId(
        int reportId,
        bool throwExceptionIfNotExist = true) =>
        GetReportPrintSettingByReportId(
            selector: e => e,
            reportId: reportId,
            throwExceptionIfNotExist: throwExceptionIfNotExist);
    public TResult GetReportPrintSettingByReportId<TResult>(
        Expression<Func<ReportPrintSetting, TResult>> selector,
        int reportId,
        bool throwExceptionIfNotExist = true)
    {
      var currentUserId = App.Providers.Security.CurrentLoginData.UserId;
      var reportPrintSetting = GetReportPrintSettings(
                    selector: selector,
                    reportId: reportId,
                    userId: currentUserId)

                .FirstOrDefault();
      if (reportPrintSetting == null && throwExceptionIfNotExist)
        throw new ReportPrintSettingNotFoundException(reportId: reportId, userId: currentUserId);
      return reportPrintSetting;
    }
    #endregion
    #region Get By ReportName
    public ReportPrintSetting GetReportPrintSettingByReportName(
        string reportName,
        bool throwExceptionIfNotExist = true) =>
        GetReportPrintSettingByReportName(
            selector: e => e,
            reportName: reportName,
            throwExceptionIfNotExist: throwExceptionIfNotExist);
    public TResult GetReportPrintSettingByReportName<TResult>(
        Expression<Func<ReportPrintSetting, TResult>> selector,
        string reportName,
        bool throwExceptionIfNotExist = true)
    {
      var currentUserId = App.Providers.Security.CurrentLoginData.UserId;
      var reportPrintSetting = GetReportPrintSettings(
                    selector: selector,
                    reportName: reportName,
                    userId: currentUserId)

                .FirstOrDefault();
      if (reportPrintSetting == null && throwExceptionIfNotExist)
        throw new ReportPrintSettingNotFoundException(reportName: reportName, userId: currentUserId);
      return reportPrintSetting;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetReportPrintSettings<TResult>(
            Expression<Func<ReportPrintSetting, TResult>> selector,
            TValue<int> id = null,
            TValue<int> reportId = null,
            TValue<int> printerId = null,
            TValue<DateTime> creationTime = null,
            TValue<int> userId = null,
            TValue<bool> showPreview = null,
            TValue<bool> showPrintDialog = null,
            TValue<int> numberOfCopies = null,
            TValue<string> reportName = null)
    {
      var query = repository.GetQuery<ReportPrintSetting>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (reportId != null)
        query = query.Where(x => x.ReportId == reportId);
      if (printerId != null)
        query = query.Where(x => x.PrinterId == printerId);
      if (creationTime != null)
        query = query.Where(x => x.CreationTime == creationTime);
      if (userId != null)
        query = query.Where(x => x.UserId == userId);
      if (showPreview != null)
        query = query.Where(x => x.ShowPreview == showPreview);
      if (showPrintDialog != null)
        query = query.Where(x => x.ShowPrintDialog == showPrintDialog);
      if (numberOfCopies != null)
        query = query.Where(x => x.NumberOfCopies == numberOfCopies);
      if (reportName != null)
        query = query.Where(x => x.Report.Name == reportName);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ReportPrintSetting AddReportPrintSetting(
            int reportId,
            int printerId,
            bool showPreview,
            bool showPrintDialog,
            int numberOfCopies)
    {
      var reportPrintSetting = repository.Create<ReportPrintSetting>();
      reportPrintSetting.ReportId = reportId;
      reportPrintSetting.PrinterId = printerId;
      reportPrintSetting.CreationTime = DateTime.Now.ToUniversalTime();
      reportPrintSetting.UserId = App.Providers.Security.CurrentLoginData.UserId;
      reportPrintSetting.ShowPreview = showPreview;
      reportPrintSetting.ShowPrintDialog = showPrintDialog;
      reportPrintSetting.NumberOfCopies = numberOfCopies;
      repository.Add(reportPrintSetting);
      return reportPrintSetting;
    }
    #endregion
    #region Edit
    public ReportPrintSetting EditReportPrintSetting(
        int id,
        byte[] rowVersion,
        TValue<int> reportId = null,
        TValue<int> printerId = null,
        TValue<bool> showPreview = null,
        TValue<bool> showPrintDialog = null,
        TValue<int> numberOfCopies = null)
    {
      var reportPrintSetting = GetReportPrintSetting(id: id);
      return EditReportPrintSetting(
                    reportPrintSetting: reportPrintSetting,
                    rowVersion: rowVersion,
                    reportId: reportId,
                    printerId: printerId,
                    showPreview: showPreview,
                    showPrintDialog: showPrintDialog,
                    numberOfCopies: numberOfCopies);
    }
    public ReportPrintSetting EditReportPrintSetting(
        ReportPrintSetting reportPrintSetting,
        byte[] rowVersion,
        TValue<int> reportId = null,
        TValue<int> printerId = null,
        TValue<bool> showPreview = null,
        TValue<bool> showPrintDialog = null,
        TValue<int> numberOfCopies = null)
    {
      if (reportId != null)
        reportPrintSetting.ReportId = reportId;
      if (printerId != null)
        reportPrintSetting.PrinterId = printerId;
      if (showPreview != null)
        reportPrintSetting.ShowPreview = showPreview;
      if (showPrintDialog != null)
        reportPrintSetting.ShowPrintDialog = showPrintDialog;
      if (numberOfCopies != null)
        reportPrintSetting.NumberOfCopies = numberOfCopies;
      repository.Update(rowVersion: rowVersion, entity: reportPrintSetting);
      return reportPrintSetting;
    }
    #endregion
    #region Delete
    public void DeleteReportPrintSetting(int id)
    {
      var reportPrintSetting = GetReportPrintSetting(id: id);
      repository.Delete(reportPrintSetting);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ReportPrintSettingResult> SortReportPrintSettingResult(
        IQueryable<ReportPrintSettingResult> query,
        SortInput<ReportPrintSettingSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReportPrintSettingSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ReportPrintSettingSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case ReportPrintSettingSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case ReportPrintSettingSortType.ReportVersion:
          return query.OrderBy(a => a.ReportVersion, sort.SortOrder);
        case ReportPrintSettingSortType.ApiUrl:
          return query.OrderBy(a => a.ApiUrl, sort.SortOrder);
        case ReportPrintSettingSortType.PrinterName:
          return query.OrderBy(a => a.PrinterName, sort.SortOrder);
        case ReportPrintSettingSortType.ReportName:
          return query.OrderBy(a => a.ReportName, sort.SortOrder);
        case ReportPrintSettingSortType.ShowPreview:
          return query.OrderBy(a => a.ShowPreview, sort.SortOrder);
        case ReportPrintSettingSortType.ShowPrintDialog:
          return query.OrderBy(a => a.ShowPrintDialog, sort.SortOrder);
        case ReportPrintSettingSortType.NumberOfCopies:
          return query.OrderBy(a => a.NumberOfCopies, sort.SortOrder);
        case ReportPrintSettingSortType.CreationTime:
          return query.OrderBy(a => a.CreationTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ReportPrintSettingResult> SearchReportPrintSettingResult(
        IQueryable<ReportPrintSettingResult> query,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.ApiUrl.Contains(searchText) ||
                    item.EmployeeFullName.Contains(searchText) ||
                    item.UserName.Contains(searchText) ||
                    item.PrinterName.Contains(searchText) ||
                    item.ReportName.Contains(searchText)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ReportPrintSetting, ReportPrintSettingResult>> ToReportPrintSettingResult =
                 reportPrintSetting => new ReportPrintSettingResult
                 {
                   Id = reportPrintSetting.Id,
                   PrinterId = reportPrintSetting.PrinterId,
                   PrinterName = reportPrintSetting.Printer.Manufacture + "-" +
                                       reportPrintSetting.Printer.Model + "-" +
                                       reportPrintSetting.Printer.PrinterType,
                   ReportId = reportPrintSetting.Report.Id,
                   ReportName = reportPrintSetting.Report.Name,
                   NumberOfCopies = reportPrintSetting.NumberOfCopies,
                   ShowPreview = reportPrintSetting.ShowPreview,
                   ShowPrintDialog = reportPrintSetting.ShowPrintDialog,
                   UserId = reportPrintSetting.UserId,
                   EmployeeFullName = reportPrintSetting.User.Employee.FirstName + " " + reportPrintSetting.User.Employee.LastName,
                   UserName = reportPrintSetting.User.UserName,
                   CreationTime = reportPrintSetting.Printer.CreationTime,
                   RowVersion = reportPrintSetting.RowVersion
                 };
    #endregion
    #region Add
    public ReportPrintSetting SaveReportPrintSettingProcess(
        string reportName,
        int printerId,
        bool showPreview,
        bool showPrintDialog,
        int numberOfCopies)
    {
      #region GetReport
      var report = GetReport(name: reportName);
      #endregion
      #region GetReportSetting
      var reportPrintSetting = GetReportPrintSettingByReportId(
              reportId: report.Id,
              throwExceptionIfNotExist: false);
      #endregion
      if (reportPrintSetting == null)
      {
        reportPrintSetting = AddReportPrintSetting(
                      reportId: report.Id,
                      printerId: printerId,
                      showPreview: showPreview,
                      showPrintDialog: showPrintDialog,
                      numberOfCopies: numberOfCopies
                      );
      }
      else
      {
        reportPrintSetting = EditReportPrintSetting(
                      reportPrintSetting: reportPrintSetting,
                      rowVersion: reportPrintSetting.RowVersion,
                      reportId: report.Id,
                      printerId: printerId,
                      showPreview: showPreview,
                      showPrintDialog: showPrintDialog,
                      numberOfCopies: numberOfCopies
                  );
      }
      return reportPrintSetting;
    }
    #endregion
  }
  //public ReportPrintSetting GetReportPrintSetting(int id)
  //{
  //    
  //        var record = repository.GetQuery<ReportPrintSetting>().FirstOrDefault(x => x.Id == id);
  //        if (record == null)
  //            throw new RecordNotFoundException(id, typeof(ReportPrintSetting));
  //        return record;
  //    });
  //}
  //public ReportPrintSetting AddReportPrintSetting(AddReportPrintSettingInput input)
  //{
  //    
  //        var setting = repository.Create<ReportPrintSetting>();
  //        setting.PrinterId = input.PrinterId;
  //        setting.ReportId = input.ReportId;
  //        setting.NumberOfCopies = input.NumberOfCopies;
  //        setting.ShowPreview = input.ShowPreview;
  //        setting.ShowPrintDialog = input.ShowPrintDialog;
  //        setting.UserId = App.Providers.Security.CurrentLoginData.UserId;
  //        setting.CreationTime = DateTime.Now.ToUniversalTime();
  //        repository.Add(setting);
  //        return setting;
  //    });
  //}
  //public ReportPrintSetting EditReportPrintSetting(EditReportPrintSettingInput input)
  //{
  //    
  //        var reportPrintSetting = GetReportPrintSetting(id: input.Id)
  //            
  //;
  //        reportPrintSetting.PrinterId = input.PrinterId;
  //        reportPrintSetting.ReportId = input.ReportId;
  //        reportPrintSetting.NumberOfCopies = input.NumberOfCopies;
  //        reportPrintSetting.ShowPreview = input.ShowPreview;
  //        reportPrintSetting.ShowPrintDialog = input.ShowPrintDialog;
  //        reportPrintSetting.UserId = App.Providers.Security.CurrentLoginData.UserId;
  //        reportPrintSetting.CreationTime = DateTime.Now.ToUniversalTime();
  //        repository.Update<ReportPrintSetting>(reportPrintSetting, input.RowVersion);
  //        return reportPrintSetting;
  //    });
  //}
  //public ReportPrintSetting DeleteReportPrintSetting(int id)
  //{
  //    
  //        var reportPrintSetting = GetReportPrintSetting(id: id)
  //            
  //;
  //        repository.Delete(reportPrintSetting);
  //        return reportPrintSetting;
  //    });
  //}
  //public IQueryable<ReportPrintSetting> GetReportsPrintSettings()
  //{
  //    
  //        return repository.GetQuery<ReportPrintSetting>();
  //    });
  //}
  //public IQueryable<ReportPrintSettingResult> GetReportPrintSettingResults()
  //{
  //    
  //        var query = (from setting in repository.GetQuery<ReportPrintSetting>()
  //                         //join user in repository.GetQuery<User>() on setting.UserId equals user.Id
  //                     let user = setting.User
  //                     let report = setting.Report
  //                     let printer = setting.Printer
  //                     select new ReportPrintSettingResult()
  //                     {
  //                         Id = setting.Id,
  //                         PrinterId = setting.PrinterId,
  //                         PrinterName = printer.Manufacture + "-" + printer.Model + "-" + printer.PrinterType,
  //                         ReportId = report.Id,
  //                         ReportName = report.Name,
  //                         NumberOfCopies = setting.NumberOfCopies,
  //                         ShowPreview = setting.ShowPreview,
  //                         ShowPrintDialog = setting.ShowPrintDialog,
  //                         UserId = setting.UserId,
  //                         UserName = user.Employee.FirstName + " " + user.Employee.LastName,
  //                         CreationTime = printer.CreationTime,
  //                         //CreatorUserName = user.Employee.FirstName + " " + user.Employee.LastName
  //                     });
  //        return query;
  //    });
  //}
  //public IQueryable<ReportPrintSettingResult> SearchReportPrintSettingResultQuery(
  //    IQueryable<ReportPrintSettingResult> query,
  //    AdvanceSearchItem[] advanceSearchItems,
  //    string searchText)
  //{
  //    if (!string.IsNullOrWhiteSpace(searchText))
  //        query = from printer in query
  //                where printer.CreatorUserName.Contains(searchText)
  //                select printer;
  //    //if (advanceSearchItems.Any())
  //    //    query = query.Where(advanceSearchItems);
  //    return query;
  //}
  //}
}