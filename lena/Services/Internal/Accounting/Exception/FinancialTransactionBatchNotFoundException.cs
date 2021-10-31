using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialTransactionBatchNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public FinancialTransactionBatchNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
