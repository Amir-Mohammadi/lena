using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PriceInquiry : IEntity
  {
    protected internal PriceInquiry()
    {
    }
    public int Id { get; set; }
    public int StuffId { get; set; }
    public int CooperatorId { get; set; }
    public byte CurrencyId { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public Nullable<int> Number { get; set; }
    public Nullable<double> Price { get; set; }
    public Nullable<DateTime> PriceAnnunciationDateTime { get; set; }
    public DateTime CreateDateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual User User { get; set; }
  }
}
