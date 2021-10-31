using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.TicketSoftware;
using System.Runtime.Serialization;
using System.Collections.Generic;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region  AddTicketSoftwareProcess
    public void AddTicketSoftwareProcess(
         string subject,
         string content,
         string issueLink,
         string[] fileKeies,
         TicketSoftwarePriority priority,
         List<UploadFileData> uploadFileDatas)
    {

      List<Document> documents = new List<Document>();
      foreach (var uploadFileData in uploadFileDatas)
      {
        Document document = null;
        if (uploadFileData != null)
          document = App.Internals.ApplicationBase.AddDocument(
                   name: uploadFileData.FileName,
                   fileStream: uploadFileData.FileData);
        documents.Add(document);
      }
      var ticketSoftware = AddTicketSoftware(
               subject: subject,
               content: content,
               priority: priority,
               issueLink: issueLink);

      foreach (var doc in documents)
      {
        AddTicketFile
              (
                  documentId: doc.Id,
                  ticketSoftWareId: ticketSoftware.Id
                      );
      }
    }

    #endregion

    #region

    public TicketSoftware AddTicketSoftware(
          string subject,
          string content,
          TicketSoftwarePriority priority,
          string issueLink)
    {

      var ticketSoftware = repository.Create<TicketSoftware>();
      ticketSoftware.Subject = subject;
      ticketSoftware.Content = content;
      ticketSoftware.UserId = App.Providers.Security.CurrentLoginData.UserId;
      ticketSoftware.CreateDateTime = DateTime.Now.ToUniversalTime();
      ticketSoftware.Priority = priority;
      ticketSoftware.IssueLink = issueLink;

      repository.Add(ticketSoftware);
      return ticketSoftware;
    }
    #endregion

    #region Get
    public TicketSoftware GetTicketSoftware(int id) => GetTicketSoftware(selector: e => e, id: id);
    public TResult GetTicketSoftware<TResult>(
        Expression<Func<TicketSoftware, TResult>> selector,
        int id)
    {

      var ticketSoftware = GetTicketSoftwares(selector: selector,
                id: id).FirstOrDefault();
      return ticketSoftware;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetTicketSoftwares<TResult>(
           Expression<Func<TicketSoftware, TResult>> selector,
           TValue<int?> id = null,
           TValue<int> userId = null,
           TValue<string> subject = null,
           TValue<string> content = null,
           TValue<DateTime> fromCreateDateTime = null,
           TValue<DateTime?> toCreateDateTime = null,
           TValue<TicketSoftwareStatus> status = null,
           TValue<string> issueLink = null,
           TValue<TicketSoftwarePriority> priority = null,
           TValue<int?> fileId = null,
           TValue<int> lastedEditorUserId = null,
           TValue<byte[]> rowVersion = null
           )
    {


      var TicketSoftware = repository.GetQuery<TicketSoftware>();

      if (id != null)
        TicketSoftware = TicketSoftware.Where(i => i.Id == id);

      if (userId != null)
        TicketSoftware = TicketSoftware.Where(r => r.UserId == userId);

      if (subject != null)
        TicketSoftware = TicketSoftware.Where(r => r.Subject == subject);

      if (content != null)
        TicketSoftware = TicketSoftware.Where(r => r.Content == content);

      if (fromCreateDateTime != null)
        TicketSoftware = TicketSoftware.Where(i => i.CreateDateTime >= fromCreateDateTime);

      if (toCreateDateTime != null)
        TicketSoftware = TicketSoftware.Where(i => i.UpdateDateTime >= toCreateDateTime);

      if (status != null)
        TicketSoftware = TicketSoftware.Where(i => i.Status == status);

      if (issueLink != null)
        TicketSoftware = TicketSoftware.Where(i => i.IssueLink == issueLink);

      if (priority != null)
        TicketSoftware = TicketSoftware.Where(i => i.Priority == priority);

      if (lastedEditorUserId != null)
        TicketSoftware = TicketSoftware.Where(i => i.LastedEditorUserId == lastedEditorUserId);

      return TicketSoftware.Select(selector);
    }
    #endregion

    #region EditTicketSoftware
    public TicketSoftware EditTicketSoftware(
        byte[] rowVersion,
        int id,
        TValue<string> subject = null,
        TValue<string> content = null,
        TValue<string> issueLink = null,
        TValue<TicketSoftwarePriority> priority = null,
        TValue<string> fileKeies = null,
        TValue<int> fileId = null)
    {

      var ticketSoftware = GetTicketSoftware(id: id);

      if (subject != null)
        ticketSoftware.Subject = subject;

      if (content != null)
        ticketSoftware.Content = content;

      if (issueLink != null)
        ticketSoftware.IssueLink = issueLink;

      if (priority != null)
        ticketSoftware.Priority = priority;

      ticketSoftware.UpdateDateTime = DateTime.Now.ToUniversalTime();
      ticketSoftware.LastedEditorUserId = App.Providers.Security.CurrentLoginData.UserId;

      return ticketSoftware;
    }
    #endregion

    #region DeleteTicketSoftware
    public void DeleteTicketSoftware(int id)
    {

      var TicketSoftware = GetTicketSoftware(id: id);
      if (TicketSoftware.Status != TicketSoftwareStatus.Open)
        throw new CanNotDeleteTicketSoftwareException(id);
      repository.Delete(TicketSoftware);
    }
    #endregion

    #region ToResult
    public Expression<Func<TicketSoftware, TicketSoftwareResult>> ToTicketSoftwareResult =
        TicketSoftware => new TicketSoftwareResult
        {
          Id = TicketSoftware.Id,
          UserId = TicketSoftware.UserId,
          UserFullName = TicketSoftware.User.Employee.FirstName + " " + TicketSoftware.User.Employee.LastName,
          Subject = TicketSoftware.Subject,
          Content = TicketSoftware.Content,
          CreateDateTime = TicketSoftware.CreateDateTime,
          UpdateDateTime = TicketSoftware.UpdateDateTime,
          Status = TicketSoftware.Status,
          IssueLink = TicketSoftware.IssueLink,
          Priority = TicketSoftware.Priority,
          LastedEditorUserId = TicketSoftware.LastedEditorUserId,
          UserFullNameEditor = TicketSoftware.LastedEditorUser.Employee.FirstName + " " + TicketSoftware.LastedEditorUser.Employee.LastName,
          TicketComments = TicketSoftware.TicketComments.AsQueryable().Select(App.Internals.Planning.ToTicketCommentResult),
          RowVersion = TicketSoftware.RowVersion
        };
    #endregion

    #region Search
    public IQueryable<TicketSoftwareResult> SearchTicketSoftwareResult(
        IQueryable<TicketSoftwareResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText = null)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.Status.ToString().Contains(searchText) ||
            item.Priority.ToString().Contains(searchText) ||
            item.Content.ToString().Contains(searchText) ||
            item.CreateDateTime.ToString().Contains(searchText) ||
            item.Subject.Contains(searchText));

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<TicketSoftwareResult> SortTicketSoftwareResult(
           IQueryable<TicketSoftwareResult> query,
        SortInput<TicketSoftwareSortType> sort)
    {
      switch (sort.SortType)
      {
        case TicketSoftwareSortType.UserId:
          return query.OrderBy(a => a.UserId, sort.SortOrder);
        case TicketSoftwareSortType.CreateDateTime:
          return query.OrderBy(a => a.CreateDateTime, sort.SortOrder);
        case TicketSoftwareSortType.UpdateDateTime:
          return query.OrderBy(a => a.UpdateDateTime, sort.SortOrder);
        case TicketSoftwareSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case TicketSoftwareSortType.Priority:
          return query.OrderBy(a => a.Priority, sort.SortOrder);
        case TicketSoftwareSortType.Content:
          return query.OrderBy(a => a.Content, sort.SortOrder);
        case TicketSoftwareSortType.Subject:
          return query.OrderBy(a => a.Subject, sort.SortOrder);
        case TicketSoftwareSortType.IssueLink:
          return query.OrderBy(a => a.IssueLink, sort.SortOrder);
        case TicketSoftwareSortType.LastedEditorUserId:
          return query.OrderBy(a => a.LastedEditorUserId, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
