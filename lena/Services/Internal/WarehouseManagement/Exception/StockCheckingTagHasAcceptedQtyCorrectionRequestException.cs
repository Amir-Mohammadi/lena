using lena.Services.Core.Foundation;
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
  public class StockCheckingTagHasAcceptedQtyCorrectionRequestException : InternalServiceException
  {
    public int StockCheckingTagId { get; set; }
    public string Serial { get; }

    public StockCheckingTagHasAcceptedQtyCorrectionRequestException(int stockCheckingTagId, string serial)
    {
      StockCheckingTagId = stockCheckingTagId;
      Serial = serial;
    }
  }
}
