using core.Autofac;
namespace core.Setting
{
  public interface ISiteSettingProvider : ISingletonDependency
  {
    SiteSetting SiteSetting { get; set; }
    T Get<T>(string key);
  }
}