using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ApplicationSettings.UserSetting
{
  public class UserSettingResult
  {
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public UserSettingValueType ValueType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
