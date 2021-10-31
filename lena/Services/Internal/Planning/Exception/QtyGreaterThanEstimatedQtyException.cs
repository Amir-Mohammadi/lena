using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class QtyGreaterThanEstimatedQtyException : InternalServiceException
  {
    public double EstimatedQty { get; }
    public double Qty { get; }
    public QtyGreaterThanEstimatedQtyException(double qty, double estimatedQty)
    {
      this.Qty = qty;
      this.EstimatedQty = estimatedQty;
    }
  }
}
