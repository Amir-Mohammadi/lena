using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialDocumentNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public FinancialDocumentNotFoundException(int id)
    {
      Id = id;
    }
  }
}