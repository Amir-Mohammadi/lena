using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.ApplicationBase.Bank;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    #region Gets
    public IQueryable<TResult> GetBanks<TResult>(
        Expression<Func<Bank, TResult>> selector,
        TValue<int> id = null,
          TValue<string> title = null,
          TValue<string> description = null,
          TValue<bool> isActive = null

    )
    {

      var query = repository.GetQuery<Bank>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);

      return query.Select(selector);
    }
    #endregion
    #region Get
    public Bank GetBank(int id) => GetBank(selector: e => e, id: id);
    public TResult GetBank<TResult>(Expression<Func<Bank, TResult>> selector, int id)
    {

      var bank = GetBanks(
                selector: selector,
                id: id).SingleOrDefault();
      if (bank == null)
        throw new BankNotFoundException(id);
      return bank;
    }
    #endregion
    #region Add
    public Bank AddBank(
    string title,
    string description,
    bool isActive)
    {

      var bank = repository.Create<Bank>();
      bank.Title = title;
      bank.Description = description;
      bank.IsActive = isActive;
      repository.Add(bank);
      return bank;
    }
    #endregion
    #region Delete
    public void DeleteBank(int id)
    {

      var bank = GetBank(id);
      repository.Delete(bank);
    }
    #endregion
    #region ToComboResult
    public Expression<Func<Bank, BankComboResult>> ToBankComboResult =
        Bank => new BankComboResult()
        {
          Id = Bank.Id,
          Title = Bank.Title,

        };
    #endregion
    #region SortCombo
    public IOrderedQueryable<BankComboResult> SortBankComboResult(
        IQueryable<BankComboResult> query, SortInput<BankComboSortType> type)
    {
      switch (type.SortType)
      {
        case BankComboSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case BankComboSortType.Title:
          return query.OrderBy(a => a.Title, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<Bank, BankResult>> ToBankResult =
       bankResult => new BankResult()
       {
         Id = bankResult.Id,
         Title = bankResult.Title,
         Description = bankResult.Description,
         IsActive = bankResult.IsActive,
         RowVersion = bankResult.RowVersion
       };

    #endregion
    #region Search
    public IQueryable<BankResult> SearchBank(
        IQueryable<BankResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
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
    public IOrderedQueryable<BankResult> SortBankResult(IQueryable<BankResult> query, SortInput<BankSortType> sort)
    {
      switch (sort.SortType)
      {
        case BankSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case BankSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case BankSortType.IsActive:
          return query.OrderBy(a => a.IsActive, sort.SortOrder);
        case BankSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Edit
    public Bank EditBank(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<string> title = null,
        TValue<bool> isActive = null)
    {

      var bank = GetBank(id: id);
      if (title != null)
        bank.Title = title;
      if (description != null)
        bank.Description = description;
      if (isActive != null)
        bank.IsActive = isActive;
      repository.Update(bank, rowVersion);
      return bank;
    }
    #endregion
  }
}
