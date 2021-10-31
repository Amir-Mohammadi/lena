using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionLineNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionLineNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
