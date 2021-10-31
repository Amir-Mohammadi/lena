using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class PackingMaterialMustBeConsumedCompletelyException : InternalServiceException
  {
    public string Serial { get; set; }

    public double RequiredQty { get; set; }
    public double SerialQty { get; set; }


    public PackingMaterialMustBeConsumedCompletelyException(string serial, double requiredQty, double serialQty)
    {
      Serial = serial;
      RequiredQty = requiredQty;
      SerialQty = serialQty;
    }
  }
}
