using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class CustomQualityControlNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public CustomQualityControlNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
