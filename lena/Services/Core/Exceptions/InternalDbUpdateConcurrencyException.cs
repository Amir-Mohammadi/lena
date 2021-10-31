using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

namespace lena.Services.Core.Exceptions
{
  public class InternalDbUpdateConcurrencyException : InternalServiceException
  {
    public InternalDbUpdateConcurrencyException(string message) : base(message)
    {

    }
  }
}
