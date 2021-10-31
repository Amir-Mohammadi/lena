using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class LinkedSerialNotFoundException : InternalServiceException
  {
    public string LinkedSerial { get; set; }

    public LinkedSerialNotFoundException(string linkedSerial)
    {
      this.LinkedSerial = linkedSerial;
    }
  }
}
