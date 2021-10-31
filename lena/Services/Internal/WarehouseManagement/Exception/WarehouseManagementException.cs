using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{

  public class SerialBufferNotFoundException : InternalServiceException
  {
    public string Serial { get; }

    public int Id { get; set; }
    public SerialBufferNotFoundException(int serialBufferId)
    {
      Id = serialBufferId;
    }

    public SerialBufferNotFoundException(string serial)
    {
      this.Serial = serial;
    }
  }

}
