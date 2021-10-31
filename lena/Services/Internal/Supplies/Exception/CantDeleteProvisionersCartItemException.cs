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
  public class CantDeleteProvisionersCartItemException : InternalServiceException
  {
    public double? SuppliedQty { get; set; }
    public CantDeleteProvisionersCartItemException(double? suppliedQty)
    {
      SuppliedQty = suppliedQty;
    }

  }
}
