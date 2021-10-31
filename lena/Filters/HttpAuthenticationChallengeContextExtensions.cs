
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
namespace lena.Filters
{
  public static class HttpAuthenticationChallengeContextExtensions
  {
    //TODO fix it sssss
    // public static void ChallengeWith(this HttpAuthenticationChallengeContext context, string scheme)
    // {
    //   ChallengeWith(context, new AuthenticationHeaderValue(scheme));
    // }
    // public static void ChallengeWith(this HttpAuthenticationChallengeContext context, string scheme, string parameter)
    // {
    //   ChallengeWith(context, new AuthenticationHeaderValue(scheme, parameter));
    // }
    // public static void ChallengeWith(this HttpAuthenticationChallengeContext context, AuthenticationHeaderValue challenge)
    // {
    //   if (context == null)
    //   {
    //     throw new ArgumentNullException("context");
    //   }
    //   context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
    // }
  }
}