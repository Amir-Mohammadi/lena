using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class MachineNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public MachineNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
