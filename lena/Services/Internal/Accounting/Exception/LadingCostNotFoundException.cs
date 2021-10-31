using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class LadingCostNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public LadingCostNotFoundException(int id)
    {
      Id = id;
    }
  }
}
