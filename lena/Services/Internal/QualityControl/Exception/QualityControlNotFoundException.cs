using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public QualityControlNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
