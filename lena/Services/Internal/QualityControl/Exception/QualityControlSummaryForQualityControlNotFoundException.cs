using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlSummaryForQualityControlNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public QualityControlSummaryForQualityControlNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
