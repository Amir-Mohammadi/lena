using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseStep;
using lena.Models.Supplies.StuffBasePriceCustoms;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public StuffBasePriceCustoms AddStuffBasePriceCustoms(
        double price,
        byte currencyId,
        short? howToBuyId,
        double? howToBuyRatio,
        double? percent,
        double? tariff,
        double? weight,
        StuffBasePriceCustomsType type,
        StuffBasePrice stuffBasePrice
        )
    {

      var customs = repository.Create<StuffBasePriceCustoms>();
      customs.Price = price;
      customs.CurrencyId = currencyId;
      customs.HowToBuyId = howToBuyId;
      customs.HowToBuyRatio = howToBuyRatio;
      customs.Percent = percent;
      customs.Tariff = tariff;
      customs.Type = type;
      customs.Weight = weight;
      customs.StuffBasePrice = stuffBasePrice;
      repository.Add(customs);
      return customs;
    }

    #endregion
    #region Get
    public StuffBasePriceCustoms GetStuffBasePriceCustoms(int id) => GetStuffBasePriceCustom(selector: e => e, id: id);
    public TResult GetStuffBasePriceCustom<TResult>(
        Expression<Func<StuffBasePriceCustoms, TResult>> selector,
            int id
        )
    {

      var purchaseStep = GetStuffBasePriceCustoms(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseStep == null)
        throw new PurchaseStepNotFoundException(id);
      return purchaseStep;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetStuffBasePriceCustoms<TResult>(
        Expression<Func<StuffBasePriceCustoms, TResult>> selector,
        TValue<double> id = null,
        TValue<double> price = null,
        TValue<int> howToBuyId = null,
        TValue<double> howToBuyRatio = null,
        TValue<double> percent = null,
        TValue<double> tariff = null,
        TValue<StuffBasePriceCustomsType> type = null
        )
    {

      var query = repository.GetQuery<StuffBasePriceCustoms>();
      if (price != null)
        query = query.Where(i => i.Id == id);
      if (price != null)
        query = query.Where(i => i.Price == price);
      if (howToBuyId != null)
        query = query.Where(i => i.HowToBuyId == howToBuyId);
      if (howToBuyRatio != null)
        query = query.Where(i => i.HowToBuyRatio == howToBuyRatio);
      if (percent != null)
        query = query.Where(i => i.Percent == percent);
      if (tariff != null)
        query = query.Where(i => i.Tariff == tariff);
      if (type != null)
        query = query.Where(i => i.Type == type);

      return query.Select(selector);
    }

    #endregion

    #region ToResult

    public Expression<Func<StuffBasePriceCustoms, StuffBasePriceCustomsResult>> ToStuffBasePriceCustomsResult =
        stuffBasePriceCustoms =>
            new StuffBasePriceCustomsResult()
            {
              Price = stuffBasePriceCustoms.Price,
              Id = stuffBasePriceCustoms.Id,
              Type = stuffBasePriceCustoms.Type,
              Percent = stuffBasePriceCustoms.Percent,
              HowToBuyTitle = stuffBasePriceCustoms.HowToBuy.Title,
              RowVersion = stuffBasePriceCustoms.RowVersion
            };

    #endregion

  }
}
