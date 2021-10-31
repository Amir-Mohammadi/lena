using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LinkSerial : IEntity
  {
    protected internal LinkSerial()
    { }
    public int Id { get; set; }
    public string LinkedSerial { get; set; }
    public int UserId { get; set; }
    public Nullable<int> UserLinkerId { get; set; }
    public int CustomerId { get; set; }
    public DateTime DateTime { get; set; }
    public Nullable<DateTime> LinkDateTime { get; set; }
    public LinkSerialType Type { get; set; }
    public long StuffSerialCode { get; set; }
    public int StuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual User UserLinker { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual Cooperator Customer { get; set; }
    public virtual IranKhodroSerial IranKhodroSerial { get; set; }
  }
}