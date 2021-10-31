using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class BankOrderCurrencySourceNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public BankOrderCurrencySourceNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
