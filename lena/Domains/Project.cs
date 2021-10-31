using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Project : ScrumProject, IEntity
  {
    protected internal Project()
    {
      this.ProjectRelatedPeoples = new HashSet<ProjectRelatedPeople>();
    }
    public virtual ICollection<ProjectRelatedPeople> ProjectRelatedPeoples { get; set; }
  }
}
