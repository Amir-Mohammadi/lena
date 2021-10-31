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
  public class LadingItemQtyIsZeroException : InternalServiceException
  {
    public int LadingItemId { get; set; }
    public string LadingItemCode { get; set; }

    public LadingItemQtyIsZeroException(int ladingItemId, string ladingItemCode)
    {
      LadingItemId = ladingItemId;
      LadingItemCode = ladingItemCode;
    }
  }
}
