using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseRequestSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public PurchaseRequestSummaryNotFoundException(int id)
    {
      this.Id = id;

    }

  }
}
