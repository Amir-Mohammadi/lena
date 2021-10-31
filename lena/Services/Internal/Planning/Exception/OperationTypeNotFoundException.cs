using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class OperationTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OperationTypeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
