
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPTaskCategoryNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPTaskCategoryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
