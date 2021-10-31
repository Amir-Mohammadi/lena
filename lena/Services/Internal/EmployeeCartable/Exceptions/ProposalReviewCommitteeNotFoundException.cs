using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class ProposalReviewCommitteeNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ProposalReviewCommitteeNotFoundException(int id)
    {
      Id = id;
    }
  }
}
