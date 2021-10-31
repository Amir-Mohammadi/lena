using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class NewShoppingDetail : BaseEntity, IEntity
  {
    protected internal NewShoppingDetail()
    {
    }
    public int NewShoppingId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public Nullable<int> LadingItemDetailId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual NewShopping NewShopping { get; set; }
    public virtual NewShoppingDetailSummary NewShoppingDetailSummary { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual LadingItemDetail LadingItemDetail { get; set; }
  }
}
