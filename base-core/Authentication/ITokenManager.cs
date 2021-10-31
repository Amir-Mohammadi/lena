using System;
using System.Security.Claims;
using System.Threading.Tasks;
using core.Autofac;
using core.Models;
namespace core.Authentication
{
  public interface ITokenManagerService : IScopedDependency
  {
    Task<bool> SetSecurityStamp(string userId, string securityStamp);
    Task<bool> IsActiveToken();
    Task<bool> CheckSecurityStamp(string userId, string hashValue);
    Task<bool> DeactivateToken(string token, DateTime expireDate);
    string GenerateSecurityStamp(IUser user);
    Task<ClaimsPrincipal> ValidateToken(string token, TokenSetting tokenSetting);
    Task<string> GenerateToken(TokenSetting tokenSetting, params Claim[] claims);
  }
}