using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectRelatedPeople : IEntity
  {
    protected internal ProjectRelatedPeople()
    {
    }
    public int Id { get; set; }
    public string Post { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Project Project { get; set; }
  }
}
