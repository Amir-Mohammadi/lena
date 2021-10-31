using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class IranKhodroSerial : IEntity
  {
    protected internal IranKhodroSerial()
    {
    }
    public int Id { get; set; }
    public int ProductionYearId { get; set; }
    public int CustomerStuffId { get; set; }
    public int CustomerStuffVersionId { get; set; }
    public DateTime ProductionDateTime { get; set; }
    //[Range(000, 999)]
    public int ProductionDay { get; set; }
    //[Range(0000, 9999)]
    public int ProductionSerial { get; set; }
    public bool Print { get; set; }
    public int PrintQty { get; set; }
    public int UserId { get; set; }
    public int? LinkSerialId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual ProductionYear ProductionYear { get; set; }
    public virtual CustomerStuff CustomerStuff { get; set; }
    public virtual CustomerStuffVersion CustomerStuffVersion { get; set; }
    public virtual LinkSerial LinkSerial { get; set; }
  }
}
