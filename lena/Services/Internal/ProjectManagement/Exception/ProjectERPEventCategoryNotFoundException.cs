
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPEventCategoryNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPEventCategoryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
