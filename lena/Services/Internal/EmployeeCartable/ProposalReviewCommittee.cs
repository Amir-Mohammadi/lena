using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.EmployeeCartable.ProposalReviewCommittees;
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
    public IQueryable<TResult> GetProposalReviewCommittees<TResult>(
        Expression<Func<ProposalReviewCommittee, TResult>> selector,
          TValue<int> id = null,
          TValue<bool> isConfirmed = null,
          TValue<string> reviewResult = null,
          TValue<DateTime> reviewDateTime = null,
          TValue<int> responsibleUserId = null,
          TValue<int> proposalId = null
    )
    {

      var query = repository.GetQuery<ProposalReviewCommittee>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (isConfirmed != null)
        query = query.Where(i => i.IsConfirmed == isConfirmed);
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
    public ProposalReviewCommittee GetProposalReviewCommittee(int id) => GetProposalReviewCommittee(selector: e => e, id: id);
    public TResult GetProposalReviewCommittee<TResult>(Expression<Func<ProposalReviewCommittee, TResult>> selector, int id)
    {

      var proposal = GetProposalReviewCommittees(
                selector: selector,
                id: id)


            .FirstOrDefault();

      if (proposal == null)
        throw new ProposalReviewCommitteeNotFoundException(id);

      return proposal;
    }
    #endregion
    #region Add
    public ProposalReviewCommittee AddProposalReviewCommittee(
        bool isConfirmed,
        string reviewResult,
        DateTime reviewDateTime,
        int responsibleUserId,
        int proposalId
        )
    {

      var reviewCommittee = repository.Create<ProposalReviewCommittee>();
      reviewCommittee.IsConfirmed = isConfirmed;
      reviewCommittee.ReviewResult = reviewResult;
      reviewCommittee.ReviewDateTime = reviewDateTime;
      reviewCommittee.ResponsibleUserId = responsibleUserId;
      reviewCommittee.ProposalId = proposalId;
      reviewCommittee.DateTime = DateTime.UtcNow;
      reviewCommittee.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(reviewCommittee);
      return reviewCommittee;
    }
    #endregion
    #region Delete
    public void DeleteProposalReviewCommittee(int id)
    {

      var reviewCommittee = GetProposalReviewCommittee(id);
      DeleteProposalReviewCommittee(reviewCommittee);
    }

    public void DeleteProposalReviewCommittee(ProposalReviewCommittee reviewCommittee)
    {

      repository.Delete(reviewCommittee);
    }
    #endregion

    #region ToResult
    public Expression<Func<ProposalReviewCommittee, ProposalReviewCommitteeResult>> ToProposalReviewCommitteeResult =
       result => new ProposalReviewCommitteeResult()
       {
         Id = result.Id,
         IsConfirmed = result.IsConfirmed,
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
    public IQueryable<ProposalReviewCommitteeResult> SearchProposalReviewCommittee(
        IQueryable<ProposalReviewCommitteeResult> query,
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
    public IOrderedQueryable<ProposalReviewCommitteeResult> SortProposalReviewCommitteeResult(IQueryable<ProposalReviewCommitteeResult> query, SortInput<ProposalReviewCommitteeSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProposalReviewCommitteeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProposalReviewCommitteeSortType.UserFullName:
          return query.OrderBy(a => a.UserFullName, sort.SortOrder);
        case ProposalReviewCommitteeSortType.IsConfirmed:
          return query.OrderBy(a => a.IsConfirmed, sort.SortOrder);
        case ProposalReviewCommitteeSortType.ReviewResult:
          return query.OrderBy(a => a.ReviewResult, sort.SortOrder);
        case ProposalReviewCommitteeSortType.ResponsibleUserFullName:
          return query.OrderBy(a => a.ResponsibleUserFullName, sort.SortOrder);
        case ProposalReviewCommitteeSortType.ReviewDateTime:
          return query.OrderBy(a => a.ReviewDateTime, sort.SortOrder);
        case ProposalReviewCommitteeSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException($"{nameof(ProposalReviewCommitteeSortType)} sort parameter is Not supported!");
      }
    }
    #endregion
    #region Edit
    public ProposalReviewCommittee EditProposalReviewCommittee(
        int id,
        byte[] rowVersion,
        TValue<bool> isConfirmed = null,
        TValue<string> reviewResult = null,
        TValue<DateTime> reviewDateTime = null,
        TValue<int> responsibleUserId = null,
        TValue<int> proposalId = null
        )
    {

      var reviewCommittee = GetProposalReviewCommittee(id: id);

      if (isConfirmed != null)
        reviewCommittee.IsConfirmed = isConfirmed;
      if (reviewResult != null)
        reviewCommittee.ReviewResult = reviewResult;
      if (reviewDateTime != null)
        reviewCommittee.ReviewDateTime = reviewDateTime;
      if (responsibleUserId != null)
        reviewCommittee.ResponsibleUserId = responsibleUserId;
      if (proposalId != null)
        reviewCommittee.ProposalId = proposalId;

      repository.Update(reviewCommittee, rowVersion);
      return reviewCommittee;
    }
    #endregion
  }
}
