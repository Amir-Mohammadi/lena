using lena.Services.Common;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting.FinancialAccountDetail;
using lena.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    public FinancialAccountDetail AddFinancialAccountDetail(
        string account,
        FinancialAccountDetailType type,
        string accountOwner,
        byte bankId,
        int financialAccountId)
    {

      var financialAccountDetail = repository.Create<FinancialAccountDetail>();
      financialAccountDetail.Account = account;
      financialAccountDetail.Type = type;
      financialAccountDetail.AccountOwner = accountOwner;
      financialAccountDetail.BankId = bankId;
      financialAccountDetail.FinancialAccountId = financialAccountId;
      repository.Add(financialAccountDetail);
      return financialAccountDetail;
    }
    #endregion

    #region Edit
    public FinancialAccountDetail EditFinancialAccountDetail(
        int id,
        byte[] rowVersion,
        TValue<string> account = null,
        TValue<FinancialAccountDetailType> type = null,
        TValue<string> accountOwner = null,
        TValue<byte> bankId = null,
        TValue<bool> isArchive = null)
    {

      var financialAccountDetail = GetFinancialAccountDetail(id: id);

      return EditFinancialAccountDetail(
                    financialAccountDetail: financialAccountDetail,
                    rowVersion: rowVersion,
                    account: account,
                    type: type,
                    accountOwner: accountOwner,
                    bankId: bankId,
                    isArchive: isArchive);
    }
    public FinancialAccountDetail EditFinancialAccountDetail(
        FinancialAccountDetail financialAccountDetail,
        byte[] rowVersion,
        TValue<string> account = null,
        TValue<FinancialAccountDetailType> type = null,
        TValue<string> accountOwner = null,
        TValue<byte> bankId = null,
        TValue<bool> isArchive = null)
    {

      if (account != null)
        financialAccountDetail.Account = account;

      if (type != null)
        financialAccountDetail.Type = type;

      if (accountOwner != null)
        financialAccountDetail.AccountOwner = accountOwner;

      if (bankId != null)
        financialAccountDetail.BankId = bankId;

      if (isArchive != null)
        financialAccountDetail.IsArchive = isArchive;

      repository.Update(entity: financialAccountDetail, rowVersion: rowVersion);
      return financialAccountDetail;
    }
    #endregion

    #region AddProcess
    public void AddFinancialAccountDetailProcess(
        string account,
        FinancialAccountDetailType type,
        string accountOwner,
        byte bankId,
        int financialAccountId)
    {

      #region GetFinancialAccountDetailAndCheckConstraint
      var checkfinancialAccountDetail = GetFinancialAccountDetails(
          selector: e => e, financialAccountId: financialAccountId,
          type: type,
          isArchive: false);

      if (checkfinancialAccountDetail.Any())
      {
        throw new FinancialAccountCannotHaveOneTypeDetailInNotArchiveMode();
      }
      #endregion

      #region Add
      AddFinancialAccountDetail(
          account: account,
          type: type,
          accountOwner: accountOwner,
          bankId: bankId,
          financialAccountId: financialAccountId);
      #endregion

    }
    #endregion

    #region EditProcess
    public void EditFinancialAccountDetailProcess(
        int id,
        byte[] rowVersion,
        string account,
        FinancialAccountDetailType type,
        string accountOwner,
        byte bankId)
    {


      #region GetFinancailAccountDetail
      var financialAccountDetail = GetFinancialAccountDetail(id: id);
      #endregion

      #region GetFinancialAccountDetailAndCheckConstraint
      var checkfinancialAccountDetail = GetFinancialAccountDetails(
          selector: e => e, financialAccountId: financialAccountDetail.FinancialAccountId,
          type: type,
          isArchive: false);

      if (checkfinancialAccountDetail.Any() && type != financialAccountDetail.Type)
      {
        throw new FinancialAccountCannotHaveOneTypeDetailInNotArchiveMode();
      }
      #endregion

      #region Edit
      EditFinancialAccountDetail(
          financialAccountDetail: financialAccountDetail,
          rowVersion: rowVersion,
          account: account,
          type: type,
          accountOwner: accountOwner,
          bankId: bankId);
      #endregion

    }
    #endregion

    #region Delete
    public void DeleteFinancialAccountDetail(int id)
    {

      var financialAccountDetail = GetFinancialAccountDetail(id: id);

      DeleteFinancialAccountDetail(financialAccountDetail: financialAccountDetail);
    }

    public void DeleteFinancialAccountDetail(
        FinancialAccountDetail financialAccountDetail)
    {


      if (financialAccountDetail.Finances.Any())
      {
        throw new FinancialAccountHasFinanceException(id: financialAccountDetail.Id);
      }
      repository.Delete(financialAccountDetail);
    }
    #endregion

    #region Get
    public FinancialAccountDetail GetFinancialAccountDetail(int id) => GetFinancialAccountDetail(selector: e => e, id: id);
    public TResult GetFinancialAccountDetail<TResult>(
        Expression<Func<FinancialAccountDetail, TResult>> selector,
        int id)
    {

      var financialAccountDetail = GetFinancialAccountDetails(selector: selector,
                id: id).FirstOrDefault();
      if (financialAccountDetail == null)
        throw new RecordNotFoundException(id, typeof(FinancialAccountDetail));
      return financialAccountDetail;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetFinancialAccountDetails<TResult>(
        Expression<Func<FinancialAccountDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int> financialAccountId = null,
        TValue<string> account = null,
        TValue<FinancialAccountDetailType> type = null,
        TValue<string> accountOwner = null,
        TValue<int> bankId = null,
        TValue<bool> isArchive = null)
    {


      var financialAccountDetail = repository.GetQuery<FinancialAccountDetail>();

      if (id != null)
        financialAccountDetail = financialAccountDetail.Where(i => i.Id == id);

      if (account != null)
        financialAccountDetail = financialAccountDetail.Where(i => i.Account == account);

      if (type != null)
        financialAccountDetail = financialAccountDetail.Where(i => i.Type == type);

      if (accountOwner != null)
        financialAccountDetail = financialAccountDetail.Where(i => i.AccountOwner == accountOwner);

      if (bankId != null)
        financialAccountDetail = financialAccountDetail.Where(i => i.BankId == bankId);

      if (isArchive != null)
        financialAccountDetail = financialAccountDetail.Where(i => i.IsArchive == isArchive);

      if (financialAccountId != null)
        financialAccountDetail = financialAccountDetail.Where(i => i.FinancialAccountId == financialAccountId);

      return financialAccountDetail.Select(selector);
    }


    #endregion

    #region Sort

    public IOrderedQueryable<FinancialAccountDetailResult> SortFinancialAccountDetailResult(
      IQueryable<FinancialAccountDetailResult> query,
      SortInput<FinancialAccountDetailSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case FinancialAccountDetailSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case FinancialAccountDetailSortType.FinancialAccountCode:
          return query.OrderBy(i => i.FinancialAccountCode, sortInput.SortOrder);
        case FinancialAccountDetailSortType.Account:
          return query.OrderBy(i => i.Account, sortInput.SortOrder);
        case FinancialAccountDetailSortType.AccountOwner:
          return query.OrderBy(i => i.AccountOwner, sortInput.SortOrder);
        case FinancialAccountDetailSortType.IsArchive:
          return query.OrderBy(i => i.IsArchive, sortInput.SortOrder);
        case FinancialAccountDetailSortType.BankTitle:
          return query.OrderBy(i => i.BankTitle, sortInput.SortOrder);
        case FinancialAccountDetailSortType.FinancialAccountDetailType:
          return query.OrderBy(i => i.FinancialAccountDetailType, sortInput.SortOrder);
        case FinancialAccountDetailSortType.FinancialAccountDescription:
          return query.OrderBy(i => i.FinancialAccountDescription, sortInput.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<FinancialAccountDetailResult> SearchFinancialAccountDetailResult(
         IQueryable<FinancialAccountDetailResult> query,
         string searchText,
         AdvanceSearchItem[] advanceSearchItems
        )
    {

      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.FinancialAccountDescription.Contains(searchText) ||
                item.FinancialAccountCode.Contains(searchText) ||
                item.Account.Contains(searchText) ||
                item.AccountOwner.Contains(searchText) ||
                item.BankTitle.Contains(searchText) ||
                item.BankTitle.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;

    }
    #endregion

    #region ToResult
    public Expression<Func<FinancialAccountDetail, FinancialAccountDetailComboResult>> ToFinancialAccountDetailComboResult =
        financialAccountDetail => new FinancialAccountDetailComboResult
        {
          Id = financialAccountDetail.Id,
          FinancialAccountDetailType = financialAccountDetail.Type,
          AccountOwner = financialAccountDetail.AccountOwner,
          BankTitle = financialAccountDetail.Bank.Title,
          Account = financialAccountDetail.Account,

        };

    public Expression<Func<FinancialAccountDetail, FinancialAccountDetailResult>> ToFinancialAccountDetailResult =
       financialAccountDetail => new FinancialAccountDetailResult
       {
         Id = financialAccountDetail.Id,
         FinancialAccountDetailType = financialAccountDetail.Type,
         FinancialAccountCode = financialAccountDetail.FinancialAccount.Code,
         FinancialAccountDescription = financialAccountDetail.FinancialAccount.Description,
         AccountOwner = financialAccountDetail.AccountOwner,
         BankTitle = financialAccountDetail.Bank.Title,
         Account = financialAccountDetail.Account,
         BankId = financialAccountDetail.BankId,
         IsArchive = financialAccountDetail.IsArchive,
         RowVersion = financialAccountDetail.RowVersion


       };
    #endregion

    #region ToggleFinancialAccountDetailArchive
    public void ToggleFinancialAccountDetailArchiveProcess(
     int financialAccountId,
     int financialAccountDetailId,
     byte[] rowVersion
    )
    {

      #region GetFinancialAccountDetailAndCheckConstraint
      var destArchive = true;
      var financialAccountDetail = GetFinancialAccountDetail(id: financialAccountDetailId);
      if (financialAccountDetail.IsArchive)
        destArchive = false;

      var financialAccountDetailCheck = GetFinancialAccountDetails(
                selector: e => e, financialAccountId: financialAccountId,
                type: financialAccountDetail.Type,
                isArchive: destArchive)

                .Any();
      if (financialAccountDetailCheck)
      {
        throw new FinancialAccountDetailToggleArchiveException();
      }
      #endregion

      #region EditFinancialAccountDetail
      EditFinancialAccountDetail(
          financialAccountDetail: financialAccountDetail,
          rowVersion: rowVersion,
          isArchive: destArchive);
      #endregion

    }
    #endregion
  }
}
