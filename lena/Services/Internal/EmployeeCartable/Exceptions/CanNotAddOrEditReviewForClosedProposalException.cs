using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.EmployeeCartable.Exceptions
{
  public class CanNotAddOrEditReviewForClosedProposalException : InternalServiceException
  {
    public int ProposalId { get; set; }

    public CanNotAddOrEditReviewForClosedProposalException(int proposalId)
    {
      this.ProposalId = proposalId;
    }
  }
}
