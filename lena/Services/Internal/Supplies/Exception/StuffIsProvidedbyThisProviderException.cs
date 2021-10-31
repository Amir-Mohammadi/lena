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
  public class StuffIsProvidedByThisProviderException : InternalServiceException
  {
    public int StuffId { get; set; }
    public int ProviderId { get; set; }

    public StuffIsProvidedByThisProviderException(int stuffId, int providerId)
    {
      this.StuffId = stuffId;
      this.ProviderId = providerId;
    }
  }
}
