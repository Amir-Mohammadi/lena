using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumProjectGroup : ScrumEntity, IEntity
  {
    protected internal ScrumProjectGroup()
    {
      this.ScrumProjects = new HashSet<ScrumProject>();
    }
    public virtual ICollection<ScrumProject> ScrumProjects { get; set; }
  }
}