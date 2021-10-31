
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPTaskDocumentNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPTaskDocumentNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
