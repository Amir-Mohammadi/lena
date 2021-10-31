using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductStuffDetailNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductStuffDetailNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
