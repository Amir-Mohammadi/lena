using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPPhase
{
  public class AddProjectERPPhaseInput
  {
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
  }
}
