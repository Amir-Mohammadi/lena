using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  class StuffMismatchException : InternalServiceException
  {
    public int RequiredStuffId { get; set; }
    public int GivenStuffId { get; set; }

    public StuffMismatchException(int requiredStuffId, int givenStuffId)
    {
      RequiredStuffId = requiredStuffId;
      GivenStuffId = givenStuffId;
    }
  }
}
