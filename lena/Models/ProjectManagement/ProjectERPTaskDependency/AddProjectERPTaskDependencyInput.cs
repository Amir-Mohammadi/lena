using lena.Models.Common;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTaskDependency
{
  public class AddProjectERPTaskDependencyInput
  {
    public int ProjectERPTaskId { get; set; }
    public int PredecessorProjectERPTaskId { get; set; }
    public ProjectERPTaskDependencyType DependencyType { get; set; }
    public int LagMinutues { get; set; } //زمان تاخیر 
  }
}
