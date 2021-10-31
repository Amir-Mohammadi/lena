using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseRequestConfirmedQtyIsLessThanOrderQtyException : InternalServiceException
  {
    public int Id { get; }
    public double EditQty { get; set; }
    public double OrderQty { get; set; }

    public PurchaseRequestConfirmedQtyIsLessThanOrderQtyException(int id, double editQty, double orderQty)
    {
      Id = id;
      EditQty = editQty;
      OrderQty = orderQty;
    }
  }
}
