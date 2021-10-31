using System;
using System.Linq;
using System.Linq.Expressions;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public ProductionSerialProfile AddProductionSerialProfile(
        ProductionSerialProfile productionSerialProfile,
        int stuffId,
        int cooperatorId)
    {

      productionSerialProfile = productionSerialProfile ?? repository.Create<ProductionSerialProfile>();

      AddSerialProfile(
                    serialProfile: productionSerialProfile,
                    stuffId: stuffId,
                    cooperatorId: cooperatorId);
      return productionSerialProfile;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetProductionSerialProfiles<TResult>(
        Expression<Func<ProductionSerialProfile, TResult>> selector,
        TValue<int> code = null,
        TValue<int> stuffId = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<DateTime> dateTime = null)
    {

      var baseQuery = GetSerialProfiles(
                    selector: e => e,
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    code: code,
                    dateTime: dateTime);
      var query = baseQuery.OfType<ProductionSerialProfile>();
      return query.Select(selector);
    }
    #endregion
    #region Get
    internal ProductionSerialProfile GetProductionSerialProfile(int code, int stuffId) => GetProductionSerialProfile(
        selector: e => e,
        code: code,
        stuffId: stuffId);
    internal TResult GetProductionSerialProfile<TResult>(
        Expression<Func<ProductionSerialProfile, TResult>> selector,
        int code,
        int stuffId)
    {

      var serialProfile = GetProductionSerialProfiles(
                    selector: selector,
                    code: code,
                    stuffId: stuffId)


                .SingleOrDefault();
      if (serialProfile == null)
        throw new ProductionSerialProfileNotFoundException(code: code, stuffId: stuffId);
      return serialProfile;
    }
    #endregion
  }
}
