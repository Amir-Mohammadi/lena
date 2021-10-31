using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class AllocationNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public AllocationNotFoundException(int id)
    {
      Id = id;
    }
  }
}
