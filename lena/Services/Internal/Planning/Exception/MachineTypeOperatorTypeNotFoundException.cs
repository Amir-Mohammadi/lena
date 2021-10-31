using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class MachineTypeOperatorTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public MachineTypeOperatorTypeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
