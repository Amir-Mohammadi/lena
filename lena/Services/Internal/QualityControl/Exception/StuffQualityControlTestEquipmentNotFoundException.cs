using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class StuffQualityControlTestEquipmentNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public StuffQualityControlTestEquipmentNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
