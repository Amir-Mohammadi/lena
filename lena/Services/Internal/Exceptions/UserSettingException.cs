

using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region UserSetting Exceptions

  public class UserSettingNotFoundException : InternalServiceException
  {
    public string Key { get; }
    public int UserId { get; }

    public UserSettingNotFoundException(int id)
    {
      Id = id;
    }

    public UserSettingNotFoundException(string key, int userId)
    {
      this.Key = key;
      this.UserId = userId;
    }

    public int Id { get; }
  }

  public class UserSettingCodeExsits : InternalServiceException
  {
    public UserSettingCodeExsits(string code)
    {
      this.Code = code;
    }

    public string Code { get; }
  }

  #endregion
}
