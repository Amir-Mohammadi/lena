
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPEventDocumentNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPEventDocumentNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
