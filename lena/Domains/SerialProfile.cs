using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SerialProfile : IEntity
  {
    protected internal SerialProfile()
    {
      this.StuffSerials = new HashSet<StuffSerial>();
    }
    public int Code { get; set; }
    public int StuffId { get; set; }
    public DateTime DateTime { get; set; }
    public int CooperatorId { get; set; }
    public byte[] RowVersion { get; set; }
    public int UserId { get; set; }
    public virtual ICollection<StuffSerial> StuffSerials { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual User User { get; set; }
  }
}