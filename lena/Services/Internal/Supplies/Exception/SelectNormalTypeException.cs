using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class SelectNormalTypeException : InternalServiceException
  {
    public string StuffCode { get; }
    public SelectNormalTypeException(string stuffCode)
    {
      StuffCode = stuffCode;
    }
  }
}
