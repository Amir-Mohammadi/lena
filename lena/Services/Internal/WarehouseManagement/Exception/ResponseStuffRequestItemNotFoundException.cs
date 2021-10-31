using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ResponseStuffRequestItemNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Code { get; }

    public ResponseStuffRequestItemNotFoundException(int id)
    {
      this.Id = id;
    }

    public ResponseStuffRequestItemNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
