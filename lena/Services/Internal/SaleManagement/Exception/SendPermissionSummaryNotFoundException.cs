using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class SendPermissionSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public SendPermissionSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
