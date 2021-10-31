using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPPhase : IEntity
  {
    protected internal ProjectERPPhase()
    {
      this.ProjectERPs = new HashSet<ProjectERP>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProjectERP> ProjectERPs { get; set; }
  }
}
