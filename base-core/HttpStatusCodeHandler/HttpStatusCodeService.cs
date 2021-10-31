using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace core.HttpStatusCodeHandler
{
  public class HttpStatusCodeService : IHttpStatusCodeService
  {
    public async Task Apply(HttpContext context)
    {
      //TODO compate this 
    }
  }
}