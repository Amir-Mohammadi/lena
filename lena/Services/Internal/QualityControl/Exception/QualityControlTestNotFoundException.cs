using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlTestNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public QualityControlTestNotFoundException(long id)
    {
      this.Id = id;
    }
  }
}
