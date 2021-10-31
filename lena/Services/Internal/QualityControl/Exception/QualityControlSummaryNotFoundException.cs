using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public QualityControlSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
