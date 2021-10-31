using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumProject : ScrumEntity, IEntity
  {
    protected internal ScrumProject()
    {
      this.ScrumSprints = new HashSet<ScrumSprint>();
    }
    public int ScrumProjectGroupId { get; set; }
    public virtual ICollection<ScrumSprint> ScrumSprints { get; set; }
    public virtual ScrumProjectGroup ScrumProjectGroup { get; set; }
  }
}