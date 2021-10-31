using Microsoft.IdentityModel.Tokens;
using lena.Services.Core.Common;
using lena.Domains;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.TokenManager
{
  public static class JwtManager
  {

    private static string SecretKey = App.Providers.Storage.SecretKey;
    private static string Encryptkey = App.Providers.Storage.Encryptkey;
    private static string Issuer = App.Providers.Storage.Issuer;
    private static string Audience = App.Providers.Storage.Issuer;
    public static string GenerateToken(User user, Guid stateKey, DateTime expireDateTime, ref string securityStamp)
    {
      var secretKey = Encoding.UTF8.GetBytes(SecretKey);
      var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

      var encryptionkey = Encoding.UTF8.GetBytes(Encryptkey);
      var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);

      securityStamp = generateSeurityStamp(user);

      var claims = _getClaims(user, stateKey, securityStamp);
      var descriptor = new SecurityTokenDescriptor
      {
        Issuer = Issuer,
        Audience = Audience,
        IssuedAt = DateTime.Now,
        NotBefore = DateTime.Now.AddMinutes(0),
        Expires = expireDateTime,
        SigningCredentials = signingCredentials,
        EncryptingCredentials = encryptingCredentials,
        Subject = new ClaimsIdentity(claims)
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var securityToken = tokenHandler.CreateToken(descriptor);

      var jwt = tokenHandler.WriteToken(securityToken);

      return jwt;
    }

    public static ClaimsPrincipal GetPrincipal(string token, bool checkExpire = true)
    {
      try
      {
        var secretkey = Encoding.UTF8.GetBytes(SecretKey);
        var encryptionkey = Encoding.UTF8.GetBytes(Encryptkey);

        var validationParameters = new TokenValidationParameters
        {
          ClockSkew = TimeSpan.Zero, // default: 5 min
          RequireSignedTokens = true,

          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(secretkey),

          RequireExpirationTime = true,
          ValidateLifetime = checkExpire,

          ValidateAudience = true, //default : false
          ValidAudience = Audience,

          ValidateIssuer = true, //default : false
          ValidIssuer = Issuer,

          TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.Aes256KW))
        {
          return null;
        }

        return principal;
      }

      catch (Exception e)
      {
        return null;
      }
    }

    private static IEnumerable<Claim> _getClaims(User user, Guid stateKey, string securityStamp)
    {
      var list = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("stateKey",stateKey.ToString()),
                new Claim("securityStamp",securityStamp),
            };
      return list;
    }

    public static string generateSeurityStamp(User user)
    {
      var stamp = user.Password + "-" + user.IsActive + "-" + user.IsLocked + "-" + user.IsDelete;
      return Crypto.Sha1(stamp);
    }

    public static bool CheckStamp(byte[] sourceA, byte[] sourceB)
    {
      if (sourceB.Length != sourceA.Length) return false;

      for (int index = 0; index < sourceA.Length; index++)
      {
        if (sourceA[index] != sourceB[index])
          return false;
      }
      return true;
    }
  }
}
