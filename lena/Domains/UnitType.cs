using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class UnitType : IEntity
  {
    protected internal UnitType()
    {
      this.Units = new HashSet<Unit>();
      this.Stuffs = new HashSet<Stuff>();
    }
    public byte Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Unit> Units { get; set; }
    public virtual ICollection<Stuff> Stuffs { get; set; }
    public virtual ICollection<StuffDefinitionRequest> StuffDefinitionRequests { get; set; }
  }
}