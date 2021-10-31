using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseRequestEditQtyGreaterThanRemainedQtyException : InternalServiceException
  {
    public int Id { get; }
    public double EditQty { get; set; }
    public double RemainedQty { get; set; }

    public PurchaseRequestEditQtyGreaterThanRemainedQtyException(int id, double editQty, double remainedQty)
    {
      Id = id;
      EditQty = editQty;
      RemainedQty = remainedQty;
    }
  }
}
