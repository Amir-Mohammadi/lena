using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Forwarder : IEntity
  {
    protected internal Forwarder()
    {
      this.CargoItems = new HashSet<CargoItem>();
    }
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<CargoItem> CargoItems { get; set; }
  }
}
