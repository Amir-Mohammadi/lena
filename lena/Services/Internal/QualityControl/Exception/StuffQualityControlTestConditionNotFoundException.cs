using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class StuffQualityControlTestConditionNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public StuffQualityControlTestConditionNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
