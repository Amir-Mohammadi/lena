using lena.Domains.Enums;
namespace lena.Models.Users
{
  public enum TokenType : byte
  {
    Verify = 0,
    Register = 1,
    Auth = 2,
    Sharing = 3,
    DeviceUserToken = 4
  }
}