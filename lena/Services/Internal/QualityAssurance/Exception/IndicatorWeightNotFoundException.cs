using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance.Exception
{
  public class IndicatorWeightNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public IndicatorWeightNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
