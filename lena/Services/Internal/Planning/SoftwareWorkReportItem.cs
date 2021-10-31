//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Planning.SoftwareWorkReportItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Domains.Enums;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Add
    public SoftwareWorkReportItem AddSoftwareWorkReportItem(
        int softwareWorkReportId,
        string issue,
        int spent,
        RestTimeStatus? restTimeIssue,
        int estimated)
    {

      var softwareWorkReportItem = repository.Create<SoftwareWorkReportItem>();
      softwareWorkReportItem.SoftwareWorkReportId = softwareWorkReportId;
      softwareWorkReportItem.Spent = spent;
      softwareWorkReportItem.Estimated = estimated;
      softwareWorkReportItem.Issue = issue;
      softwareWorkReportItem.RestTimeIssue = restTimeIssue;
      repository.Add(softwareWorkReportItem);
      return softwareWorkReportItem;
    }
    #endregion


    #region Edit
    public SoftwareWorkReportItem EditSoftwareWorkReportItem(
        int id,
        byte[] rowVersion,
        TValue<int> softwareWorkReportId = null,
        TValue<int> spent = null,
        TValue<int> estimated = null,
        TValue<string> issue = null)
    {

      var softwareWorkReportItem = GetSoftwareWorkReportItem(id: id);

      EditSoftwareWorkReportItem(
                softwareWorkReportItem: softwareWorkReportItem,
                rowVersion: rowVersion,
                softwareWorkReportId: softwareWorkReportId,
                spent: spent,
                estimated: estimated,
                issue: issue);
      return softwareWorkReportItem;
    }



    public SoftwareWorkReportItem EditSoftwareWorkReportItem(
        SoftwareWorkReportItem softwareWorkReportItem,
        byte[] rowVersion,
        TValue<int> softwareWorkReportId = null,
        TValue<int> spent = null,
        TValue<int> estimated = null,
        TValue<string> issue = null)
    {


      if (softwareWorkReportId != null)
        softwareWorkReportItem.SoftwareWorkReportId = softwareWorkReportId;
      if (spent != null)
        softwareWorkReportItem.Spent = spent;
      if (estimated != null)
        softwareWorkReportItem.Estimated = estimated;
      if (issue != null)
        softwareWorkReportItem.Issue = issue;


      repository.Update(softwareWorkReportItem, rowVersion);

      return softwareWorkReportItem;
    }
    #endregion

    #region Delete
    public void DeleteSoftwareWorkReportItemProcess(int id)
    {

      var softwareWorkReportItem = GetSoftwareWorkReportItem(id: id);
      var softwareWorkReportId = softwareWorkReportItem.SoftwareWorkReportId;

      DeleteSoftwareWorkReportItem(softwareWorkReportItem: softwareWorkReportItem);

      var softwareWorkReport = GetSoftwareWorkReport(id: softwareWorkReportId);

      if (!softwareWorkReport.SoftwareWorkReportItems.Any())
      {
        DeleteSoftwareWorkReport(softwareWorkReport: softwareWorkReport);
      }

    }

    public void DeleteSoftwareWorkReportItem(int id)
    {

      var softwareWorkReportItem = GetSoftwareWorkReportItem(id: id);
      DeleteSoftwareWorkReportItem(softwareWorkReportItem: softwareWorkReportItem);


    }

    public void DeleteSoftwareWorkReportItem(SoftwareWorkReportItem softwareWorkReportItem)
    {

      repository.Delete(softwareWorkReportItem);
    }
    #endregion

    #region Get
    public SoftwareWorkReportItem GetSoftwareWorkReportItem(int id) => GetSoftwareWorkReportItem(selector: e => e, id: id);
    public TResult GetSoftwareWorkReportItem<TResult>(
        Expression<Func<SoftwareWorkReportItem, TResult>> selector,
        int id)
    {

      var softwareWorkReportItem = GetSoftwareWorkReportItems(
                selector: selector,
                id: id).FirstOrDefault();
      if (softwareWorkReportItem == null)
        throw new SoftwareWorkReportItemNotFoundException(id);
      return softwareWorkReportItem;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetSoftwareWorkReportItems<TResult>(
        Expression<Func<SoftwareWorkReportItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int> softwareWorkReportId = null
        )
    {


      var softwareWorkReportItems = repository.GetQuery<SoftwareWorkReportItem>();



      if (id != null)
        softwareWorkReportItems = softwareWorkReportItems.Where(x => x.Id == id);

      if (softwareWorkReportId != null)
        softwareWorkReportItems = softwareWorkReportItems.Where(i => i.SoftwareWorkReportId == softwareWorkReportId);



      return softwareWorkReportItems.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<SoftwareWorkReportItem, SoftwareWorkReportItemResult>> ToSoftwareWorkReportItemResult =
        softwareWorkReportItem => new SoftwareWorkReportItemResult
        {
          Id = softwareWorkReportItem.Id,
          Spent = softwareWorkReportItem.Spent,
          Estimated = softwareWorkReportItem.Estimated,
          Issue = softwareWorkReportItem.Issue,
          RowVersion = softwareWorkReportItem.RowVersion,
          RestTimeIssue = softwareWorkReportItem.RestTimeIssue,
        };
    #endregion
  }
}
