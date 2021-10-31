using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.BankOrderContractType;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Gets
    public IQueryable<TResult> GetBankOrderContractTypes<TResult>(
        Expression<Func<BankOrderContractType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<bool> isActive = null
    )
    {

      var query = repository.GetQuery<BankOrderContractType>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      return query.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<BankOrderContractType, BankOrderContractTypeResult>> ToBankOrderContractTypeResult =
        BankOrderContractType => new BankOrderContractTypeResult()
        {
          Id = BankOrderContractType.Id,
          Title = BankOrderContractType.Title,
          IsActive = BankOrderContractType.IsActive
        };
    #endregion
    #region Sort
    public IOrderedQueryable<BankOrderContractTypeResult> SortBankOrderContractTypeResult(
        IQueryable<BankOrderContractTypeResult> query, SortInput<BankOrderContractTypeSortType> type)
    {
      switch (type.SortType)
      {
        case BankOrderContractTypeSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case BankOrderContractTypeSortType.Title:
          return query.OrderBy(a => a.Title, type.SortOrder);
        case BankOrderContractTypeSortType.IsActive:
          return query.OrderBy(a => a.IsActive, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Get
    public BankOrderContractType GetBankOrderContractType(int id) => GetBankOrderContractType(selector: e => e, id: id);
    public TResult GetBankOrderContractType<TResult>(
        Expression<Func<BankOrderContractType, TResult>> selector,
        int id)
    {

      var bankOrderContractType = GetBankOrderContractTypes(selector: selector,
                id: id).FirstOrDefault();
      if (bankOrderContractType == null)
        throw new RecordNotFoundException(id, typeof(BankOrderContractType));
      return bankOrderContractType;
    }
    #endregion
    #region Delete
    public void DeleteBankOrderContractType(int id)
    {

      var bankOrderStep = GetBankOrderContractType(id: id);
      var bankOrders = GetBankOrders(
                e => e,
                bankOrderContractTypeId: bankOrderStep.Id);
      if (bankOrders.Any())
      {
        throw new CanNotDeleteBankOrderContractTypeIsUsedException(bankOrderStep.Id);
      }
      repository.Delete(bankOrderStep);
    }
    #endregion
    #region Search
    public IQueryable<BankOrderContractTypeResult> SearchBankOrderContractTypeResult(
         IQueryable<BankOrderContractTypeResult> query,
         string searchText,
         AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
        item.Id.ToString().Contains(searchText) ||
        item.Title.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Edit
    public BankOrderContractType EditBankOrderContractType(
        int id,
        string title,
        bool isActive,
        byte[] rowVersion)
    {

      var bankOrderContractType = GetBankOrderContractType(id: id);
      if (title != null)
        bankOrderContractType.Title = title;
      if (isActive != null)
        bankOrderContractType.IsActive = isActive;
      repository.Update(entity: bankOrderContractType, rowVersion: bankOrderContractType.RowVersion);
      return bankOrderContractType;
    }

    #endregion
    #region Add
    public BankOrderContractType AddBankOrderContractType(string title,
        bool isActive)
    {

      var bankOrderContractType = repository.Create<BankOrderContractType>();
      bankOrderContractType.Title = title;
      bankOrderContractType.IsActive = isActive;
      repository.Add(bankOrderContractType);
      return bankOrderContractType;
    }
    #endregion
    #region GetCombo
    public IQueryable<TResult> GetBankOrderContractTypesCombo<TResult>(
     Expression<Func<BankOrderContractType, TResult>> selector,
        TValue<int> id = null,
          TValue<string> title = null,
          TValue<bool> isActive = null)
    {

      var query = repository.GetQuery<BankOrderContractType>();
      if (id != null)
        query = query.Where(r => r.Id == id);
      if (title != null)
        query = query.Where(r => r.Title == title);
      if (isActive != null)
        query = query.Where(r => r.IsActive == isActive);
      return query.Select(selector);
    }

    #endregion
    #region ToComboResult
    public Expression<Func<BankOrderContractType, BankOrderContractTypeComboResult>> ToBankOrderContractTypeComboResult =
        contractType => new BankOrderContractTypeComboResult
        {
          Id = contractType.Id,
          Title = contractType.Title,
          IsActive = contractType.IsActive,
          RowVersion = contractType.RowVersion
        };
    #endregion
  }
}
