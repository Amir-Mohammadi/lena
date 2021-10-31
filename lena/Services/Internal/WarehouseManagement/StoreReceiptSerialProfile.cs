using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.WarehouseManagement.SerialProfile;
using lena.Models.WarehouseManagement.StoreReceipt;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public StoreReceiptSerialProfile AddStoreReceiptSerialProfile(
        StoreReceiptSerialProfile storeReceiptSerialProfile,
        int stuffId,
        int storeReceiptId,
        int cooperatorId)
    {

      storeReceiptSerialProfile = storeReceiptSerialProfile ?? repository.Create<StoreReceiptSerialProfile>();
      var storeReceipt = GetStoreReceipt(storeReceiptId);
      storeReceiptSerialProfile.StoreReceipt = storeReceipt;
      AddSerialProfile(
                    serialProfile: storeReceiptSerialProfile,
                    stuffId: stuffId,
                    cooperatorId: cooperatorId);
      return storeReceiptSerialProfile;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetStoreReceiptSerialProfiles<TResult>(
        Expression<Func<StoreReceiptSerialProfile, TResult>> selector,
        TValue<int> code = null,
        TValue<int> stuffId = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<DateTime> dateTime = null,
        TValue<int> storeReceiptId = null)
    {

      var baseQuery = GetSerialProfiles(
                    selector: e => e,
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    code: code,
                    dateTime: dateTime);
      var query = baseQuery.OfType<StoreReceiptSerialProfile>();
      if (storeReceiptId != null)
        query = query.Where(i => i.StoreReceipt.Id == storeReceiptId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    internal StoreReceiptSerialProfile GetStoreReceiptSerialProfile(int code, int stuffId) => GetStoreReceiptSerialProfile(
        selector: e => e,
        code: code,
        stuffId: stuffId);
    internal TResult GetStoreReceiptSerialProfile<TResult>(
        Expression<Func<StoreReceiptSerialProfile, TResult>> selector,
        int code,
        int stuffId)
    {


      var serialProfile = GetStoreReceiptSerialProfiles(
                    selector: selector,
                    code: code,
                    stuffId: stuffId)


                .SingleOrDefault();
      if (serialProfile == null)
        throw new StoreReceiptSerialProfileNotFoundException(code: code, stuffId: stuffId);
      return serialProfile;
    }

    internal TResult GetStoreReceiptSerialProfile<TResult>(
        Expression<Func<StoreReceiptSerialProfile, TResult>> selector,
        int storeReceiptId)
    {

      var serialProfile = GetStoreReceiptSerialProfiles(
                    selector: selector,
                    storeReceiptId: storeReceiptId)


                .SingleOrDefault();
      if (serialProfile == null)
        throw new StoreReceiptSerialProfileNotFoundException(storeReceiptId: storeReceiptId);
      return serialProfile;
    }
    #endregion
    #region ToSerialsInfoResult
    public Expression<Func<StoreReceiptSerialProfile, StoreReceiptSerialsInfoResult>> ToStoreReceiptSerialsInfoResult =
            storeReceiptSerialProfile => new StoreReceiptSerialsInfoResult()
            {
              StoreReceiptId = storeReceiptSerialProfile.StoreReceipt.Id,
              SerialProfileCode = storeReceiptSerialProfile.Code,
              StuffId = storeReceiptSerialProfile.StuffId,
              MinSerial = storeReceiptSerialProfile.StuffSerials.FirstOrDefault(i => i.Code == storeReceiptSerialProfile.StuffSerials.Min(x => x.Code)).Serial,
              MaxSerial = storeReceiptSerialProfile.StuffSerials.FirstOrDefault(i => i.Code == storeReceiptSerialProfile.StuffSerials.Max(x => x.Code)).Serial,
            };
    #endregion
  }
}
