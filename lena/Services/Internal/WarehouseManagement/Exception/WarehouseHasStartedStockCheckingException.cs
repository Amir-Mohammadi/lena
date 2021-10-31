using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class WarehouseHasStartedStockCheckingException : InternalServiceException
  {
    public int Id { get; }

    public WarehouseHasStartedStockCheckingException(int id)
    {
      this.Id = id;
    }
  }
}
