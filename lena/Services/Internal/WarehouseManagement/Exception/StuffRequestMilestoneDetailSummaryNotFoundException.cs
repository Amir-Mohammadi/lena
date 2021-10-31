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
  public class StuffRequestMilestoneDetailSummaryNotFoundException : InternalServiceException
  {
    public int StuffRequestMilestoneDetailSummaryId { get; set; }

    public StuffRequestMilestoneDetailSummaryNotFoundException(int id)
    {
      this.StuffRequestMilestoneDetailSummaryId = id;

    }
  }
}
