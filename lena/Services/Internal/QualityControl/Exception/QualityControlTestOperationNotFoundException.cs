using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlTestOperationNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public QualityControlTestOperationNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
