using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class EnactmentActionProcessLogNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public EnactmentActionProcessLogNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
