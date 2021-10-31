using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class WorkShiftNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public WorkShiftNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
