using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class NewShoppingNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public NewShoppingNotFoundException(string code)
    {
      this.Code = code;
    }

    public NewShoppingNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
