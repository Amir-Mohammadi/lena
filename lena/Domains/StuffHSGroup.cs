using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffHSGroup : IEntity
  {
    protected internal StuffHSGroup()
    {
      this.Stuffs = new HashSet<Stuff>();
      this.BankOrderDetails = new HashSet<BankOrderDetail>();
      this.CottageItems = new HashSet<CottageItem>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Stuff> Stuffs { get; set; }
    public virtual ICollection<BankOrderDetail> BankOrderDetails { get; set; }
    public virtual ICollection<CottageItem> CottageItems { get; set; }
  }
}