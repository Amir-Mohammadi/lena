using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class CurrentItemQtyIsBiggerThanTargetItemQtyException : InternalServiceException
  {
    public double CurrentItemQty { get; set; }
    public double TargetItemQty { get; set; }

    public CurrentItemQtyIsBiggerThanTargetItemQtyException(double currentItemQty, double targetItemQty)
    {
      CurrentItemQty = currentItemQty;
      TargetItemQty = targetItemQty;
    }
  }
}
