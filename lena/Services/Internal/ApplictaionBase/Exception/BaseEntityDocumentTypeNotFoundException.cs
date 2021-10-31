using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class BaseEntityDocumentTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public BaseEntityDocumentTypeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
