using System;
using System.Linq;
using lena.Domains;
using lena.Services.Core;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using lena.Models;
using lena.Models.Common;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Supplies.Exchange;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Delete
    public void DeleteExchange(int id)
    {

      var cooperatorFinancialAccount = App.Internals.Accounting.GetCooperatorFinancialAccounts(
                selector: e => e,
                cooperatorId: id);

      if (cooperatorFinancialAccount.Any())
      {
        throw new CooperatorHasFinancialAcountException(id);
      }
      var exchange = GetExchange(id);
      repository.Delete(exchange);
    }
    #endregion

    #region Add
    public Cooperator AddExchange(string name)
    {

      var exchange = repository.Create<Cooperator>();
      App.Internals.SaleManagement.AddCooperator(
                exchange,
                detailedCode: "",
                name: name,
                cooperatorType: CooperatorType.Exchange);
      return exchange;
    }
    #endregion

    #region Gets
    public IQueryable<Cooperator> GetExchanges(
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> Code = null)
    {

      var cooperators = App.Internals.SaleManagement.GetCooperators(
                selector: e => e,
                id: id,
                name: name,
                Code: Code,
                cooperatorType: CooperatorType.Exchange);

      return cooperators;
    }
    #endregion

    #region Get
    public Cooperator GetExchange(int id)
    {

      var exchange = GetExchanges(id: id).FirstOrDefault();
      if (exchange == null)
        throw new ExchangeNotfoundException(id: id);
      return exchange;
    }
    #endregion

    #region Edit
    public Cooperator EditExchange(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> code = null)
    {

      var exchange = this.GetExchange(id: id);
      App.Internals.SaleManagement.EditCooperator(
                cooperator: exchange,
                rowVersion: rowVersion,
                name: name,
                code: code);
      return exchange;
    }
    #endregion

    #region ToExchangeComboList
    public IQueryable<ExchangeComboResult> ToExchangeComboList(IQueryable<Cooperator> query)
    {
      return from exchange in query
             select new ExchangeComboResult()
             {
               Id = exchange.Id,
               Name = exchange.Name,
               Code = exchange.Code
             };
    }
    #endregion

    #region ToExchangeResultList
    public IQueryable<ExchangeResult> ToExchangeResultList(IQueryable<Cooperator> query)
    {
      return from exchange in query
             select new ExchangeResult()
             {
               Id = exchange.Id,
               Name = exchange.Name,
               Code = exchange.Code,
               RowVersion = exchange.RowVersion
             };
    }
    #endregion

    #region ToExchangeResult
    public ExchangeResult ToExchangeResult(Cooperator exchange)
    {
      return new ExchangeResult()
      {
        Id = exchange.Id,
        Code = exchange.Code,
        Name = exchange.Name,
        RowVersion = exchange.RowVersion
      };
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ExchangeResult> SortExchangeResult(IQueryable<ExchangeResult> input, SortInput<ExchangeSortType> options)
    {
      switch (options.SortType)
      {
        case ExchangeSortType.Id:
          return input.OrderBy(res => res.Id, options.SortOrder);
        case ExchangeSortType.Name:
          return input.OrderBy(res => res.Name, options.SortOrder);
        case ExchangeSortType.Code:
          return input.OrderBy(res => res.Code, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ExchangeResult> SearchExchangeResult(
        IQueryable<ExchangeResult> query,
        string searchText
        , AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where item.Name.Contains(searchText) ||
                      item.Code.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;

    }
    #endregion
  }
}