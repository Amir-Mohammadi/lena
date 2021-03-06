using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class PlanCodeUsedInPurchaseRequestException : InternalServiceException
  {
    public int Id { get; }

    public PlanCodeUsedInPurchaseRequestException(int id)
    {
      this.Id = id;
    }
  }
}
