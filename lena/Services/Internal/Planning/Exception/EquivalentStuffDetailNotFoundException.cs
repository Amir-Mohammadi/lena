using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class EquivalentStuffDetailNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public EquivalentStuffDetailNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
