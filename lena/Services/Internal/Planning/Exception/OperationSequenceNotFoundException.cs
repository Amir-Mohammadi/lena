using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class OperationSequenceNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public OperationSequenceNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
