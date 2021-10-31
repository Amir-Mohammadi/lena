using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class NotHavePermissionToRejectOrConfirmStuffPurchaseRequestException : InternalServiceException
  {
    public string UserGroups { get; }

    public NotHavePermissionToRejectOrConfirmStuffPurchaseRequestException(string userGroups)
    {
      UserGroups = userGroups;
    }
  }
}
