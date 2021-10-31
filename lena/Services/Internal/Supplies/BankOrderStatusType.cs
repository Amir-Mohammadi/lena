using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.BankOrderStatusType;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public BankOrderStatusType AddBankOrderStatusType(
        string code,
        string name)
    {
      var bankOrderStatusType = repository.Create<BankOrderStatusType>();
      bankOrderStatusType.Code = code.ToUpper();
      bankOrderStatusType.Name = name;
      repository.Add(bankOrderStatusType);
      return bankOrderStatusType;

    }
    #endregion
    #region Edit
    public BankOrderStatusType EditBankOrderStatusType(
       int bankOrderStatusId,
       byte[] rowVersion,
       TValue<string> code = null,
       TValue<string> name = null)
    {
      var bankOrderStatusType = GetBankOrderStatusType(bankOrderStatusId);
      if (code != null)
        bankOrderStatusType.Code = code.Value.ToUpper();
      if (name != null)
        bankOrderStatusType.Name = name;
      repository.Update(bankOrderStatusType, rowVersion);
      return bankOrderStatusType;

    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetBankOrderStatusTypes<TResult>(
        Expression<Func<BankOrderStatusType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null
        )
    {
      var query = repository.GetQuery<BankOrderStatusType>();
      if (code != null)
        query = query.Where(a => a.Code == code);
      if (name != null)
        query = query.Where(a => a.Name == name);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public BankOrderStatusType GetBankOrderStatusType(int id) => GetBankOrderStatusType(selector: e => e, id: id);
    public TResult GetBankOrderStatusType<TResult>(
           Expression<Func<BankOrderStatusType, TResult>> selector,
        int id)
    {
      var BankOrderStatusType = GetBankOrderStatusTypes(selector: selector, id: id)


            .FirstOrDefault();
      if (BankOrderStatusType == null)
        throw new BankOrderStatusTypeNotFoundException(id);
      return BankOrderStatusType;
    }
    #endregion
    #region ToResult
    public Expression<Func<BankOrderStatusType, BankOrderStatusTypeResult>> ToBankOrderStatusTypesResult =
        (bankOrderStatusType) => new BankOrderStatusTypeResult
        {
          Id = bankOrderStatusType.Id,
          Code = bankOrderStatusType.Code,
          Name = bankOrderStatusType.Name,
          RowVersion = bankOrderStatusType.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<BankOrderStatusTypeResult> SearchBankOrderStatusTypesResults(
        IQueryable<BankOrderStatusTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i => i.Code.Contains(searchText) || i.Name.Contains(searchText));
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region sort
    public IQueryable<BankOrderStatusTypeResult> SortBankOrderStatusTypesResult(IQueryable<BankOrderStatusTypeResult> query,
        SortInput<BankOrderStatusTypeSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case BankOrderStatusTypeSortType.Id:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
        case BankOrderStatusTypeSortType.Code:
          query = query.OrderBy(x => x.Code, sortInput.SortOrder);
          break;
        case BankOrderStatusTypeSortType.Name:
          query = query.OrderBy(x => x.Name, sortInput.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
      }
      return query;
    }
    #endregion
    #region Delete
    public void DeleteBankOrderStatusType(int id)
    {
      var bankOrderStatusType = GetBankOrderStatusType(id);
      if (bankOrderStatusType.BankOrderLogs.Any())
      {
        throw new CannotDeleteBankOrderStatusHasBeenUsedInLogs(bankOrderStatusType.Id, bankOrderStatusType.Name);
      }
      repository.Delete(bankOrderStatusType);
    }
    #endregion
    #region ToComboResult
    public Expression<Func<BankOrderStatusType, BankOrderStatusTypeComboResult>> ToBankOrderStateComboResult =
        bankOrderStatusType => new BankOrderStatusTypeComboResult
        {
          Id = bankOrderStatusType.Id,
          Code = bankOrderStatusType.Code,
          Name = bankOrderStatusType.Name,
          RowVersion = bankOrderStatusType.RowVersion
        };
    #endregion
  }
}