
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class AllocationDoNotHaveDocumentException : InternalServiceException
  {
    public int Id { get; }

    public AllocationDoNotHaveDocumentException(int id)
    {
      this.Id = id;
    }
  }
}
