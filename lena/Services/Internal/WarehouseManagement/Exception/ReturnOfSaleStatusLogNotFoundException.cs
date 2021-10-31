using lena.Services.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ReturnOfSaleStatusLogNotFoundException : InternalException

  {

    public int ReturnOfSaleStatusLogId { get; set; }
    public ReturnOfSaleStatusLogNotFoundException(int id)
    {
      ReturnOfSaleStatusLogId = id;
    }
  }
}
