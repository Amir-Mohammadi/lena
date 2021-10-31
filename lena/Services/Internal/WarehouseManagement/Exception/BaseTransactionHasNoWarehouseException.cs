using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class BaseTransactionHasNoWarehouseException : InternalServiceException
  {
    public int Id { get; set; }

    public BaseTransactionHasNoWarehouseException(int id)
    {
      Id = id;
    }
  }
}
