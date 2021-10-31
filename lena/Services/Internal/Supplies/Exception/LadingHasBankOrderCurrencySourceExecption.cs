using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class LadingHasBankOrderCurrencySourceExecption : InternalServiceException
  {
    public int LadingId { get; }

    public LadingHasBankOrderCurrencySourceExecption(int ladingId)
    {
      this.LadingId = ladingId;
    }
  }
}
