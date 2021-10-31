using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.EmployeeCartable.ProposalQaReviews;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.EmployeeCartable
{
  public partial class EmployeeCartable
  {
    #region Gets
    public IQueryable<TResult> GetProposalQaReviews<TResult>(
        Expression<Func<ProposalQAReview, TResult>> selector,
          TValue<int> id = null,
          TValue<string> reviewResult = null,
          TValue<DateTime> reviewDateTime = null,
          TValue<int> responsibleUserId = null,
          TValue<int> proposalId = null
    )
    {

      var query = repository.GetQuery<ProposalQAReview>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (reviewResult != null)
        query = query.Where(i => i.ReviewResult == reviewResult);
      if (reviewDateTime != null)
        query = query.Where(i => i.ReviewDateTime == reviewDateTime);
      if (responsibleUserId != null)
        query = query.Where(i => i.ResponsibleUserId == responsibleUserId);
      if (proposalId != null)
        query = query.Where(i => i.ProposalId == proposalId);

      return query.Select(selector);
    }
    #endregion
    #region Get
    public ProposalQAReview GetProposalQaReview(int id) => GetProposalQaReview(selector: e => e, id: id);
    public TResult GetProposalQaReview<TResult>(Expression<Func<ProposalQAReview, TResult>> selector, int id)
    {

      var proposal = GetProposalQaReviews(
                selector: selector,
                id: id)


            .FirstOrDefault();

      if (proposal == null)
        throw new ProposalQAReviewNotFoundException(id);

      return proposal;
    }
    #endregion
    #region Add
    public ProposalQAReview AddProposalQaReview(
        string reviewResult,
        DateTime reviewDateTime,
        int responsibleUserId,
        int proposalId
        )
    {

      var qaReview = repository.Create<ProposalQAReview>();
      qaReview.ReviewResult = reviewResult;
      qaReview.ReviewDateTime = reviewDateTime;
      qaReview.ResponsibleUserId = responsibleUserId;
      qaReview.ProposalId = proposalId;
      qaReview.DateTime = DateTime.UtcNow;
      qaReview.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(qaReview);
      return qaReview;
    }
    #endregion
    #region Delete
    public void DeleteProposalQaReview(int id)
    {

      var qaReview = GetProposalQaReview(id);
      DeleteProposalQaReview(qaReview);
    }

    public void DeleteProposalQaReview(ProposalQAReview qaReview)
    {

      repository.Delete(qaReview);
    }
    #endregion

    #region ToResult
    public Expression<Func<ProposalQAReview, ProposalQaReviewResult>> ToProposalQaReviewResult =
       result => new ProposalQaReviewResult()
       {
         Id = result.Id,
         ProposalId = result.ProposalId,
         ResponsibleUserId = result.ResponsibleUserId,
         ResponsibleUserFullName = result.ResponsibleUser.Employee.FirstName + " " + result.ResponsibleUser.Employee.LastName,
         ReviewResult = result.ReviewResult,
         ReviewDateTime = result.ReviewDateTime,
         UserId = result.UserId,
         UserFullName = result.User.Employee.FirstName + " " + result.User.Employee.LastName,
         DateTime = result.DateTime,
         RowVersion = result.RowVersion
       };

    #endregion
    #region Search
    public IQueryable<ProposalQaReviewResult> SearchProposalQaReview(
        IQueryable<ProposalQaReviewResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.ReviewResult.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }

    #endregion
    #region SortResult
    public IOrderedQueryable<ProposalQaReviewResult> SortProposalQaReviewResult(IQueryable<ProposalQaReviewResult> query, SortInput<ProposalQaReviewSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProposalQaReviewSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProposalQaReviewSortType.UserFullName:
          return query.OrderBy(a => a.UserFullName, sort.SortOrder);
        case ProposalQaReviewSortType.ReviewResult:
          return query.OrderBy(a => a.ReviewResult, sort.SortOrder);
        case ProposalQaReviewSortType.ResponsibleUserFullName:
          return query.OrderBy(a => a.ResponsibleUserFullName, sort.SortOrder);
        case ProposalQaReviewSortType.ReviewDateTime:
          return query.OrderBy(a => a.ReviewDateTime, sort.SortOrder);
        case ProposalQaReviewSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException($"{nameof(ProposalQaReviewSortType)} sort parameter is Not supported!");
      }
    }
    #endregion
    #region Edit
    public ProposalQAReview EditProposalQaReview(
        int id,
        byte[] rowVersion,
        TValue<string> reviewResult = null,
        TValue<DateTime> reviewDateTime = null,
        TValue<int> responsibleUserId = null,
        TValue<int> proposalId = null
        )
    {

      var qaReview = GetProposalQaReview(id: id);

      if (reviewResult != null)
        qaReview.ReviewResult = reviewResult;
      if (reviewDateTime != null)
        qaReview.ReviewDateTime = reviewDateTime;
      if (responsibleUserId != null)
        qaReview.ResponsibleUserId = responsibleUserId;
      if (proposalId != null)
        qaReview.ProposalId = proposalId;

      repository.Update(qaReview, rowVersion);
      return qaReview;
    }
    #endregion
  }
}
