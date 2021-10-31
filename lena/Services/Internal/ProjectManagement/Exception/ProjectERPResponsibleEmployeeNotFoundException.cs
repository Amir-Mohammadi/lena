
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPResponsibleEmployeeNotFoundException : InternalServiceException
  {
    public int ProjectERPId { get; set; }
    public ProjectERPResponsibleEmployeeNotFoundException(int projectERPId)
    {
      this.ProjectERPId = projectERPId;
    }
  }
}
