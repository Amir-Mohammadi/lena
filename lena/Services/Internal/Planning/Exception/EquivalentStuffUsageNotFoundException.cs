using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class EquivalentStuffUsageNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public EquivalentStuffUsageNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
