using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class StuffQualityControlTestOperationNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public StuffQualityControlTestOperationNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
