using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class PlanCodeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public PlanCodeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
