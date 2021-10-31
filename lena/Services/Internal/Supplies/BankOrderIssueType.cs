using System;
using System.Linq;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.BankOrderIssueType;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Supplies.BankOrderIssueType;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public BankOrderIssueType AddBankOrderIssueType(string name)
    {

      var bankOrderIssueType = repository.Create<BankOrderIssueType>();
      bankOrderIssueType.Name = name;
      repository.Add(bankOrderIssueType);
      return bankOrderIssueType;
    }
    #endregion

    #region Edit
    public BankOrderIssueType EditBankOrderIssueType(
        byte[] rowVersion,
        int id,
        TValue<string> name = null)
    {

      var bankOrderIssueType = GetBankOrderIssueType(id: id);
      if (name != null)
        bankOrderIssueType.Name = name;
      repository.Update(entity: bankOrderIssueType, rowVersion: bankOrderIssueType.RowVersion);
      return bankOrderIssueType;
    }
    #endregion

    #region Delete
    public void DeleteBankOrderIssueType(int id)
    {

      var bankOrderIssueType = GetBankOrderIssueType(id: id);
      repository.Delete(bankOrderIssueType);
    }
    #endregion

    #region Gets
    public IQueryable<BankOrderIssueType> GetBankOrderIssueTypes(TValue<int> id = null, TValue<string> name = null)
    {

      var isIdNUll = id == null;
      var isNameNull = name == null;
      var bankOrderIssueTypes = from bankOrderIssueType in repository.GetQuery<BankOrderIssueType>()
                                where (isIdNUll || bankOrderIssueType.Id == id) &&
                                            (isNameNull || bankOrderIssueType.Name == name)
                                select bankOrderIssueType;
      return bankOrderIssueTypes;
    }
    #endregion

    #region Get
    public BankOrderIssueType GetBankOrderIssueType(int id)
    {

      var bankOrderIssueType = GetBankOrderIssueTypes(id: id).FirstOrDefault();
      if (bankOrderIssueType == null)
        throw new BankOrderIssueTypeNotFoundException(id);
      return bankOrderIssueType;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<BankOrderIssueTypeResult> SortBankOrderIssueTypeResult(IQueryable<BankOrderIssueTypeResult> query, SortInput<BankOrderIssueTypeSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case BankOrderIssueTypeSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case BankOrderIssueTypeSortType.Name:
          return query.OrderBy(r => r.Name, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToBankOrderIssueTypeResultQuery
    public IQueryable<BankOrderIssueTypeResult> ToBankOrderIssueTypeResultQuery(IQueryable<BankOrderIssueType> query)
    {
      var result = from item in query
                   select new BankOrderIssueTypeResult()
                   {
                     Id = item.Id,
                     Name = item.Name,
                     RowVersion = item.RowVersion
                   };
      return result;
    }
    #endregion

    #region ToBankOrderIssueTypeResult
    public BankOrderIssueTypeResult ToBankOrderIssueTypeResult(BankOrderIssueType bankOrderIssueType)
    {
      var result = new BankOrderIssueTypeResult()
      {
        Id = bankOrderIssueType.Id,
        Name = bankOrderIssueType.Name,
        RowVersion = bankOrderIssueType.RowVersion
      };
      return result;
    }
    #endregion

    #region Search
    public IQueryable<BankOrderIssueTypeResult> SearchBankOrderIssueTypeResult(
      IQueryable<BankOrderIssueTypeResult> query,
      string searchText,
      AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Name.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region ToBankOrderIssueTypeComboList
    public IQueryable<BankOrderIssueTypeComboResult> ToBankOrderIssueTypeComboList(IQueryable<BankOrderIssueType> input)
    {
      return from BankOrderIssueType in input
             select new BankOrderIssueTypeComboResult()
             {
               Id = BankOrderIssueType.Id,
               Name = BankOrderIssueType.Name
             };
    }
    #endregion

  }
}