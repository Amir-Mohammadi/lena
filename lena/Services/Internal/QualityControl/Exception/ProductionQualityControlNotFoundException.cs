using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class ProductionQualityControlNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionQualityControlNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
