using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class StuffQualityControlTestNotFoundException : InternalServiceException
  {
    public long QualityControlTestId { get; }
    public int StuffId { get; }

    public StuffQualityControlTestNotFoundException(int stuffId, long qualityControlTestId)
    {
      this.StuffId = stuffId;
      this.QualityControlTestId = qualityControlTestId;
    }
  }
}
