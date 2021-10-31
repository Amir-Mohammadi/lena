
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace lena.Filters.FilterResult
{
  //TODO fix it sssss
  public class AuthenticationFailureResult : IActionResult
  {
    public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
    {
      ReasonPhrase = reasonPhrase;
      Request = request;
    }
    public string ReasonPhrase { get; }
    public HttpRequestMessage Request { get; }
    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
      return Task.FromResult(Execute());
    }
    public Task ExecuteResultAsync(ActionContext context)
    {
      throw new System.NotImplementedException();
    }
    private HttpResponseMessage Execute()
    {
      //TODO fix it sssss
      // HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
      // {
      //   Content = new StringContent(Json.Encode(new Result
      //   {
      //     Success = false,
      //     Message = new UserNotLoginException().ToString()
      //   }))
      // };
      // //response.RequestMessage = Request;
      // //response.ReasonPhrase = ReasonPhrase;
      // return response;
      return null;
    }
  }
}