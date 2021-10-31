using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrderDetail : IEntity
  {
    protected internal BankOrderDetail()
    {
      this.CottageItems = new HashSet<CottageItem>();
    }
    public int Id { get; set; }
    public int Index { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public double Fee { get; set; }
    public int BankOrderId { get; set; }
    public int StuffHSGroupId { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public double GrossWeight { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual BankOrder BankOrder { get; set; }
    public virtual StuffHSGroup StuffHSGroup { get; set; }
    public virtual ICollection<CottageItem> CottageItems { get; set; }
  }
}