using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialTransactionTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public FinancialTransactionTypeNotFoundException(int id)
    {
      Id = id;
    }
  }
}