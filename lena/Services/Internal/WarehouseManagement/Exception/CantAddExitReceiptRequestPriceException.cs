using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class CantAddExitReceiptRequestPriceException : InternalServiceException
  {
    public string ExitReceiptRequestTitle { get; }

    public CantAddExitReceiptRequestPriceException(string exitReceiptRequestTitle)
    {
      this.ExitReceiptRequestTitle = exitReceiptRequestTitle;
    }
  }
}



