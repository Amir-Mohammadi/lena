
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
