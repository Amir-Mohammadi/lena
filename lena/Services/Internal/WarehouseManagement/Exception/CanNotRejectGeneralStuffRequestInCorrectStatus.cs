using lena.Services.Core.Foundation;
using lena.Domains.Enums;
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
  public class CanNotRejectGeneralStuffRequestInCorrectStatus : InternalServiceException
  {
    public int Id { get; set; }
    public GeneralStuffRequestStatus Status { get; set; }
    public CanNotRejectGeneralStuffRequestInCorrectStatus(int id, GeneralStuffRequestStatus status)
    {
      Id = id;
      Status = status;
    }
  }
}
