using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class EquivalentStuffCanNotDeleteHasEquivalentStuffUsageException : InternalServiceException
  {
    public int Id { get; }

    public EquivalentStuffCanNotDeleteHasEquivalentStuffUsageException(int id)
    {
      this.Id = id;
    }
  }
}
