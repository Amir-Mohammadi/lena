using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class QualityControlSampleStatusInWarehouseStatusException : InternalServiceException
  {
    public int Id { get; set; }

    public QualityControlSampleStatusInWarehouseStatusException(int id)
    {
      this.Id = id;
    }
  }
}
