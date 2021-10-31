using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class LinkedSerialNotHasRelationException : InternalServiceException
  {
    public string LinkedSerial { get; set; }

    public LinkedSerialNotHasRelationException(string linkedSerial)
    {
      this.LinkedSerial = linkedSerial;
    }
  }
}
