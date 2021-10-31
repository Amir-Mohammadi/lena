
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPDocumentNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPDocumentNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
