using core.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace core.OAuth
{
  public static class OAuthServiceExtention
  {
    private static void RegisterScope(this AuthorizationOptions options, string name, string issuer)
    {
      options.AddPolicy(name, policy =>
        {
          policy.Requirements.Add(new HasScopeRequirement(name, issuer));
        });
    }
    public static void AddOAuthAuthorization<TScopes>(this IServiceCollection services, IConfiguration configuration) where TScopes : class, IScopes
    {
      services.AddScoped<TScopes>();
      var scopes = services.BuildServiceProvider().GetService<TScopes>();
      var tokenSetting = configuration.GetSection(nameof(SiteSetting)).Get<SiteSetting>().TokenSetting;
      services.AddAuthorization(options =>
      {
        foreach (var item in scopes.Map)
          options.RegisterScope(item.Key, tokenSetting.Issuer);
      });
      services.AddSingleton<IAuthorizationHandler, HasScopeAuthorizationHandler>();
    }
  }
}