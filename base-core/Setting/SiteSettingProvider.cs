using Microsoft.Extensions.Configuration;
namespace core.Setting
{
  public class SiteSettingProvider : ISiteSettingProvider
  {
    private readonly IConfiguration configuration;
    public SiteSetting SiteSetting { get; set; }
    public SiteSettingProvider(IConfiguration configuration)
    {
      SiteSetting = configuration.GetSection(nameof(SiteSetting)).Get<SiteSetting>();
      this.configuration = configuration;
    }
    public T Get<T>(string key)
    {
      var section = configuration.GetSection(key);
      if (section != null)
        return section.Get<T>();
      return default;
    }
  }
}