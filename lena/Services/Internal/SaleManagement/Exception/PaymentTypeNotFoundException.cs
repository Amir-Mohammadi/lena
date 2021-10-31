using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class PaymentTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public PaymentTypeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}

