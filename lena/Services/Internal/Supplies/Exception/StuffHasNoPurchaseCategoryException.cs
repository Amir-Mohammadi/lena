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
  public class StuffHasNoPurchaseCategoryException : InternalServiceException
  {
    public int StuffId { get; }
    public string StuffCode { get; }

    public StuffHasNoPurchaseCategoryException(int stuffId, string stuffCode)
    {
      StuffId = stuffId;
      StuffCode = stuffCode;
    }
  }
}
