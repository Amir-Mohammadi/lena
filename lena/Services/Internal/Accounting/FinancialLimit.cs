using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting.FinancialLimit;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    public FinancialLimit AddFinancialLimit(
        FinancialLimit financialLimit,
        int allowance,
        byte currencyId,
        bool isArchive)
    {
      financialLimit = financialLimit ?? repository.Create<FinancialLimit>();
      financialLimit.Allowance = allowance;
      financialLimit.CurrencyId = currencyId;
      financialLimit.IsArchive = isArchive;
      financialLimit.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(financialLimit);
      return financialLimit;
    }
    public FinancialLimit AddFinancialLimitProcess(
        FinancialLimit financialLimit,
        int allowance,
        byte currencyId,
        bool isArchive)
    {
      var getFinancialLimitsResult = GetFinancialLimits(
                selector: ToFinancialLimitResult,
                currencyId: currencyId,
                isArchive: false);
      if (getFinancialLimitsResult.Any())
      {
        foreach (var item in getFinancialLimitsResult)
        {
          EditFinancialLimit(id: item.Id, rowVersion: item.RowVersion, isArchive: true);
        }
      }
      return AddFinancialLimit(
                financialLimit: null,
                allowance: allowance,
                currencyId: currencyId,
                isArchive: false);
    }
    #endregion
    #region Edit
    public FinancialLimit EditFinancialLimit(
        int id,
        byte[] rowVersion,
        TValue<int> allowance = null,
        TValue<byte> currencyId = null,
        TValue<bool> isArchive = null)
    {
      var financialLimit = GetFinancialLimit(id: id);
      return EditFinancialLimit(
                    financialLimit: financialLimit,
                    rowVersion: rowVersion,
                    allowance: allowance,
                    currencyId: currencyId,
                    isArchive: isArchive);
    }
    public FinancialLimit EditFinancialLimit(
        FinancialLimit financialLimit,
        byte[] rowVersion,
        TValue<int> allowance = null,
        TValue<byte> currencyId = null,
        TValue<bool> isArchive = null)
    {
      if (allowance != null)
        financialLimit.Allowance = allowance;
      if (currencyId != null)
        financialLimit.CurrencyId = currencyId;
      if (isArchive != null)
        financialLimit.IsArchive = isArchive;
      repository.Update(financialLimit, rowVersion);
      return financialLimit;
    }
    #endregion
    #region Get
    public FinancialLimit GetFinancialLimit(int id) => GetFinancialLimit(selector: e => e, id: id);
    public TResult GetFinancialLimit<TResult>(
        Expression<Func<FinancialLimit, TResult>> selector,
        int id)
    {
      var financialLimit = GetFinancialLimits(selector: selector,
                id: id).FirstOrDefault();
      if (financialLimit == null)
        throw new RecordNotFoundException(id, typeof(FinancialLimit));
      return financialLimit;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinancialLimits<TResult>(
        Expression<Func<FinancialLimit, TResult>> selector,
        TValue<int> id = null,
        TValue<int> allowance = null,
        TValue<int> currencyId = null,
        TValue<bool> isArchive = null,
        TValue<int> userId = null)
    {
      var baseEntities = repository.GetQuery<FinancialLimit>();
      if (id != null)
        baseEntities = baseEntities.Where(i => i.Id == id);
      if (allowance != null)
        baseEntities = baseEntities.Where(i => i.Allowance == allowance);
      if (currencyId != null)
        baseEntities = baseEntities.Where(i => i.CurrencyId == currencyId);
      if (isArchive != null)
        baseEntities = baseEntities.Where(i => i.IsArchive == isArchive);
      if (userId != null)
        baseEntities = baseEntities.Where(i => i.UserId == userId);
      return baseEntities.Select(selector);
    }
    #endregion
    #region Search
    public IQueryable<FinancialLimitResult> SearchFinancialLimitResult(
         IQueryable<FinancialLimitResult> query,
         string searchText, AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (searchText != null)
        query = query.Where(i => i.CurrencyName.Contains(searchText) ||
        i.EmployeeCode.Contains(searchText) ||
        i.EmployeeFullName.Contains(searchText) ||
        i.Allowance.ToString().Contains(searchText));
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<FinancialLimitResult> SortFinancialLimitResult(
        IQueryable<FinancialLimitResult> query,
        SortInput<FinancialLimitSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case FinancialLimitSortType.Allowance:
          return query.OrderBy(i => i.Allowance, sortInput.SortOrder);
        case FinancialLimitSortType.CurrencyId:
          return query.OrderBy(i => i.CurrencyId, sortInput.SortOrder);
        case FinancialLimitSortType.CurrencyName:
          return query.OrderBy(i => i.CurrencyName, sortInput.SortOrder);
        case FinancialLimitSortType.EmployeeCode:
          return query.OrderBy(i => i.EmployeeCode, sortInput.SortOrder);
        case FinancialLimitSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case FinancialLimitSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case FinancialLimitSortType.IsArchive:
          return query.OrderBy(i => i.IsArchive, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<FinancialLimit, FinancialLimitResult>> ToFinancialLimitResult =
        financialLimit => new FinancialLimitResult
        {
          Id = financialLimit.Id,
          Allowance = financialLimit.Allowance,
          CurrencyId = financialLimit.Currency.Id,
          CurrencyName = financialLimit.Currency.Title,
          EmployeeCode = financialLimit.User.Employee.Code,
          EmployeeFullName = financialLimit.User.Employee.FirstName + " " + financialLimit.User.Employee.LastName,
          IsArchive = financialLimit.IsArchive,
          RowVersion = financialLimit.RowVersion
        };
    #endregion
  }
}