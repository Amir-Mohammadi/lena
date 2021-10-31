using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class SupplierNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public SupplierNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
