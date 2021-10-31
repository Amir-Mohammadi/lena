using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffPrice : BaseEntity, IEntity
  {
    protected internal StuffPrice()
    {
    }
    public double Price { get; set; }
    public StuffPriceType Type { get; set; }
    public StuffPriceStatus Status { get; set; }
    public int StuffId { get; set; }
    public byte CurrencyId { get; set; }
    public Nullable<int> ConfirmUserId { get; set; }
    public Nullable<DateTime> ConfirmDate { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual User ConfirmUser { get; set; }
  }
}