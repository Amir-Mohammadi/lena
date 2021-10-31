using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Models.Supplies.PurchaseStep;
using lena.Models.Supplies.StuffBasePriceTransport;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public StuffBasePriceTransport AddStuffBasePriceTransport(
        double price,
        double percent,
        StuffBasePriceTransportComputeType computeType,
        StuffBasePriceTransportType type,
        StuffBasePrice stuffBasePrice)
    {

      var transport = repository.Create<StuffBasePriceTransport>();
      transport.Price = price;
      transport.Percent = percent;
      transport.ComputeType = computeType;
      transport.Type = type;
      transport.StuffBasePrice = stuffBasePrice;
      repository.Add(transport);
      return transport;
    }

    #endregion
    #region Get
    public StuffBasePriceTransport GetStuffBasePriceTransport(int id) => GetStuffBasePriceTransport(selector: e => e, id: id);
    public TResult GetStuffBasePriceTransport<TResult>(
        Expression<Func<StuffBasePriceTransport, TResult>> selector,
            int id
        )
    {

      var purchaseStep = GetStuffBasePriceTransports(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseStep == null)
        throw new StuffBasePriceTransportNotFoundException(id);
      return purchaseStep;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetStuffBasePriceTransports<TResult>(
        Expression<Func<StuffBasePriceTransport, TResult>> selector,
        TValue<int> id = null,
        TValue<double> price = null,
        TValue<double> percent = null,
        TValue<StuffBasePriceTransportComputeType> computeType = null,
        TValue<StuffBasePriceTransportType> type = null
        )
    {

      var query = repository.GetQuery<StuffBasePriceTransport>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (price != null)
        query = query.Where(i => i.Price == price);
      if (percent != null)
        query = query.Where(i => i.Percent == percent);
      if (computeType != null)
        query = query.Where(i => i.ComputeType == computeType);
      if (percent != null)
        query = query.Where(i => i.Percent == percent);
      if (type != null)
        query = query.Where(i => i.Type == type);
      return query.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<StuffBasePriceTransport, StuffBasePriceTransportResult>> ToStuffBasePriceTransportResult =
        stuffBasePriceTransport =>
        new StuffBasePriceTransportResult
        {
          Type = stuffBasePriceTransport.Type,
          Id = stuffBasePriceTransport.Id,
          Percent = stuffBasePriceTransport.Percent,
          ComputeType = stuffBasePriceTransport.ComputeType,
          RowVersion = stuffBasePriceTransport.RowVersion
        };
    #endregion

  }
}
