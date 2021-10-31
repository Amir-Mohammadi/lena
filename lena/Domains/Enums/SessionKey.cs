using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum SessionKey : byte
  {
    UserCredentials,
    TerminalSecretKey,
    TerminalDeviceCode,
    LastActivity,
    SecurityStamp

  }
}
