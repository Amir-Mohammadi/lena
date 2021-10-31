using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using core.Authentication;
using core.Context;
using core.Crypto;
using core.ExceptionHandler;
using core.Models;
using core.Setting;
using core.StateManager;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using lena.Services.Core;
using lena.Domains.Enums;
namespace lena.ServicesUsers
{
  public class TokenManagerService : ITokenManagerService
  {
    #region Fields
    private readonly IStateManagerService stateManagerService;
    private readonly ICryptoService cryptoService;
    private readonly IWorkContext workContext;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public TokenManagerService(IStateManagerService stateManagerService,
                               ICryptoService cryptoService,
                               IWorkContext workContext,
                               IErrorFactory errorFactory)
    {
      this.stateManagerService = stateManagerService;
      this.cryptoService = cryptoService;
      this.workContext = workContext;
      this.errorFactory = errorFactory;
    }
    #endregion
    public async Task<bool> SetSecurityStamp(string userId, string securityStamp)
    {
      return await stateManagerService.SetState($"user:{userId}", securityStamp);
    }
    public async Task<bool> IsActiveToken()
    {
      var hashToken = cryptoService.Hash(workContext.GetUserToken());
      var value = await stateManagerService.GetState<string>($"tokens:{hashToken}");
      if (string.IsNullOrEmpty(value))
        return true;
      else
        return false;
    }
    public async Task<bool> CheckSecurityStamp(string userId, string hashValue)
    {
      var value = await this.stateManagerService.GetState<string>($"user:{userId}");
      var tokenStamp = Encoding.ASCII.GetBytes(hashValue);
      var mainStamp = Encoding.ASCII.GetBytes(value);
      if (CheckStamp(tokenStamp, mainStamp))
        return true;
      else
        return false;
    }
    public async Task<bool> DeactivateToken(string token, DateTime expireDate)
    {
      var date = DateTime.UtcNow;
      var expirationTimeSpan = expireDate.Subtract(date);
      return await this.stateManagerService.SetState($"tokens:{token}", "deactivated", expirationTimeSpan);
    }
    public async Task<ClaimsPrincipal> ValidateToken(string token, TokenSetting tokenSetting)
    {
      var secretkey = Encoding.UTF8.GetBytes(tokenSetting.SecretKey);
      var encryptionkey = Encoding.UTF8.GetBytes(tokenSetting.EncryptKey);
      var validationParameters = new TokenValidationParameters
      {
        ClockSkew = TimeSpan.Zero,
        RequireSignedTokens = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretkey),
        RequireExpirationTime = true,
        ValidateLifetime = false,
        ValidateAudience = true,
        ValidAudience = tokenSetting.Audience,
        ValidateIssuer = true,
        ValidIssuer = tokenSetting.Issuer,
        TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey),
        NameClaimType = ClaimTypes.NameIdentifier
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
      if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.Aes256KW))
      {
        throw errorFactory.InvalidToken();
      }
      return await Task.Run(() => claimsPrincipal);
    }
    public string GenerateSecurityStamp(IUser user)
    {
      var securityStamp = user.GetSecurityStamp();
      return cryptoService.Hash(securityStamp);
    }
    public async Task<string> GenerateToken(TokenSetting tokenSetting, params Claim[] claims)
    {
      var secretKey = Encoding.UTF8.GetBytes(tokenSetting.SecretKey);
      var signingCredentials = new SigningCredentials(key: new SymmetricSecurityKey(secretKey),
                                                      algorithm: SecurityAlgorithms.HmacSha256Signature);
      var encryptionkey = Encoding.UTF8.GetBytes(tokenSetting.EncryptKey);
      var encryptingCredentials = new EncryptingCredentials(key: new SymmetricSecurityKey(encryptionkey),
                                                            alg: SecurityAlgorithms.Aes256KW,
                                                            enc: SecurityAlgorithms.Aes256CbcHmacSha512);
      var descriptor = new SecurityTokenDescriptor
      {
        Issuer = tokenSetting.Issuer,
        Audience = tokenSetting.Audience,
        IssuedAt = DateTime.UtcNow,
        Expires = DateTime.UtcNow.AddMinutes(tokenSetting.ExpirationMinutes),
        SigningCredentials = signingCredentials,
        EncryptingCredentials = encryptingCredentials,
        Subject = new ClaimsIdentity(claims)
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var securityToken = tokenHandler.CreateToken(descriptor);
      var jwt = tokenHandler.WriteToken(securityToken);
      return await Task.Run(() => jwt.ToString());
    }
    private static bool CheckStamp(byte[] sourceA, byte[] sourceB)
    {
      return sourceB.SequenceEqual(sourceA);
    }
  }
}