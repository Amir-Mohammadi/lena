using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class PreparingSendingNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public PreparingSendingNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
