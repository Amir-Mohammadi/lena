using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class ProposalQAReviewNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ProposalQAReviewNotFoundException(int id)
    {
      Id = id;
    }
  }
}
