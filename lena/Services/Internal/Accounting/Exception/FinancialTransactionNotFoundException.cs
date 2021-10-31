using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialTransactionNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public FinancialTransactionNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
