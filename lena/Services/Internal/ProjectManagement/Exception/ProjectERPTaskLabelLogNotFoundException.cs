
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPTaskLabelLogNotFoundException : InternalServiceException
  {
    public int ProjectERPTaskId { get; set; }
    public short ProjectERPLabelId { get; set; }
    public ProjectERPTaskLabelLogNotFoundException(int projectERPTaskId, short projectERPLabelId)
    {
      this.ProjectERPTaskId = projectERPTaskId;
      this.ProjectERPLabelId = projectERPLabelId;
    }
  }
}
