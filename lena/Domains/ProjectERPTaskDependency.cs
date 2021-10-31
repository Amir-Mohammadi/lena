using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPTaskDependency : IEntity
  {
    protected internal ProjectERPTaskDependency()
    {
    }
    public int Id { get; set; }
    public int ProjectERPTaskId { get; set; }
    public int PredecessorProjectERPTaskId { get; set; }
    public ProjectERPTaskDependencyType DependencyType { get; set; }
    public int LagMinutues { get; set; } //زمان تاخیر 
    public byte[] RowVersion { get; set; }
    public virtual ProjectERPTask ProjectERPTask { get; set; }
    public virtual ProjectERPTask PredecessorProjectERPTask { get; set; }
  }
}
