using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace lena.Filters.FilterResult
{
  public class AddChallengeOnUnauthorizedResult : ActionResult
  {
    public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IActionResult innerResult)
    {
      Challenge = challenge;
      InnerResult = innerResult;
    }
    public AuthenticationHeaderValue Challenge { get; private set; }
    public IActionResult InnerResult { get; private set; }
    public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
      HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);
      if (response.StatusCode == HttpStatusCode.Unauthorized)
      {
        // Only add one challenge per authentication scheme.
        //برای جلوگیری از عدم نمایش صفحه لاگین مرورگر، زمانی که یوزر دسترسی به عملیات مورد نظر را ندارد (HTTP_UNAUTHORIZED")
        //if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
        //{
        //    response.Headers.WwwAuthenticate.Add(Challenge);
        //}
      }
      return response;
    }
  }
}