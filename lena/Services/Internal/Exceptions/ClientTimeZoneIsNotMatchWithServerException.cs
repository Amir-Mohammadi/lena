using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class ClientTimeZoneIsNotMatchWithServerException : InternalServiceException
  {
    public string ServerTimeZone { get; }

    public ClientTimeZoneIsNotMatchWithServerException(string serverTimeZone)
    {
      ServerTimeZone = serverTimeZone;
    }
  }
}
