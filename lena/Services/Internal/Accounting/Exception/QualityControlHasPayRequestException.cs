using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class QualityControlHasPayRequestException : InternalServiceException
  {
    public string QualityControlCode { get; }

    public QualityControlHasPayRequestException(string qualityControlCode)
    {
      QualityControlCode = qualityControlCode;
    }
  }
}
