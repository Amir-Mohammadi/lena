using Base = System;
using lena.Services.Core.Foundation;
using lena.Services.Core.Foundation.Service;
using lena.Services.Core.Provider.Notification;
using lena.Services.Core.Provider.Security;
using lena.Services.Core.Provider.Session;
using System;
using System.IO;
using System.Reflection;
using lena.Services.Common.Helpers;
using System.Linq;
//using Parlar.DAL;
using lena.Models.UserManagement.SecurityAction;
using lena.Domains.Enums;
using lena.Services.Core.Provider.Logger;
using lena.Services.Core.Common;
using lena.Services.Core.Provider.DependencyProvider;
using lena.Services.Core.Provider.Dependency;
using lena.Services.Core.Provider.RequestInfo;
namespace lena.Services.Core
{
  public static class App
  {
    #region version
    public static string BuildDate()
    {
      var filePath = Base.Reflection.Assembly.GetExecutingAssembly().Location;
      const int c_PeHeaderOffset = 60;
      const int c_LinkerTimestampOffset = 8;
      var buffer = new byte[2048];
      using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        stream.Read(buffer, 0, 2048);
      var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
      var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      var linkTimeUtc = epoch.AddSeconds(secondsSince1970);
      //var tz = TimeZoneInfo.Local;
      //var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);
      return $"{linkTimeUtc:yyyy-MM-dd HH:mm:ss}";
    }
    #endregion
    #region init        
    public static Base.DateTime AppInitializDateTime { get; private set; }
    public static Version AssemblyVersion { get; private set; }
    public static void Start()
    {
      AppInitializDateTime = Base.DateTime.Now.ToUniversalTime();
      AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
      Providers = new ProviderGroup();
      Api = new ExternalServiceGroup();
      Internals = new InternalServiceGroup();
      App.Internals.Notification.Emit(eventKey: SystemEvents.OnApplicationStart, payload: "")

           ;
    }
    public static void RegisterModules(DependencyProvider dependencyProvider)
    {
      ProviderGroup.RegisterModule(dependencyProvider);
      ExternalServiceGroup.RegisterModule(dependencyProvider);
      InternalServiceGroup.RegisterModule(dependencyProvider);
      //App.Internals.ApplicationBase.InitStaticData();
      //Add Api Controller Security Actions
      //var methods = ApiHelper.GetAssemblyApiMethods(Assembly.GetCallingAssembly())
      //    .Select(x => new AddBulkSecurityActionInput()
      //    {
      //        Name = x.Name,
      //        ActionName = x.ActionName,
      //        GroupName = x.GroupName
      //    });
      //App.Internals.UserManagement.AddBulkSecurityActions(methods.ToArray());      
      //    ;
    }
    #endregion
    #region check version
    public static void CheckVesrison(string version)
    {
      var existHashedVersion = App.Internals.ApplicationSetting.GetHashedversion(SettingKey.HashedVersion);
      var hashedVersion = Crypto.Sha1(version);
      if (string.IsNullOrEmpty(existHashedVersion))
        //Add to applicationSetting
        App.Internals.ApplicationSetting.AddApplicationSetting(SettingKey.HashedVersion, hashedVersion);
      if (existHashedVersion != hashedVersion)
      {
        //hased version should change
        App.Internals.ApplicationSetting.EditApplicationSetting(SettingKey.HashedVersion, hashedVersion);
      }
    }
    #endregion
    public static ProviderGroup Providers { get; private set; }
    //TODO fix sssss
    // public static ExternalServiceGroup Api { get; private set; }
    internal static InternalServiceGroup Internals { get; private set; }
  }
}