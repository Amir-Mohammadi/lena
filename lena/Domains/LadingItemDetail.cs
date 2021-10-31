using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingItemDetail : BaseEntity, IEntity
  {
    protected internal LadingItemDetail()
    {
      this.NewShoppingDetails = new HashSet<NewShoppingDetail>();
    }
    public double Qty { get; set; }
    public int LadingItemId { get; set; }
    public int CargoItemDetailId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual LadingItem LadingItem { get; set; }
    public virtual CargoItemDetail CargoItemDetail { get; set; }
    public virtual ICollection<NewShoppingDetail> NewShoppingDetails { get; set; }
    public virtual LadingItemDetailSummary LadingItemDetailSummary { get; set; }
  }
}