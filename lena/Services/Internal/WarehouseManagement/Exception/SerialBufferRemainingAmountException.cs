using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class SerialBufferRemainingAmountException : InternalServiceException
  {
    public double RemainingAmount { get; }
    public string Serial { get; }

    public SerialBufferRemainingAmountException(string serial, double remainingAmount)
    {
      this.Serial = serial;
      this.RemainingAmount = remainingAmount;
    }
  }
}
