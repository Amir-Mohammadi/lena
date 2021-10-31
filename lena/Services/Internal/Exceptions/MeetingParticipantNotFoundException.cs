using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class MeetingParticipantNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public MeetingParticipantNotFoundException(int id)
    {
      Id = id;
    }
  }
}
