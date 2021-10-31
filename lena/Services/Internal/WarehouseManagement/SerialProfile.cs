using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
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
    public SerialProfile AddSerialProfile(
        SerialProfile serialProfile,
        int stuffId,
        int cooperatorId)
    {

      var maxCode = GetMaxSerialProfileCode(stuffId);
      maxCode++;
      serialProfile = serialProfile ?? repository.Create<SerialProfile>();
      serialProfile.StuffId = stuffId;
      serialProfile.Code = maxCode;
      serialProfile.CooperatorId = cooperatorId;
      serialProfile.DateTime = DateTime.Now.ToUniversalTime();
      serialProfile.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(serialProfile);
      return serialProfile;
    }
    #endregion
    #region GetMaxCode
    internal int GetMaxSerialProfileCode(int stuffId)
    {

      var query = GetSerialProfiles(
                e => e.Code,
                stuffId: stuffId);
      var maxCode = query.Any() ? query.Max(i => i) : 0;
      return maxCode;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetSerialProfiles<TResult>(
        Expression<Func<SerialProfile, TResult>> selector,
        TValue<int> code = null,
        TValue<int> stuffId = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<DateTime> dateTime = null)
    {

      var query = repository.GetQuery<SerialProfile>();
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      return query.Select(selector);
    }
    #endregion
    #region Get
    internal SerialProfile GetSerialProfile(int code, int stuffId) => GetSerialProfile(
        selector: e => e,
        code: code,
        stuffId: stuffId);
    internal TResult GetSerialProfile<TResult>(
        Expression<Func<SerialProfile, TResult>> selector,
        int code,
        int stuffId)
    {

      var serialProfile = GetSerialProfiles(
                    selector: selector,
                    code: code,
                    stuffId: stuffId)


                .SingleOrDefault();
      if (serialProfile == null)
        throw new SerialProfileNotFoundException(code: code, stuffId: stuffId);
      return serialProfile;
    }
    #endregion


    //#region Edit

    //internal SerialProfile EditSerialProfile(byte[] rowVersion, int id, TValue<int> stuffId = null, TValue<int> unitId = null)
    //{
    //    
    //        var serialProfile = GetSerialProfile(code: id);

    //        if (stuffId != null)
    //            serialProfile.StuffId = stuffId;

    //        repository.Update(serialProfile, rowVersion);

    //        return serialProfile;
    //    });


    //}

    //#endregion

    //#region Sort

    //internal IOrderedQueryable<SerialProfileResult> SortSerialProfile(IQueryable<SerialProfileResult> input,
    //    SortInput<SerialProfileSortType> options)
    //{
    //    switch (options.SortType)
    //    {
    //        case SerialProfileSortType.Code:
    //            return input.OrderBy(a => a.Code, options.SortOrder);
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}

    //#endregion
    //internal IOrderedQueryable<SerialProfileResult> SortSerialProfileResult(IQueryable<SerialProfileResult> input,
    //    SortInput<SerialProfileSortType> options)
    //{
    //    switch (options.SortType)
    //    {
    //        case SerialProfileSortType.Code:
    //            return input.OrderBy(i => i.Code, options.SortOrder);
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}
    //internal SerialProfileResult ToSerialProfileResult(SerialProfile data)
    //{
    //    var result = new SerialProfileResult()
    //    {
    //        Code = data.Code,
    //        StuffId = data.StuffId,
    //        RowVersion = data.RowVersion

    //    };
    //    return result;
    //}

    //internal IQueryable<SerialProfileResult> ToSerialProfileResultQuery(IQueryable<SerialProfile> data)
    //{
    //    return (from d in data
    //            select new SerialProfileResult
    //            {
    //                Code = d.Code,
    //                StuffId = d.StuffId,
    //                RowVersion = d.RowVersion
    //            });
    //}
  }
}
