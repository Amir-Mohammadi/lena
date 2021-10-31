using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class SendPermissionSummaryForSendPermissionNotFoundException : InternalServiceException
  {
    public int SendPermissionId { get; }

    public SendPermissionSummaryForSendPermissionNotFoundException(int sendPermissionId)
    {
      this.SendPermissionId = sendPermissionId;
    }
  }
}
