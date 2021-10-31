using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class IndicatorNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public IndicatorNotFoundException(int id)
    {
      Id = id;
    }
  }
}
