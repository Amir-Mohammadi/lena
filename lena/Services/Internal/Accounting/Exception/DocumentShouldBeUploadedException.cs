using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class DocumentShouldBeUploadedException : InternalServiceException
  {
    public int Id { get; }

    public DocumentShouldBeUploadedException(int id)
    {
      this.Id = id;
    }
  }
}
