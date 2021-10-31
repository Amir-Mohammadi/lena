using System.Net.Sockets;
using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ResponseWarehouseIssueNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Code { get; }

    public ResponseWarehouseIssueNotFoundException(int id)
    {
      this.Id = id;

    }

    public ResponseWarehouseIssueNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
