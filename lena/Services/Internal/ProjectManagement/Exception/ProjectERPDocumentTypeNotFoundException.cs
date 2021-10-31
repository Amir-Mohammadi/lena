
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPDocumentTypeNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPDocumentTypeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
