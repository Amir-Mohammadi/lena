using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class SerialBufferDamagedAmountException : InternalServiceException
  {
    public double DamagedAmount { get; }
    public int Id { get; }

    public SerialBufferDamagedAmountException(int id, double damagedAmount)
    {
      this.Id = id;
      this.DamagedAmount = damagedAmount;
    }
  }
}
