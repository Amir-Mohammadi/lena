using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class BillOfMaterialPublishRequestExistsForMaterialException : InternalServiceException
  {
    private int stuffId { get; set; }

    public BillOfMaterialPublishRequestExistsForMaterialException(int stuffId)
    {
      this.stuffId = stuffId;
    }
  }
}
