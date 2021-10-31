using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class SerialBufferShortageAmountException : InternalServiceException
  {
    public double ShortageAmount { get; }
    public int Id { get; }

    public SerialBufferShortageAmountException(int id, double shortageAmount)
    {
      this.Id = id;
      this.ShortageAmount = shortageAmount;
    }
  }
}
