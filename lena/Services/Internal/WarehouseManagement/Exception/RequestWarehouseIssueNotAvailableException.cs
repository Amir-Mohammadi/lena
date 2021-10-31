using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class RequestWarehouseIssueNotAvailableException : InternalServiceException
  {
    public string Serial { get; }
    public RequestWarehouseIssueNotAvailableException(string serial)
    {
      this.Serial = serial;
    }
  }
}
