using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectWorkItemNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProjectWorkItemNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
