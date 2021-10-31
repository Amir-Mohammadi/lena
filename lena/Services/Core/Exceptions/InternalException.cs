using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Exceptions
{
  public class InternalException : Exception
  {
    public InternalException() { }

    public InternalException(string message) : base(message)
    {

    }
  }
}
