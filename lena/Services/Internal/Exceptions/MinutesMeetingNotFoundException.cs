using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class MinutesMeetingNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public MinutesMeetingNotFoundException(int id)
    {
      Id = id;
    }
  }
}
