using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CargoItemHasNoFinancialTransactionException : InternalServiceException
  {
    public string CargoItemCode { get; }

    public CargoItemHasNoFinancialTransactionException(string cargoItemCode)
    {
      CargoItemCode = cargoItemCode;
    }
  }
}
