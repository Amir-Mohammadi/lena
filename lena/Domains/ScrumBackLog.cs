using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumBackLog : ScrumEntity, IEntity
  {
    protected internal ScrumBackLog()
    {
      this.ScrumTasks = new HashSet<ScrumTask>();
    }
    public int ScrumSprintId { get; set; }
    public virtual ICollection<ScrumTask> ScrumTasks { get; set; }
    public virtual ScrumSprint ScrumSprint { get; set; }
  }
}