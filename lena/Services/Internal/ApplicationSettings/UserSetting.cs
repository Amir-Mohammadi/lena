using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;

using lena.Services.Internals.Common.Exception;
using lena.Services.Internals.Exceptions;
//using Parlar.DAL;
//using Parlar.DAL.UnitOfWorks;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.ApplicationSettings.UserSetting;
using lena.Models.Common;
using lena.Models.UserManagement.User;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplicationSettings
{
  public partial class ApplicationSetting
  {
    #region Get
    public UserSetting GetUserSetting(string key) => GetUserSetting(selector: e => e, key: key);
    public TResult GetUserSetting<TResult>(
            Expression<Func<UserSetting, TResult>> selector,
            string key
            )
    {

      var Setting = GetUserSettings(
                selector: selector,
                key: key)


                .FirstOrDefault();
      var userId = App.Providers.Security.CurrentLoginData.UserId;
      if (Setting == null)
        throw new UserSettingNotFoundException(key: key, userId: userId);
      return Setting;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetUserSettings<TResult>(
        Expression<Func<UserSetting, TResult>> selector,
        TValue<int> id = null,
        TValue<string> key = null,
        TValue<UserSettingValueType> valueType = null,
        TValue<string> value = null)
    {

      var query = repository.GetQuery<UserSetting>();
      var currenLoginData = App.Providers.Security.CurrentLoginData;
      var userId = currenLoginData.UserId;
      query = query.Where(i => i.UserId == userId);
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (key != null)
        query = query.Where(i => i.Key == key);
      if (valueType != null)
        query = query.Where(i => i.ValueType == valueType);
      if (value != null)
        query = query.Where(i => i.Value == value);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public UserSetting AddUserSetting(
        string key,
        UserSettingValueType valueType,
        string value)
    {

      var setting = repository.Create<UserSetting>();
      setting.UserId = App.Providers.Security.CurrentLoginData.UserId;
      setting.Key = key;
      setting.ValueType = valueType;
      setting.Value = value;
      repository.Add(setting);
      return setting;
    }
    #endregion
    #region Edit
    public UserSetting EditUserSetting(
        int id,
        byte[] rowVersion,
        TValue<string> key = null,
        TValue<UserSettingValueType> valueType = null,
        TValue<string> value = null
         )
    {

      var setting = GetUserSetting(key: key);
      setting.Id = id;
      if (key != null)
        setting.Key = key;
      if (valueType != null)
        setting.ValueType = valueType;
      if (value != null)
        setting.Value = value;
      repository.Update(setting, rowVersion: rowVersion);
      return setting;
    }
    #endregion
    #region Delete
    public UserSetting DeleteUserSetting(
        int id
    )
    {

      var setting = GetUserSettings(
                selector: e => e,
                id: id)


                .FirstOrDefault();
      repository.Delete(setting);
      return setting;
    }
    #endregion
    #region Save
    public UserSetting SaveUserSetting(
        string key,
        UserSettingValueType valueType,
        string value)
    {



      var userSetting = GetUserSettings(
                selector: e => e,
                key: key)


            .FirstOrDefault();

      if (userSetting == null)
        userSetting = AddUserSetting(
                  key: key,
                  valueType: valueType,
                  value: value);
      else
      {
        userSetting = EditUserSetting(
                  id: userSetting.Id,
                  rowVersion: userSetting.RowVersion,
                  key: key,
                  valueType: valueType,
                  value: value);
      }
      return userSetting;
    }
    #endregion
    #region ToResult
    public Expression<Func<UserSetting, UserSettingResult>> ToSettingResult =
        setting => new UserSettingResult
        {
          Id = setting.Id,
          Key = setting.Key,
          ValueType = setting.ValueType,
          Value = setting.Value,
          RowVersion = setting.RowVersion
        };

    #endregion

    #region CheckClientDateTime
    public void CheckClientDateTime(CheckClientTimeInfoInput input)
    {

      var serverTime = DateTime.Now.ToUniversalTime();
      var serverOffset = TimeZoneInfo.Local.BaseUtcOffset.TotalMinutes;
      var timeDiff = Math.Abs((serverTime - input.DateTime).TotalMinutes);
      var timeOffsetDiff = Math.Abs(serverOffset - input.TimeZoneOffsetInMinutes);
      var tolerance = App.Internals.ApplicationSetting.GetTimeZoneOffsetTolerance();

      // اگر منطقه زمانی سرور و کلاینت، بیشتر از مقدار مشخصی اختلاف داشتند
      if (timeOffsetDiff > tolerance)
        throw new ClientTimeZoneIsNotMatchWithServerException(serverTimeZone: TimeZoneInfo.Local.DisplayName);

      // اگر ساعت سرور و کلاینت، بیشتر از مقدار مشخصی اختلاف داشتند
      if (timeDiff > tolerance)
        throw new ClientDateTimeIsNotMatchWithServerException(serverTime: serverTime);
    }
    #endregion
  }
}
