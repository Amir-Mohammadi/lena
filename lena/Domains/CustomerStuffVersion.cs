using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CustomerStuffVersion : IEntity
  {
    protected internal CustomerStuffVersion()
    {
      this.IranKhordoSerials = new HashSet<IranKhodroSerial>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public bool IsPublish { get; set; }
    public int CustomerStuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual CustomerStuff CustomerStuff { get; set; }
    public virtual ICollection<IranKhodroSerial> IranKhordoSerials { get; set; }
  }
}
