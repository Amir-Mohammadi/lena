using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlSampleNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public QualityControlSampleNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
