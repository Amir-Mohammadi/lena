using lena.Services.Core;
using lena.Services.Core.Provider.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals
{
  public class DatabaseLoader : IStorageInitializer
  {
    public void Load(Storage storage)
    {
      storage.CheckTransactionBatchFragmentedSerials = bool.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.CheckTransactionBatchFragmentedSerials));
      storage.CheckTransactionBatchNegativeStuffSerialValues = bool.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.CheckTransactionBatchNegativeStuffSerialValues));
      storage.CheckTransactionBatchNegativeStuffValues = bool.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.CheckTransactionBatchNegativeStuffValues));
      storage.RedisHost = App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.RedisHost);
      storage.RedisPort = int.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.RedisPort));
      storage.TokenTimeout = int.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.TokenTimeout));
      storage.Issuer = App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.Issuer);
      storage.SecretKey = App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.SecretKey);
      storage.Encryptkey = App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.Encryptkey);
      storage.CheckInternalProvider = bool.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.CheckInternalProvider));
      storage.CheckForeignProvider = bool.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.CheckForeignProvider));
      storage.ApplicationLogEnabled = bool.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.ApplicationLogEnabled));

      storage.NotificationHost = App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.NotificationHost);
      storage.NotificationPort = int.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.NotificationPort));

      storage.LoggerHost = App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.LoggerHost);
      storage.LoggerPort = int.Parse(App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.LoggerPort));
      storage.LoggerToken = App.Internals.ApplicationSetting.GetValue(Domains.Enums.SettingKey.LoggerToken);




    }
  }
}
