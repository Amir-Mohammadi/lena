using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.ApplicationBase.Currency;
using lena.Models.Common;
using System;
using System.Linq;
using Currency = lena.Domains.Currency;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    public Currency AddCurrency(
        string title,
        CurrencyType type,
        string code,
        string sign,
        bool isMain,
        byte decimalDigitCount)
    {

      var currency = repository.Create<Currency>();
      currency.Title = title;
      currency.Type = type;
      currency.Code = code;
      currency.Sign = sign;
      currency.IsMain = isMain;
      currency.DecimalDigitCount = decimalDigitCount;
      repository.Add(currency);
      return currency;
    }

    public Currency EditCurrency(
        byte[] rowVersion,
        int id,
        TValue<string> title = null,
        TValue<CurrencyType> type = null,
        TValue<string> code = null,
        TValue<string> sign = null,
        TValue<bool> isMain = null,
        TValue<byte> decimalDigitCount = null)
    {

      var currency = GetCurrency(id);
      if (title != null)
        currency.Title = title;
      if (type != null)
        currency.Type = type;
      if (code != null)
        currency.Code = code;
      if (isMain != null)
        currency.IsMain = isMain;
      if (sign != null)
        currency.Sign = sign;
      if (decimalDigitCount != null)
        currency.DecimalDigitCount = decimalDigitCount;
      repository.Update(entity: currency, rowVersion: currency.RowVersion);
      return currency;
    }
    public void DeleteCurrency(int id)
    {

      repository.Delete<CurrencyRate>(x => x.FromCurrencyId == id || x.ToCurrencyId == id);
      var currency = GetCurrency(id);
      repository.Delete(currency);
    }
    public Currency GetCurrency(int id)
    {

      var currency = GetCurrencys(id: id).FirstOrDefault();
      if (currency == null)
        throw new CurrencyNotFoundException(id);
      return currency;
    }
    public IQueryable<Currency> GetCurrencys(
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<CurrencyType> type = null,
        TValue<string> code = null,
        TValue<string> sign = null,
        TValue<bool> isMain = null)
    {

      var isIdNUll = id == null;
      var isCodeNull = code == null;
      var isTypeNull = type == null;
      var isTitleNull = title == null;
      var isSignNull = sign == null;
      var isIsMainNull = isMain == null;
      var currencys = from currency in repository.GetQuery<Currency>()
                      where (isIdNUll || currency.Id == id) &&
                                  (isCodeNull || currency.Code == code) &&
                                  (isTypeNull || currency.Type == type) &&
                                  (isTitleNull || currency.Title == title) &&
                                  (isIsMainNull || currency.IsMain == isMain) &&
                                  (isSignNull || currency.Sign == sign)
                      select currency;
      return currencys;
    }

    public IQueryable<CurrencyResult> SearchCurrencyResult(
        IQueryable<CurrencyResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Title.Contains(searchText) ||
                      item.Sign.Contains(searchText) ||
                      item.Code.Contains(searchText) ||
                      item.DecimalDigitCount.ToString().Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;

    }

    public IOrderedQueryable<CurrencyResult> SortCurrencyResult(IQueryable<CurrencyResult> query, SortInput<CurrencySortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CurrencySortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case CurrencySortType.Code:
          return query.OrderBy(r => r.Code, sortInput.SortOrder);
        case CurrencySortType.Type:
          return query.OrderBy(r => r.Type, sortInput.SortOrder);
        case CurrencySortType.IsMain:
          return query.OrderBy(r => r.IsMain, sortInput.SortOrder);
        case CurrencySortType.Sign:
          return query.OrderBy(r => r.Sign, sortInput.SortOrder);
        case CurrencySortType.Title:
          return query.OrderBy(r => r.Title, sortInput.SortOrder);
        case CurrencySortType.DecimalDigitCount:
          return query.OrderBy(r => r.DecimalDigitCount, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<CurrencyResult> ToCurrencyResultQuery(IQueryable<Currency> query)
    {
      var result = from currency in query
                   select new CurrencyResult()
                   {
                     Id = currency.Id,
                     Title = currency.Title,
                     Type = currency.Type,
                     Code = currency.Code,
                     Sign = currency.Sign,
                     IsMain = currency.IsMain,
                     DecimalDigitCount = currency.DecimalDigitCount,
                     RowVersion = currency.RowVersion
                   };
      return result;
    }
    public IQueryable<CurrencyComboResult> ToCurrencyComboResult(IQueryable<Currency> query)
    {
      var result = from currency in query
                   select new CurrencyComboResult()
                   {
                     Id = currency.Id,
                     Title = currency.Title,
                     Type = currency.Type,
                     DecimalDigitCount = currency.DecimalDigitCount
                   };
      return result;
    }
    public CurrencyResult ToCurrencyResult(Currency currency)
    {
      var result = new CurrencyResult()
      {
        Id = currency.Id,
        Title = currency.Title,
        Type = currency.Type,
        Code = currency.Code,
        Sign = currency.Sign,
        IsMain = currency.IsMain,
        DecimalDigitCount = currency.DecimalDigitCount,
        RowVersion = currency.RowVersion
      };
      return result;
    }
  }
}
