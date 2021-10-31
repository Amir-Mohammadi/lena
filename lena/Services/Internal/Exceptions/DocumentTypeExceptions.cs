using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Document Type
  public class DocumentTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public DocumentTypeNotFoundException(int id)
    {
      this.Id = id;
    }

  }
  #endregion

}
