using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Cargo : BaseEntity, IEntity
  {
    protected internal Cargo()
    {
      this.CargoItems = new HashSet<CargoItem>();
      this.CargoCosts = new HashSet<CargoCost>();
    }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<CargoItem> CargoItems { get; set; }
    public virtual ICollection<CargoCost> CargoCosts { get; set; }
  }
}