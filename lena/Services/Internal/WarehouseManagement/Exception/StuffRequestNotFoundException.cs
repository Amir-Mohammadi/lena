
using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class StuffRequestNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public StuffRequestNotFoundException(string code)
    {
      this.Code = code;
    }

    public StuffRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
