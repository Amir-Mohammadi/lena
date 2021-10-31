using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class CantEditPriceAnnunciationException : InternalServiceException
  {
    public int Id { get; }

    public CantEditPriceAnnunciationException(int id)
    {
      this.Id = id;
    }
  }
}
