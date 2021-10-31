using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class CantDeletePriceAnnunciationException : InternalServiceException
  {
    public int Id { get; }

    public CantDeletePriceAnnunciationException(int id)
    {
      this.Id = id;
    }
  }
}
