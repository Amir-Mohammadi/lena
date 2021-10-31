using core.Authentication;
using core.Messaging;
using Microsoft.Extensions.Primitives;
namespace core.Setting
{
  public class SiteSetting
  {
    public StringValues CacheControl { get; set; }
    public string AllowSpecificOrigins { get; set; }
    public string WithOrigins { get; set; }
    public string RSAKey { get; set; }
    public string AESKey { get; set; }
    public TokenSetting RoundTokenSetting { get; set; }
    public TokenSetting TokenSetting { get; set; }
    public TokenSetting AuthTokenSetting { get; set; }
    public int VerificationCodeExpireMinutes { get; set; }
  }
}