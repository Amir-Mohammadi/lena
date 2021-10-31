using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{

  public class CustomerStuffVersionNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public CustomerStuffVersionNotFoundException(int id)
    {
      this.Id = id;

    }
  }
}
