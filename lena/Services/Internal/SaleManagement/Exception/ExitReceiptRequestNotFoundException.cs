using lena.Services.Core.Foundation;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ExitReceiptRequestNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ExitReceiptRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
