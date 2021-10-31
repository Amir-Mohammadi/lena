using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionFaultTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionFaultTypeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
