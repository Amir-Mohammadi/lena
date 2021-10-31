using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplicationSettings.Exception;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.ApplicationSettings.AppliicationSetting;
//using lena.Services.Common.Helpers;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplicationSettings
{
  public partial class ApplicationSetting
  {
    private static List<lena.Domains.ApplicationSetting> ApplicationSettings;
    /// <summary>
    /// Add setting if it is not exist in database
    /// </summary>
    /// <param name="settingKey"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public void AddApplicationSetting(SettingKey settingKey, string value)
    {

      var applicationSetting = repository.GetQuery<lena.Domains.ApplicationSetting>()
            .FirstOrDefault(i => i.SettingKey == settingKey);
      if (applicationSetting == null)
      {
        var entity = repository.Create<lena.Domains.ApplicationSetting>();
        entity.SettingKey = settingKey;
        entity.SettingKeyName = settingKey.ToString();
        entity.Value = value;
        ApplicationSettings = null;
        repository.Add(entity);
      }
    }
    public void EditApplicationSetting(SettingKey settingKey, string value)
    {

      var applicationSetting = repository.GetQuery<lena.Domains.ApplicationSetting>()
            .FirstOrDefault(i => i.SettingKey == settingKey);
      applicationSetting.SettingKey = settingKey;
      applicationSetting.SettingKeyName = settingKey.ToString();
      applicationSetting.Value = value;
      ApplicationSettings = null;
      repository.Update(applicationSetting, applicationSetting.RowVersion);
    }
    public string GetValue(SettingKey settingKey)
    {
      if (ApplicationSettings == null)
        ApplicationSettings = this.GetApplicationSettings(selector: e => e).ToList();
      var applicationSetting = ApplicationSettings.FirstOrDefault(i => i.SettingKey == settingKey);
      if (applicationSetting == null)
        throw new ApplicationSettingsNotFoundException(settingKey);
      return applicationSetting.Value;
    }
    public int GetUserMaxFailedLoginCount()
    {
      var result = GetValue(SettingKey.UserMaxFailedLoginCount);
      return Convert.ToInt32(result);
    }
    public double GetMinimumSerialBufferAmount()
    {
      var result = GetValue(SettingKey.MinimumSerialBufferAmount);
      return Convert.ToDouble(result);
    }
    public int GetUserDefaultLockOutTime()
    {
      var result = GetValue(SettingKey.UserMaxFailedLoginCount);
      return Convert.ToInt32(result);
    }
    public string GetHashedversion(SettingKey settingKey)
    {
      var applicationSetting = ApplicationSettings.FirstOrDefault(i => i.SettingKey == settingKey);
      if (applicationSetting == null)
        return string.Empty;
      return applicationSetting.Value;
    }
    public int GetNumberOfPricesForAveraging()
    {
      var result = GetValue(SettingKey.NumberOfPricesForAveraging);
      return Convert.ToInt32(result);
    }
    public string GetBarcodeOnColumnReportName()
    {
      var result = GetValue(SettingKey.BarcodeOneColumnReport);
      return result;
    }
    public string GetBarcodeThreeColumnReportName()
    {
      var result = GetValue(SettingKey.BarcodeThreeColumnReport);
      return result;
    }
    public bool GetApplicationLoggerStatus()
    {
      var result = Convert.ToBoolean(GetValue(SettingKey.ApplicationLogEnabled));
      return result;
    }
    public int GetCompanyId()
    {
      var result = GetValue(SettingKey.CompanyId);
      return Convert.ToInt32(result);
    }
    public int GetMaxPublishedBomCount()
    {
      var result = GetValue(SettingKey.MaxPublishedBomCount);
      return Convert.ToInt32(result);
    }
    public int PasswordValidity()
    {
      var result = GetValue(SettingKey.PasswordValidity);
      return Convert.ToInt32(result);
    }
    public int WarehouseIssueConfirmDeadline()
    {
      var result = GetValue(SettingKey.WarehouseIssueConfirmDeadline);
      return Convert.ToInt32(result);
    }
    public int GetSerialBatchCount()
    {
      var result = GetValue(SettingKey.SerialBatchCount);
      return Convert.ToInt32(result);
    }
    public string GetBarcodeFooterText()
    {
      var result = GetValue(SettingKey.BarcodeLabelFooterText);
      var regex = Regex.Match(result, ".*(?'Date'{\\s*(D|d)ate\\s*:\\s*\"(?'Format'.*)\"\\s*}).*");
      if (regex.Success && regex.Groups.Count != 0)
      {
        var format = regex.Groups["Format"].Value;
        var dateSection = regex.Groups["Date"].Value;
        var date = "";
        if (!string.IsNullOrEmpty(format))
          date = DateTime.Now.Format(new CultureInfo("fa-IR"), format);
        result = result.Replace(dateSection, date);
      }
      return result;
    }
    public bool GetCkeckProductionPlanningWhenDeactivateBom()
    {
      var settingValue = GetValue(SettingKey.CheckProductionPlanningWhenDeactivateBom);
      bool result = false;
      bool.TryParse(settingValue, out result);
      return result;
    }
    public int GetNormalBoardTime()
    {
      var result = GetValue(SettingKey.NormalBoardTime);
      return Convert.ToInt32(result);
    }
    public int GetThresholdDateValue()
    {
      var result = GetValue(SettingKey.ThresholdDate);
      return Convert.ToInt32(result);
    }
    public int GetTerminatedBankOrderValue()
    {
      var result = GetValue(SettingKey.TerminatedBanOrder);
      return Convert.ToInt32(result);
    }
    public int GetAllMembersGroupValue()
    {
      var result = GetValue(SettingKey.AllMembersGroup);
      return Convert.ToInt32(result);
    }
    public int GetStoreReceiptDaysAfterInboundCargoValue()
    {
      var result = GetValue(SettingKey.StoreReceiptDaysAfterInboundCargo);
      return Convert.ToInt32(result);
    }
    public int GetTimeZoneOffsetTolerance()
    {
      var result = GetValue(SettingKey.TimeZoneOffsetTolerance);
      return Convert.ToInt32(result);
    }
    public int GetPurchaseRequestEssentialDateTimeValue()
    {
      var result = GetValue(SettingKey.PurchaseRequestEssentialDateTime);
      return Convert.ToInt32(result);
    }
    public int GetMaximumReprintAmount()
    {
      var result = GetValue(SettingKey.MaximumReprintAmount);
      return Convert.ToInt32(result);
    }
    #region Get 
    public TResult GetApplicationSetting<TResult>(
        Expression<Func<lena.Domains.ApplicationSetting, TResult>> selector,
        string key
        )
    {

      var Setting = GetApplicationSettings(
                selector: selector,
                key: key)


                .FirstOrDefault();
      if (Setting == null)
        throw new ApplicationSettingsNotFoundException(key: key);
      return Setting;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetApplicationSettings<TResult>(
       Expression<Func<lena.Domains.ApplicationSetting, TResult>> selector,
       TValue<string> key = null)
    {

      var query = repository.GetQuery<lena.Domains.ApplicationSetting>();
      if (key != null)
        query = query.Where(i => i.SettingKeyName == key.Value);
      return query.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<lena.Domains.ApplicationSetting, ApplicationSettingResult>> ToApplicationSettingResult =
        setting => new ApplicationSettingResult
        {
          Value = setting.Value
        };
    #endregion
  }
}