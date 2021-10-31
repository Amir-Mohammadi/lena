
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPLabelNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPLabelNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
