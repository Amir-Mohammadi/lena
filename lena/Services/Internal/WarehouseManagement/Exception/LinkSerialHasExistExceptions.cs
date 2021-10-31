using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class LinkSerialHasExistExceptions : InternalServiceException
  {

    public string LinkSerial { get; set; }

    public LinkSerialHasExistExceptions(string linkSerial)
    {
      this.LinkSerial = linkSerial;
    }
  }
}
