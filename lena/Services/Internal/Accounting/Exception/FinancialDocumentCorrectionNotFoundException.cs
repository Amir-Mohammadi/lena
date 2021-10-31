using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialDocumentCorrectionNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public FinancialDocumentCorrectionNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
