using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionYear : IEntity
  {
    protected internal ProductionYear()
    {
      this.IranKhordoSerials = new HashSet<IranKhodroSerial>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime Year { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<IranKhodroSerial> IranKhordoSerials { get; set; }
  }
}
