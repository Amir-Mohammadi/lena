
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPTaskDependencyNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPTaskDependencyNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
