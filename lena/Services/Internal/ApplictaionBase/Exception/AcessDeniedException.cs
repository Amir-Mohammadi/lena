using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class AcessDeniedException : InternalServiceException
  {
    public int? SecurityActionId { get; }
    public string SecurityActionAddress { get; }
    public string SecurityActionName { get; }

    public AcessDeniedException(
        int? securityActionId,
        string securityActionAddress,
        string securityActionName)
    {
      SecurityActionId = securityActionId;
      SecurityActionAddress = securityActionAddress;
      SecurityActionName = securityActionName;
    }
  }
}
