using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class MachineTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public MachineTypeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
