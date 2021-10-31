using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CustomerStuff : IEntity
  {
    protected internal CustomerStuff()
    {
      this.CustomerStuffVersions = new HashSet<CustomerStuffVersion>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int StuffId { get; set; }
    public int CustomerId { get; set; }
    public CustomerStuffType Type { get; set; }
    public string ManufacturerCode { get; set; }
    public string TechnicalNumber { get; set; } // شماره فنی
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Cooperator Customer { get; set; }
    public virtual ICollection<CustomerStuffVersion> CustomerStuffVersions { get; set; }
    public virtual ICollection<IranKhodroSerial> IranKhordoSerials { get; set; }
  }
}
