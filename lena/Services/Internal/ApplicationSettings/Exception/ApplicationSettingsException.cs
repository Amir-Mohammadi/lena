

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplicationSettings.Exception
{
  #region Application Settings Exceptions
  public class ApplicationSettingsNotFoundException : InternalServiceException
  {
    public lena.Domains.Enums.SettingKey Key { get; }
    public string KeyName { get; }
    public ApplicationSettingsNotFoundException(lena.Domains.Enums.SettingKey key)
    {
      Key = key;
    }

    public ApplicationSettingsNotFoundException(string key)
    {
      key = KeyName;
    }
  }


  #endregion
}
