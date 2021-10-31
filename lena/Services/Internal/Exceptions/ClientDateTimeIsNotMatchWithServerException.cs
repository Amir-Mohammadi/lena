using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Common.Exception
{
  public class ClientDateTimeIsNotMatchWithServerException : InternalServiceException
  {
    public DateTime ServerTime { get; set; }

    public ClientDateTimeIsNotMatchWithServerException(DateTime serverTime)
    {
      this.ServerTime = serverTime;
    }
  }
}
