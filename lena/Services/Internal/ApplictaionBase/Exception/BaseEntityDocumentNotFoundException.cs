using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class BaseEntityDocumentNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public BaseEntityDocumentNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
