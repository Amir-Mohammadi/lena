using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class LinkSerialHasBeenLinkedToSerialExistExceptions : InternalServiceException
  {

    public string LinkSerial { get; set; }
    public string Serial { get; set; }

    public LinkSerialHasBeenLinkedToSerialExistExceptions(string linkSerial, string serial)
    {
      this.Serial = serial;
      this.LinkSerial = linkSerial;
    }
  }
}
