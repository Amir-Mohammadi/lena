using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class StuffQualityControlTestImportanceDegreeNotFoundException : InternalServiceException
  {
    public long Id { get; }

    public StuffQualityControlTestImportanceDegreeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
