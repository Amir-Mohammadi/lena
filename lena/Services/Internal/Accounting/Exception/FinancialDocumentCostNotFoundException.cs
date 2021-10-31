using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialDocumentCostNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public FinancialDocumentCostNotFoundException(int id)
    {
      Id = id;
    }
  }
}
