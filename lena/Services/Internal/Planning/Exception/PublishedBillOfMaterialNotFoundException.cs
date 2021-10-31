using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class PublishedBillOfMaterialNotFoundException : InternalServiceException
  {
    public string StuffCode { get; set; }

    public PublishedBillOfMaterialNotFoundException(string stuffCode)
    {
      this.StuffCode = stuffCode;
    }
  }
}
