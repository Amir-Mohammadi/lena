using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPLabel : IEntity
  {
    protected internal ProjectERPLabel()
    {
      this.ProjectERPLabelLogs = new HashSet<ProjectERPLabelLog>(); this.ProjectERPTaskLabelLogs = new HashSet<ProjectERPTaskLabelLog>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProjectERPLabelLog> ProjectERPLabelLogs { get; set; }
    public virtual ICollection<ProjectERPTaskLabelLog> ProjectERPTaskLabelLogs { get; set; }
  }
}
