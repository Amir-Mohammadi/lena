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
  public class PurchaseRequestEditQtyGreaterThanConfirmedQtyException : InternalServiceException
  {
    public int Id { get; }
    public double EditQty { get; set; }
    public double ConfirmedQty { get; set; }

    public PurchaseRequestEditQtyGreaterThanConfirmedQtyException(int id, double editQty, double confirmedQty)
    {
      Id = id;
      EditQty = editQty;
      ConfirmedQty = confirmedQty;
    }
  }
}
