using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class RequestWarehouseIssueNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }
    public RequestWarehouseIssueNotFoundException(int id)
    {
      this.Id = id;
    }
    public RequestWarehouseIssueNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
