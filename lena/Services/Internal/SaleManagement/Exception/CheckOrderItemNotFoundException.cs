using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class CheckOrderItemNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public CheckOrderItemNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
