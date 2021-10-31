using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class ProposalNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ProposalNotFoundException(int id)
    {
      Id = id;
    }
  }
}
