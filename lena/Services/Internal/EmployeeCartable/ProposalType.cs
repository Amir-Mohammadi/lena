using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.EmployeeCartable.ProposalTypes;
using System;
// using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.EmployeeCartable
{
  public partial class EmployeeCartable
  {
    #region Gets
    public IQueryable<TResult> GetProposalTypes<TResult>(
        Expression<Func<ProposalType, TResult>> selector,
          TValue<int> id = null,
          TValue<string> title = null,
          TValue<bool> isActive = null
    )
    {

      var query = repository.GetQuery<ProposalType>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);

      return query.Select(selector);
    }
    #endregion
    #region Get
    public ProposalType GetProposalType(int id) => GetProposalType(selector: e => e, id: id);
    public TResult GetProposalType<TResult>(Expression<Func<ProposalType, TResult>> selector, int id)
    {

      var proposal = GetProposalTypes(
                selector: selector,
                id: id)


            .FirstOrDefault();

      if (proposal == null)
        throw new ProposalTypeNotFoundException(id);

      return proposal;
    }
    #endregion
    #region Add
    public ProposalType AddProposalType(
        string title,
        string description,
        bool isActive
        )
    {

      var proposalType = repository.Create<ProposalType>();
      proposalType.Title = title;
      proposalType.Description = description;
      proposalType.IsActive = isActive;
      repository.Add(proposalType);
      return proposalType;
    }
    #endregion
    #region Delete
    public void DeleteProposalType(int id)
    {

      var bank = GetProposalType(id);
      repository.Delete(bank);
    }
    #endregion

    #region ToResult
    public Expression<Func<ProposalType, ProposalTypeResult>> ToProposalTypeResult =
       result => new ProposalTypeResult()
       {
         Id = result.Id,
         Title = result.Title,
         Description = result.Description,
         IsActive = result.IsActive,
         RowVersion = result.RowVersion
       };

    #endregion
    #region Search
    public IQueryable<ProposalTypeResult> SearchProposalType(
        IQueryable<ProposalTypeResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Title.Contains(searchText) ||
                item.Description.Contains(searchText)
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
    public IOrderedQueryable<ProposalTypeResult> SortProposalTypeResult(IQueryable<ProposalTypeResult> query, SortInput<ProposalTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProposalTypeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProposalTypeSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case ProposalTypeSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case ProposalTypeSortType.IsActive:
          return query.OrderBy(a => a.IsActive, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException($"{nameof(ProposalTypeSortType)} sort parameter is Not supported!");
      }
    }
    #endregion
    #region Edit
    public ProposalType EditProposalType(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<bool> isActive = null
        )
    {

      var proposalType = GetProposalType(id: id);

      if (title != null)
        proposalType.Title = title;
      if (description != null)
        proposalType.Description = description;
      if (isActive != null)
        proposalType.IsActive = isActive;

      repository.Update(proposalType, rowVersion);
      return proposalType;
    }
    #endregion
  }
}
