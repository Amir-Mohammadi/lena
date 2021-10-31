using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class DuplicateAssignEmployeeToStuffPurchaseException : InternalServiceException
  {
    public int UserId { get; set; }
    public int StuffId { get; set; }
    public DuplicateAssignEmployeeToStuffPurchaseException(int userId, int stuffId)
    {
      StuffId = stuffId;
      UserId = userId;
    }
  }
}
