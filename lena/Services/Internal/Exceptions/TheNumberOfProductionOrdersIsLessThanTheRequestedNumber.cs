using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class TheNumberOfProductionOrdersIsLessThanTheRequestedNumber : InternalServiceException
  {
    public double Count { get; set; }
    public TheNumberOfProductionOrdersIsLessThanTheRequestedNumber(double count)
    {
      Count = count;
    }
  }
}

