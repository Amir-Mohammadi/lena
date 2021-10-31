using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class QualityControlDocumentExistException : InternalServiceException
  {
    public int Id { get; }

    public QualityControlDocumentExistException(int id)
    {
      this.Id = id;
    }
  }
}
