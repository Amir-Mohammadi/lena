using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ProductionRequestSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionRequestSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
