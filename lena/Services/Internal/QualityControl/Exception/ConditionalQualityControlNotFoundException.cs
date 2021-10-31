using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class ConditionalQualityControlNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ConditionalQualityControlNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
