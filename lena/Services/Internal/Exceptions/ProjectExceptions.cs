using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class ProjectNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ProjectNotFoundException(int projectId)
    {
      Id = projectId;
    }
  }
}
