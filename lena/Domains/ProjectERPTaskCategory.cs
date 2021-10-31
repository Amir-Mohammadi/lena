using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPTaskCategory : IEntity
  {
    protected internal ProjectERPTaskCategory()
    {
      this.ProjectERPTasks = new HashSet<ProjectERPTask>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProjectERPTask> ProjectERPTasks { get; set; }
  }
}
