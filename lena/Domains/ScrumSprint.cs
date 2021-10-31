using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumSprint : ScrumEntity, IEntity
  {
    protected internal ScrumSprint()
    {
      this.ScrumBackLogs = new HashSet<ScrumBackLog>();
    }
    public int ScrumProjectId { get; set; }
    public virtual ScrumProject ScrumProject { get; set; }
    public virtual ICollection<ScrumBackLog> ScrumBackLogs { get; set; }
  }
}