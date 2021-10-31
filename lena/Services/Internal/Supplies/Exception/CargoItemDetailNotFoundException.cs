using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemDetailNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public CargoItemDetailNotFoundException(string code)
    {
      this.Code = code;
    }

    public CargoItemDetailNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
