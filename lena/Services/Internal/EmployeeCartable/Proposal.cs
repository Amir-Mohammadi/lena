using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.EmployeeCartable.Proposals;
using Stimulsoft.Report.Components;
using System;
// using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.EmployeeCartable
{
  public partial class EmployeeCartable
  {
    #region Gets
    public IQueryable<TResult> GetProposals<TResult>(
        Expression<Func<Proposal, TResult>> selector,
          TValue<int> id = null,
          TValue<int> proposalTypeId = null,
          TValue<ProposalStatus> status = null,
          TValue<bool> isOpen = null,
          TValue<bool> isEffective = null,
          TValue<bool> isIncognitoUser = null,
          TValue<int> userId = null,
          TValue<int[]> userIds = null,
          TValue<DateTime> fromDateTime = null,
          TValue<DateTime> toDateTime = null

    )
    {

      var query = repository.GetQuery<Proposal>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (proposalTypeId != null)
        query = query.Where(i => i.ProposalTypeId == proposalTypeId);
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (isOpen != null)
        query = query.Where(i => i.IsOpen == isOpen);
      if (isEffective != null)
        query = query.Where(i => i.IsEffective == isOpen);
      if (isIncognitoUser != null)
        query = query.Where(i => i.IsIncognitoUser == isIncognitoUser);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (userIds != null && userIds.Value.Length != 0)
        query = query.Where(i => userIds.Value.Contains(i.UserId));
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);


      return query.Select(selector);
    }
    #endregion
    #region Get
    public Proposal GetProposal(int id) => GetProposal(selector: e => e, id: id);
    public TResult GetProposal<TResult>(Expression<Func<Proposal, TResult>> selector, int id)
    {

      var proposal = GetProposals(
                selector: selector,
                id: id)


            .FirstOrDefault();

      if (proposal == null)
        throw new ProposalNotFoundException(id);

      return proposal;
    }
    #endregion
    #region Add
    public Proposal AddProposal(
          string currentSituationDescription,
          string proposalDescription,
          string proposalEffect,
          int proposalTypeId,
          bool isIncognitoUser
        )
    {

      var proposal = repository.Create<Proposal>();
      proposal.CurrentSituationDescription = currentSituationDescription;
      proposal.ProposalDescription = proposalDescription;
      proposal.ProposalEffect = proposalEffect;
      proposal.ProposalTypeId = proposalTypeId;
      proposal.Status = ProposalStatus.NotAction;
      proposal.IsIncognitoUser = isIncognitoUser;
      proposal.IsOpen = true;
      proposal.IsEffective = null;
      proposal.DateTime = DateTime.UtcNow;
      proposal.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(proposal);
      return proposal;
    }
    #endregion
    #region Delete
    public void DeleteProposal(int id)
    {

      var proposal = GetProposal(id);
      repository.Delete(proposal);
    }

    public void DeleteProposal(Proposal proposal)
    {

      repository.Delete(proposal);
    }

    #endregion

    #region ToResult
    public Expression<Func<Proposal, ProposalResult>> ToProposalResult =
       result => new ProposalResult()
       {
         Id = result.Id,
         CurrentSituationDescription = result.CurrentSituationDescription,
         ProposalDescription = result.ProposalDescription,
         ProposalEffect = result.ProposalEffect,
         IsIncognitoUser = result.IsIncognitoUser,
         IsOpen = result.IsOpen,
         IsEffective = result.IsEffective,
         ProposalTypeId = result.ProposalTypeId,
         ProposalTypeName = result.ProposalType.Title,
         Status = result.Status,
         UserId = (result.IsIncognitoUser && result.UserId != App.Providers.Security.CurrentLoginData.UserId) ? (int?)null : result.UserId,
         UserFullName = (result.IsIncognitoUser && result.UserId != App.Providers.Security.CurrentLoginData.UserId) ? null : (result.User.Employee.FirstName + " " + result.User.Employee.LastName),
         DateTime = result.DateTime,
         RowVersion = result.RowVersion
       };

    #endregion
    #region Search
    public IQueryable<ProposalResult> SearchProposal(
        IQueryable<ProposalResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.CurrentSituationDescription.Contains(searchText) ||
                item.ProposalDescription.Contains(searchText) ||
                item.ProposalEffect.Contains(searchText) ||
                item.ProposalTypeName.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }

    #endregion
    #region SortResult
    public IOrderedQueryable<ProposalResult> SortProposalResult(IQueryable<ProposalResult> query, SortInput<ProposalSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProposalSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProposalSortType.CurrentSituationDescription:
          return query.OrderBy(a => a.CurrentSituationDescription, sort.SortOrder);
        case ProposalSortType.ProposalDescription:
          return query.OrderBy(a => a.ProposalDescription, sort.SortOrder);
        case ProposalSortType.ProposalEffect:
          return query.OrderBy(a => a.ProposalEffect, sort.SortOrder);
        case ProposalSortType.ProposalTypeName:
          return query.OrderBy(a => a.ProposalTypeName, sort.SortOrder);
        case ProposalSortType.IsOpen:
          return query.OrderBy(a => a.IsOpen, sort.SortOrder);
        case ProposalSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case ProposalSortType.IsIncognitoUser:
          return query.OrderBy(a => a.IsIncognitoUser, sort.SortOrder);
        case ProposalSortType.UserFullName:
          return query.OrderBy(a => a.UserFullName, sort.SortOrder);
        case ProposalSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException($"{nameof(ProposalSortType)} sort parameter is Not supported!");
      }
    }
    #endregion
    #region Edit

    public Proposal EditProposal(
        Proposal proposal,
        byte[] rowVersion,
        TValue<string> currentSituationDescription = null,
        TValue<string> proposalDescription = null,
        TValue<string> proposalEffect = null,
        TValue<int> proposalTypeId = null,
        TValue<ProposalStatus> status = null,
        TValue<bool> isOpen = null,
        TValue<bool?> isEffective = null,
        TValue<bool> isIncognitoUser = null
        )
    {

      if (currentSituationDescription != null)
        proposal.CurrentSituationDescription = currentSituationDescription;
      if (proposalDescription != null)
        proposal.ProposalDescription = proposalDescription;
      if (proposalEffect != null)
        proposal.ProposalEffect = proposalEffect;
      if (proposalTypeId != null)
        proposal.ProposalTypeId = proposalTypeId;
      if (status != null)
        proposal.Status = status;
      if (isIncognitoUser != null)
        proposal.IsIncognitoUser = isIncognitoUser;
      if (isOpen != null)
        proposal.IsOpen = isOpen;
      if (isEffective != null)
        proposal.IsEffective = isEffective;

      repository.Update(proposal, rowVersion);
      return proposal;
    }

    public Proposal EditProposal(
        int id,
        byte[] rowVersion,
        TValue<string> currentSituationDescription = null,
        TValue<string> proposalDescription = null,
        TValue<string> proposalEffect = null,
        TValue<int> proposalTypeId = null,
        TValue<ProposalStatus> status = null,
        TValue<bool> isOpen = null,
        TValue<bool?> isEffective = null,
        TValue<bool> isIncognitoUser = null
        )
    {

      var proposal = GetProposal(id: id);
      EditProposal(
                proposal: proposal,
                rowVersion: rowVersion,
                currentSituationDescription: currentSituationDescription,
                proposalDescription: proposalDescription,
                proposalEffect: proposalEffect,
                proposalTypeId: proposalTypeId,
                status: status,
                isOpen: isOpen,
                isEffective: isEffective,
                isIncognitoUser: isIncognitoUser
                );

      return proposal;
    }
    #endregion
  }
}
