
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace lena.Filters
{
  //TODO fix ssss
  // public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
  // {
  //   public string Realm { get; set; }
  //   public bool AllowMultiple => false;
  //   public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
  //   {
  //     var request = context.Request;
  //     var authorization = request.Headers.Authorization;
  //     if (authorization == null || authorization.Scheme != "Bearer")
  //       return;
  //     if (string.IsNullOrEmpty(authorization.Parameter))
  //     {
  //       context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
  //       return;
  //     }
  //     var token = authorization.Parameter;
  //     var principal = await AuthenticateJwtToken(token);
  //     if (principal == null)
  //     {
  //       context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
  //       return;
  //     }
  //     context.Principal = principal;
  //   }
  //   private static bool ValidateToken(string token, ref IPrincipal principal)
  //   {
  //     var simplePrinciple = JwtManager.GetPrincipal(token);
  //     principal = simplePrinciple;
  //     var identity = simplePrinciple?.Identity as ClaimsIdentity;
  //     if (identity == null)
  //       return false;
  //     if (!identity.IsAuthenticated)
  //       return false;
  //     //check the valid token that logged out
  //     var key = $"Deactive:{token.ComputeHashToken()}";
  //     if (App.Providers.Session.Contains(key))
  //       return false;
  //     //check security stamp
  //     var userId = identity.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
  //     var tokenStamp = identity.FindFirst(x => x.Type == "securityStamp")?.Value;
  //     var mainStamp = App.Providers.Session.GetAs<string>(SessionKey.SecurityStamp.ToString().KeyPrefix(userId));
  //     if (tokenStamp == null || mainStamp == null || !JwtManager.CheckStamp(Encoding.ASCII.GetBytes(mainStamp), Encoding.ASCII.GetBytes(tokenStamp)))
  //       return false;
  //     return true;
  //   }
  //   protected Task<IPrincipal> AuthenticateJwtToken(string token)
  //   {
  //     IPrincipal principal = null;
  //     if (ValidateToken(token, ref principal))
  //     {
  //       return Task.FromResult(principal);
  //     }
  //     return Task.FromResult<IPrincipal>(null);
  //   }
  //   public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
  //   {
  //     Challenge(context);
  //     return Task.FromResult(0);
  //   }
  //   private void Challenge(HttpAuthenticationChallengeContext context)
  //   {
  //     string parameter;
  //     if (String.IsNullOrEmpty(Realm))
  //     {
  //       parameter = null;
  //     }
  //     else
  //     {
  //       parameter = "realm=\"" + Realm + "\"";
  //     }
  //     context.ChallengeWith("Basic", parameter);
  //   }
  // }
}