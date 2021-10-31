using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class AmbiguousPublishedBillOfMaterialException : InternalServiceException
  {
    public string StuffCode { get; }

    public AmbiguousPublishedBillOfMaterialException(string stuffCode)
    {
      this.StuffCode = stuffCode;
    }
  }
}
