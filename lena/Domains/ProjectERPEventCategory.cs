using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPEventCategory : IEntity
  {
    protected internal ProjectERPEventCategory()
    {
      this.ProjectERPEvents = new HashSet<ProjectERPEvent>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProjectERPEvent> ProjectERPEvents { get; set; }
  }
}
