using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Foundation.Service.Internal
{
  public class UnknownInternalServiceException : InternalServiceException
  {
    public UnknownInternalServiceException(string message, Exception ex) : base(message, ex) { }
  }
}
