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
using lena.Models.Supplies.LadingBankOrderStatus;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public LadingBankOrderStatus AddLadingBankOrderStatus(
        string code,
        string name)
    {
      var bankOrderStatusType = repository.Create<LadingBankOrderStatus>();
      bankOrderStatusType.Code = code.ToUpper();
      bankOrderStatusType.Name = name;
      repository.Add(bankOrderStatusType);
      return bankOrderStatusType;

    }
    #endregion
    #region Edit
    public LadingBankOrderStatus EditLadingBankOrderStatus(
       int bankOrderStatusId,
       byte[] rowVersion,
       TValue<string> code = null,
       TValue<string> name = null)
    {
      var bankOrderStatusType = GetLadingBankOrderStatus(bankOrderStatusId);
      if (code != null)
        bankOrderStatusType.Code = code.Value.ToUpper();
      if (name != null)
        bankOrderStatusType.Name = name;
      repository.Update(bankOrderStatusType, rowVersion);
      return bankOrderStatusType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetLadingBankOrderStatuses<TResult>(
        Expression<Func<LadingBankOrderStatus, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null
        )
    {
      var query = repository.GetQuery<LadingBankOrderStatus>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (code != null)
        query = query.Where(a => a.Code == code);
      if (name != null)
        query = query.Where(a => a.Name == name);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public LadingBankOrderStatus GetLadingBankOrderStatus(int id) => GetLadingBankOrderStatus(selector: e => e, id: id);
    public TResult GetLadingBankOrderStatus<TResult>(
           Expression<Func<LadingBankOrderStatus, TResult>> selector,
        int id)
    {
      var BankOrderStatusType = GetLadingBankOrderStatuses(selector: selector, id: id)
            .FirstOrDefault();
      if (BankOrderStatusType == null)
        throw new LadingBankOrderStatusTypeNotFoundException(id);
      return BankOrderStatusType;
    }
    #endregion
    #region ToResult
    public Expression<Func<LadingBankOrderStatus, LadingBankOrderStatusResult>> ToLadingBankOrderStatusesResult =
        (bankOrderStatusType) => new LadingBankOrderStatusResult
        {
          Id = bankOrderStatusType.Id,
          Code = bankOrderStatusType.Code,
          Name = bankOrderStatusType.Name,
          RowVersion = bankOrderStatusType.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<LadingBankOrderStatusResult> SearchLadingBankOrderStatusesResults(
        IQueryable<LadingBankOrderStatusResult> query,
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
    public IQueryable<LadingBankOrderStatusResult> SortLadingBankOrderStatusesResult(IQueryable<LadingBankOrderStatusResult> query,
        SortInput<LadingBankOrderStatusSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case LadingBankOrderStatusSortType.Id:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
        case LadingBankOrderStatusSortType.Code:
          query = query.OrderBy(x => x.Code, sortInput.SortOrder);
          break;
        case LadingBankOrderStatusSortType.Name:
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
    public void DeleteLadingBankOrderStatus(int id)
    {
      var ladingBankOrderStatus = GetLadingBankOrderStatus(id);
      if (ladingBankOrderStatus.LadingBankOrderLogs.Any())
      {
        throw new CannotDeleteLadingBankOrderStatusHasBeenUsedInLogs(ladingBankOrderStatus.Id, ladingBankOrderStatus.Name);
      }
      repository.Delete(ladingBankOrderStatus);
    }
    #endregion
    #region ToComboResult
    public Expression<Func<LadingBankOrderStatus, LadingBankOrderStatusComboResult>> ToLadingBankOrderStateComboResult =
        bankOrderStatus => new LadingBankOrderStatusComboResult
        {
          Id = bankOrderStatus.Id,
          Code = bankOrderStatus.Code,
          Name = bankOrderStatus.Name,
          RowVersion = bankOrderStatus.RowVersion
        };
    #endregion
  }
}