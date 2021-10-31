using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class OperationSequenceMachineTypeParameterNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public OperationSequenceMachineTypeParameterNotFoundException(int id)
    {
      Id = id;
    }
  }
}
