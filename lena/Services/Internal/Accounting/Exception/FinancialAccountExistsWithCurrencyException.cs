using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialAccountExistsWithCurrencyException : InternalServiceException
  {
    public int Id { get; }

    public FinancialAccountExistsWithCurrencyException(int id)
    {
      this.Id = id;
    }
  }
}
