using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class UserCantEditPurchaseRequestException : InternalServiceException
  {
    public int Id { get; }

    public UserCantEditPurchaseRequestException(int id)
    {
      this.Id = id;
    }
  }
}