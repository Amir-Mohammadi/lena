using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class MeetingApprovalNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public MeetingApprovalNotFoundException(int id)
    {
      Id = id;
    }
  }
}
