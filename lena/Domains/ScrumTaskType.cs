using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumTaskType : IEntity
  {
    protected internal ScrumTaskType()
    {
      this.ScrumTasks = new HashSet<ScrumTask>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ScrumTask> ScrumTasks { get; set; }
  }
}