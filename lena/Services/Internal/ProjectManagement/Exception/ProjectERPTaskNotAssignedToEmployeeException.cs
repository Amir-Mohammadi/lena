
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPTaskNotAssignedToEmployeeException : InternalServiceException
  {
    public int ProjectERPTaskId { get; set; }
    public int EmployeeId { get; set; }

    public ProjectERPTaskNotAssignedToEmployeeException(int projectERPTaskId, int employeeId)
    {
      this.ProjectERPTaskId = projectERPTaskId;
      this.EmployeeId = employeeId;

    }
  }
}
