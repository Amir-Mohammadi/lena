using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlConfirmationTestNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public QualityControlConfirmationTestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
