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
using lena.Models.Planning.TicketComment;
using System.Runtime.Serialization;
using System.Collections.Generic;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region  AddTicketCommentProcess
    public void AddTicketCommentProcess(

      string contentResponse,
      int ticketSoftwareId,
      string issueLink
     )
    {

      var TicketComment = AddTicketComment(

              content: contentResponse,
              ticketSoftwareId: ticketSoftwareId,
              issueLink: issueLink
            );
    }
    #endregion

    #region
    public TicketComment AddTicketComment(
        string content,
        int ticketSoftwareId,
        string issueLink
       )
    {

      var ticketComment = repository.Create<TicketComment>();
      ticketComment.Content = content;
      ticketComment.UserId = App.Providers.Security.CurrentLoginData.UserId;
      ticketComment.CreateDateTime = DateTime.Now.ToUniversalTime();
      ticketComment.TicketSoftwareId = ticketSoftwareId;
      repository.Add(ticketComment);
      return ticketComment;
    }
    #endregion

    #region Get
    public TicketComment GetTicketComment(int id) => GetTicketComment(selector: e => e, id: id);
    public TResult GetTicketComment<TResult>(
        Expression<Func<TicketComment, TResult>> selector,
        int id)
    {

      var ticketComment = GetTicketComments(selector: selector,
                id: id).FirstOrDefault();
      return ticketComment;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetTicketComments<TResult>(
           Expression<Func<TicketComment, TResult>> selector,
           TValue<int?> id = null,
           TValue<int> userId = null,
           TValue<int> ticketSoftwareId = null,
           TValue<string> contentResponse = null,
           TValue<DateTime> fromCreateDateTime = null,
           TValue<DateTime> toCreateDateTime = null,
           TValue<byte[]> rowVersion = null
     )
    {

      var TicketComment = repository.GetQuery<TicketComment>();

      if (id != null)
        TicketComment = TicketComment.Where(i => i.Id == id);

      if (userId != null)
        TicketComment = TicketComment.Where(r => r.UserId == userId);

      if (contentResponse != null)
        TicketComment = TicketComment.Where(r => r.Content == contentResponse);

      if (ticketSoftwareId != null)
        TicketComment = TicketComment.Where(i => i.TicketSoftwareId == ticketSoftwareId);

      return TicketComment.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<TicketComment, TicketCommentResult>> ToTicketCommentResult =
        TicketComment => new TicketCommentResult
        {
          Id = TicketComment.Id,
          UserId = TicketComment.UserId,
          TicketSoftwareId = TicketComment.TicketSoftwareId,
          ContentResponse = TicketComment.Content,
          CreateDateTime = TicketComment.CreateDateTime,
          SupporterFullName = TicketComment.User.Employee.FirstName + " " + TicketComment.User.Employee.LastName,
          RowVersion = TicketComment.RowVersion
        };
    #endregion

    #region Search
    public IQueryable<TicketCommentResult> SearchTicketCommentResult(
        IQueryable<TicketCommentResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText = null)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.ContentResponse.ToString().Contains(searchText) ||
            item.ResponseDateTime.ToString().Contains(searchText) ||
            item.CreateDateTime.ToString().Contains(searchText));

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<TicketCommentResult> SortTicketCommentResult(
           IQueryable<TicketCommentResult> query,
        SortInput<TicketCommentSortType> sort)
    {
      switch (sort.SortType)
      {
        case TicketCommentSortType.UserId:
          return query.OrderBy(a => a.UserId, sort.SortOrder);

        case TicketCommentSortType.CreateDateTime:
          return query.OrderBy(a => a.CreateDateTime, sort.SortOrder);

        case TicketCommentSortType.ResponseDateTime:
          return query.OrderBy(a => a.ResponseDateTime, sort.SortOrder);

        case TicketCommentSortType.SupporterFullName:
          return query.OrderBy(a => a.SupporterFullName, sort.SortOrder);

        case TicketCommentSortType.Content:
          return query.OrderBy(a => a.ContentResponse, sort.SortOrder);

        case TicketCommentSortType.TicketSoftwareId:
          return query.OrderBy(a => a.TicketSoftwareId, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
