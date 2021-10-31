using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class BillOfMaterialIsPriceTypeException : InternalServiceException
  {
    public int StuffId { get; set; }
    public int Version { get; set; }
    public string StuffCode { get; set; }
    public BillOfMaterialIsPriceTypeException(string stuffCode, int stuffId, int? version = null)
    {
      this.StuffCode = stuffCode;
      this.StuffId = stuffId;
      this.Version = version.Value;
    }
  }
}
