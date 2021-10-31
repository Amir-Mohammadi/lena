using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Qty
  public class QtyInvalidException : InternalServiceException
  {
    public double Qty { get; }
    public QtyInvalidException(double qty)
    {
      this.Qty = qty;
    }

  }
  #endregion
}
