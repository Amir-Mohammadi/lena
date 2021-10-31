
using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement.Exception
{
  public class ProjectERPLabelLogNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public ProjectERPLabelLogNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
