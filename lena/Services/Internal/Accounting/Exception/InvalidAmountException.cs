using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class InvalidAmountException : InternalServiceException
  {
    public double Amount { get; set; }

    public InvalidAmountException(double amount)
    {
      Amount = amount;
    }
  }
}
