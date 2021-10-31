using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.ApplicationBase.CurrencyRate;
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
    public CurrencyRate AddCurrencyRate(
        DateTime fromDateTime,
        double commission,
        double rate,
        int exchangeId,
        byte fromCurrencyId,
        byte toCurrencyId)
    {
      var currencyRate = repository.Create<CurrencyRate>();
      currencyRate.FromDateTime = fromDateTime;
      currencyRate.ExchangeId = exchangeId;
      currencyRate.Commission = commission;
      currencyRate.Rate = rate;
      currencyRate.FromCurrencyId = fromCurrencyId;
      currencyRate.ToCurrencyId = toCurrencyId;
      currencyRate.DateTime = DateTime.Now.ToUniversalTime();
      repository.Add(currencyRate);
      return currencyRate;
    }
    public CurrencyRate EditCurrencyRate(
        int id,
        byte[] rowVersion,
        TValue<DateTime> fromDateTime = null,
        TValue<int> exchangeId = null,
        TValue<double> commission = null,
        TValue<double> rate = null,
        TValue<byte> fromCurrencyId = null,
        TValue<byte> toCurrencyId = null)
    {
      var currencyRate = GetCurrencyRate(id);
      if (fromDateTime != null)
        currencyRate.FromDateTime = fromDateTime;
      if (exchangeId != null)
        currencyRate.ExchangeId = exchangeId;
      if (commission != null)
        currencyRate.Commission = commission;
      if (rate != null)
        currencyRate.Rate = rate;
      if (fromCurrencyId != null)
        currencyRate.FromCurrencyId = fromCurrencyId;
      if (toCurrencyId != null)
        currencyRate.ToCurrencyId = toCurrencyId;
      repository.Update(entity: currencyRate, rowVersion: currencyRate.RowVersion);
      return currencyRate;
    }
    public void DeleteCurrencyRate(int id)
    {
      var currencyRate = GetCurrencyRate(id);
      repository.Delete(currencyRate);
    }
    public CurrencyRate GetCurrencyRate(int id)
    {
      var currencyRate = GetCurrencyRates(id: id).FirstOrDefault();
      if (currencyRate == null)
        throw new CurrencyRateNotFoundException(id);
      return currencyRate;
    }
    public IQueryable<CurrencyRate> GetCurrencyRates(
        TValue<int> id = null,
        TValue<int> exchangeId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<double> commission = null,
        TValue<double> rate = null,
        TValue<int> fromCurrencyId = null,
        TValue<int> toCurrencyId = null)
    {
      var isIdNUll = id == null;
      var isExchangeNull = exchangeId == null;
      var isFromDateTimeNull = fromDateTime == null;
      var isFromCurrencyIdNull = fromCurrencyId == null;
      var isCommissionNull = commission == null;
      var isRateNull = rate == null;
      var isToCurrencyIdNull = toCurrencyId == null;
      var CurrencyRates = from currencyRate in repository.GetQuery<CurrencyRate>()
                          where (isIdNUll || currencyRate.Id == id) &&
                                      (isFromDateTimeNull || currencyRate.FromDateTime == fromDateTime) &&
                                      (isExchangeNull || currencyRate.ExchangeId == exchangeId) &&
                                      (isFromCurrencyIdNull || currencyRate.FromCurrencyId == fromCurrencyId) &&
                                      (isCommissionNull || currencyRate.Commission == commission) &&
                                      (isRateNull || currencyRate.Rate == rate) &&
                                      (isToCurrencyIdNull || currencyRate.ToCurrencyId == toCurrencyId)
                          select currencyRate;
      return CurrencyRates;
    }
    public IOrderedQueryable<CurrencyRateResult> SortCurrencyRateResult(IQueryable<CurrencyRateResult> query,
        SortInput<CurrencyRateSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CurrencyRateSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case CurrencyRateSortType.Commission:
          return query.OrderBy(r => r.Commission, sortInput.SortOrder);
        case CurrencyRateSortType.CreationTime:
          return query.OrderBy(r => r.CreationTime, sortInput.SortOrder);
        case CurrencyRateSortType.FromCurrencyCode:
          return query.OrderBy(r => r.FromCurrencyCode, sortInput.SortOrder);
        case CurrencyRateSortType.FromCurrencyTitle:
          return query.OrderBy(r => r.FromCurrencyTitle, sortInput.SortOrder);
        case CurrencyRateSortType.FromCurrencySign:
          return query.OrderBy(r => r.FromCurrencySign, sortInput.SortOrder);
        case CurrencyRateSortType.ToCurrencyCode:
          return query.OrderBy(r => r.FromCurrencyTitle, sortInput.SortOrder);
        case CurrencyRateSortType.ToCurrencySign:
          return query.OrderBy(r => r.FromCurrencyTitle, sortInput.SortOrder);
        case CurrencyRateSortType.ToCurrencyTitle:
          return query.OrderBy(r => r.ToCurrencyTitle, sortInput.SortOrder);
        case CurrencyRateSortType.FromDate:
          return query.OrderBy(r => r.FromDate, sortInput.SortOrder);
        case CurrencyRateSortType.Rate:
          return query.OrderBy(r => r.Rate, sortInput.SortOrder);
        case CurrencyRateSortType.ExchangeName:
          return query.OrderBy(r => r.ExchangeName, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public CurrencyRateResult ToCurrencyRateResult(CurrencyRate currencyRate)
    {
      var result = new CurrencyRateResult()
      {
        Id = currencyRate.Id,
        ExchangeId = currencyRate.ExchangeId,
        ExchangeCode = currencyRate.Exchange.Code,
        ExchangeName = currencyRate.Exchange.Name,
        CreationTime = currencyRate.DateTime,
        FromDate = currencyRate.FromDateTime,
        Rate = currencyRate.Rate,
        FromCurrencyId = currencyRate.FromCurrencyId,
        FromCurrencyTitle = currencyRate.FromCurrency.Title,
        FromCurrencyCode = currencyRate.FromCurrency.Code,
        FromCurrencySign = currencyRate.FromCurrency.Sign,
        ToCurrencyId = currencyRate.ToCurrencyId,
        ToCurrencyTitle = currencyRate.ToCurrency.Title,
        ToCurrencyCode = currencyRate.ToCurrency.Code,
        ToCurrencySign = currencyRate.ToCurrency.Sign,
        Commission = currencyRate.Commission,
        RowVersion = currencyRate.RowVersion
      };
      return result;
    }
    public IQueryable<CurrencyRateResult> ToCurrencyRateResultQuery(IQueryable<CurrencyRate> query)
    {
      var result = from currencyRate in query
                   select new CurrencyRateResult()
                   {
                     Id = currencyRate.Id,
                     ExchangeId = currencyRate.ExchangeId,
                     ExchangeCode = currencyRate.Exchange.Code,
                     ExchangeName = currencyRate.Exchange.Name,
                     CreationTime = currencyRate.DateTime,
                     FromDate = currencyRate.FromDateTime,
                     Rate = currencyRate.Rate,
                     FromCurrencyId = currencyRate.FromCurrencyId,
                     FromCurrencyTitle = currencyRate.FromCurrency.Title,
                     FromCurrencyCode = currencyRate.FromCurrency.Code,
                     FromCurrencySign = currencyRate.FromCurrency.Sign,
                     ToCurrencyId = currencyRate.ToCurrencyId,
                     ToCurrencyTitle = currencyRate.ToCurrency.Title,
                     ToCurrencyCode = currencyRate.ToCurrency.Code,
                     ToCurrencySign = currencyRate.ToCurrency.Sign,
                     Commission = currencyRate.Commission,
                     RowVersion = currencyRate.RowVersion
                   };
      return result;
    }
    public double GetCurrencyRateOnDate(
        int fromCurrencyId,
        int toCurrencyId,
        DateTime dateTime)
    {
      var rates =
                App.Internals.ApplicationBase.GetCurrencyRates(
                        fromCurrencyId: fromCurrencyId,
                        toCurrencyId: toCurrencyId)
                    ;
      var rate = rates.
                Where(u => u.FromDateTime <= dateTime)
                .OrderByDescending(u => u.DateTime)
                .Select(u => u.Rate)
                .FirstOrDefault();
      return rate;
    }
    public Currency GetMainCurrency()
    {
      return App.Internals.ApplicationBase.GetCurrencys(isMain: true)
                .SingleOrDefault();
    }
    public double GetCurrencyRateOnDateByMainCurrency(
        int fromCurrencyId,
        DateTime dateTime)
    {
      var toCurrency = GetMainCurrency();
      if (fromCurrencyId == toCurrency.Id)
      {
        return 1;
      }
      else
      {
        return GetCurrencyRateOnDate(
                      fromCurrencyId: fromCurrencyId,
                      toCurrencyId: toCurrency.Id,
                      dateTime: dateTime);
      }
    }
    public IQueryable<CurrencyRateResult> SearchCurrencyRateResult(
     IQueryable<CurrencyRateResult> query,
     string searchText,
     AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Id.ToString().Contains(searchText) ||
                      item.FromCurrencyTitle.Contains(searchText) ||
                      item.FromCurrencySign.Contains(searchText) ||
                      item.FromCurrencyCode.Contains(searchText) ||
                      item.ToCurrencyTitle.Contains(searchText) ||
                      item.ToCurrencySign.Contains(searchText) ||
                      item.ToCurrencyCode.Contains(searchText) ||
                      item.CreationTime.ToString().Contains(searchText) ||
                      item.Rate.ToString().Contains(searchText) ||
                      item.Commission.ToString().Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IQueryable<CurrencyRate> GetCurrencyRatesOnDate(TValue<int> fromCurrencyId, TValue<int> toCurrencyId, TValue<DateTime> dateTime = null)
    {
      var currencyRates = repository.GetQuery<CurrencyRate>();
      var now = DateTime.Now.ToUniversalTime();
      if (fromCurrencyId != null)
        currencyRates = currencyRates.Where(u => u.FromCurrencyId == fromCurrencyId);
      if (toCurrencyId != null)
        currencyRates = currencyRates.Where(u => u.ToCurrencyId == toCurrencyId);
      if (dateTime != null)
        currencyRates = currencyRates.Where(u => u.FromDateTime <= dateTime);
      if (dateTime == null)
        currencyRates = currencyRates.Where(u => u.FromDateTime <= now);
      var result = currencyRates
                .GroupBy(u => new { fromCurrencyId = u.FromCurrencyId, toCurrencyId = u.ToCurrencyId })
                .Select(u => u.OrderByDescending(k => k.DateTime).FirstOrDefault());
      return result;
    }
    #region Calculate CurrencyRateByGivenRates
    internal double GetCurrencyRate(CurrencyRateValue[] rates, int fromCurrencyId, int toCurrencyId)
    {
      if (fromCurrencyId == toCurrencyId) return 1d;
      var rate = rates.SingleOrDefault(u => u.FromCurrencyId == fromCurrencyId && u.ToCurrencyId == toCurrencyId);
      if (rate == null) throw new NoMatchCurrencyRateFoundException(
          fromCurrencyId: fromCurrencyId,
          toCurrencyId: toCurrencyId);
      return rate?.Rate ?? 0;
    }
    #endregion
  }
}