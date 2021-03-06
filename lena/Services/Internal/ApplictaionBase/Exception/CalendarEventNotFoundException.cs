using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class CalendarEventNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public CalendarEventNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
