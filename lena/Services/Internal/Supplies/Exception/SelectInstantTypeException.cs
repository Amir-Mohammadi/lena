using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class SelectInstantTypeException : InternalServiceException
  {
    public string StuffCode { get; set; }
    public SelectInstantTypeException(string stuffCode)
    {
      StuffCode = stuffCode;
    }
  }
}
