using lena.Services.Core.Foundation;
using lena.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Externals.EmployeeCartable.Exceptions
{
  public class CanNotDeleteProposalWithChangedStatus : InternalServiceException
  {
    public int Id { get; set; }
    public ProposalStatus Status { get; set; }

    public CanNotDeleteProposalWithChangedStatus(int id, ProposalStatus status)
    {
      this.Id = id;
      this.Status = status;
    }
  }
}
