using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialDocumentDiscountNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public FinancialDocumentDiscountNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
