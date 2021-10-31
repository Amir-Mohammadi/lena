using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectWorkNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProjectWorkNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
