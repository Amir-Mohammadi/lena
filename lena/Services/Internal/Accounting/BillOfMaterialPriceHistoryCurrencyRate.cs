using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Accounting.BillOfMaterialPriceHistoryCurrencyRates;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    public IQueryable<BillOfMaterialPriceHistoryCurrencyRate> GetBillOfMaterialPriceHistoryCurrencyRates(
       TValue<int> billOfMaterialPriceHistoryId = null,
       TValue<int?> fromCurrencyId = null,
       TValue<int> toCurrencyId = null,
       TValue<double> rate = null
        )
    {

      return GetBillOfMaterialPriceHistoryCurrencyRates(
                e => e,
                billOfMaterialPriceHistoryId: billOfMaterialPriceHistoryId,
                fromCurrencyId: fromCurrencyId,
                toCurrencyId: toCurrencyId,
                rate: rate
                );
    }

    public IQueryable<TResult> GetBillOfMaterialPriceHistoryCurrencyRates<TResult>(
            Expression<Func<BillOfMaterialPriceHistoryCurrencyRate, TResult>> selector,
           TValue<int> billOfMaterialPriceHistoryId = null,
           TValue<int?> fromCurrencyId = null,
           TValue<int> toCurrencyId = null,
           TValue<double> rate = null
            )
    {

      var query = repository.GetQuery<BillOfMaterialPriceHistoryCurrencyRate>();

      if (billOfMaterialPriceHistoryId != null)
        query = query.Where(i => i.BillOfMaterialPriceHistoryId == billOfMaterialPriceHistoryId);
      if (fromCurrencyId != null)
        query = query.Where(i => i.FromCurrencyId == fromCurrencyId);
      if (toCurrencyId != null)
        query = query.Where(i => i.ToCurrencyId == toCurrencyId);
      if (rate != null)
        query = query.Where(i => i.Rate == rate);

      return query.Select(selector);
    }

    public void DeleteBillOfMaterialPriceHistoryCurrencyRate(int id)
    {

      var entity = repository.GetQuery<BillOfMaterialPriceHistoryCurrencyRate>().FirstOrDefault(i => i.Id == id);
      if (entity == null)
        throw new RecordNotFoundException(recordId: id, typeof(BillOfMaterialPriceHistoryCurrencyRate));

      repository.Delete(entity);

    }

    public void AddBillOfMaterialPriceHistoryCurrencyRate(
        int billOfMaterialPriceHistoryId,
        byte fromCurrencyId,
        byte toCurrencyId,
        double rate)
    {

      var entity = repository.Create<BillOfMaterialPriceHistoryCurrencyRate>();
      entity.BillOfMaterialPriceHistoryId = billOfMaterialPriceHistoryId;
      entity.FromCurrencyId = fromCurrencyId;
      entity.ToCurrencyId = toCurrencyId;
      entity.Rate = rate;

      repository.Add(entity);
    }

    public Expression<Func<BillOfMaterialPriceHistoryCurrencyRate, BillOfMaterialPriceHistoryCurrencyRateResult>> ToBillOfMaterialPriceHistoryCurrencyRateResult =
               entity => new BillOfMaterialPriceHistoryCurrencyRateResult
               {
                 Id = entity.Id,
                 BillOfMaterialPriceHistoryId = entity.BillOfMaterialPriceHistoryId,
                 FromCurrencyId = entity.FromCurrencyId,
                 FromCurrencyTitle = entity.FromCurrency.Title,
                 ToCurrencyId = entity.ToCurrencyId,
                 ToCurrencyTitle = entity.ToCurrency.Title,
                 Rate = entity.Rate,
                 RowVersion = entity.RowVersion
               };


  }
}
