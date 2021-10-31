using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTaskDependency
{
  public class ProjectERPTaskDependencyResult
  {
    public int Id { get; set; }
    public int ProjectERPTaskId { get; set; }
    public int PredecessorProjectERPTaskId { get; set; }
    public ProjectERPTaskDependencyType DependencyType { get; set; }
    public int LagMinutues { get; set; } //زمان تاخیر 
    public byte[] RowVersion { get; set; }
  }
}