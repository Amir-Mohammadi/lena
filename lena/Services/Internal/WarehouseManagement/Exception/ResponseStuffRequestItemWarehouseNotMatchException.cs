using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ResponseStuffRequestItemWarehouseNotMatchException : InternalServiceException
  {
    public int Id { get; }

    public ResponseStuffRequestItemWarehouseNotMatchException(int id)
    {
      this.Id = id;
    }
  }
}
