using System;
using System.Security.Claims;
using core.Autofac;
namespace core.Context
{
  public interface IWorkContext : IScopedDependency
  {
    bool IsAuthenticated();
    string GetCurrentUserId();
    Claim GetClaim(string key);
    DateTime GetUserTokenExpiration();
    string GetUserToken();
  }
}