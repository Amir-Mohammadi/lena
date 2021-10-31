using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class SelectMoqTypeException : InternalServiceException
  {
    public string StuffCode { get; set; }
    public SelectMoqTypeException(string stuffCode)
    {
      StuffCode = stuffCode;
    }
  }
}
