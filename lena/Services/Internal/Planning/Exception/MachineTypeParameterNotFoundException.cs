using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class MachineTypeParameterNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public MachineTypeParameterNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
