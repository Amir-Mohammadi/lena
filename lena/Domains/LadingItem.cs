using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingItem : BaseEntity, IEntity
  {
    protected internal LadingItem()
    {
      this.LadingCosts = new HashSet<LadingCost>();
      this.LadingItemDetails = new HashSet<LadingItemDetail>();
      this.NewShoppings = new HashSet<NewShopping>();
    }
    public double Qty { get; set; }
    public int LadingId { get; set; }
    public int CargoItemId { get; set; }
    public LadingItemStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Lading Lading { get; set; }
    public virtual CargoItem CargoItem { get; set; }
    public virtual ICollection<LadingCost> LadingCosts { get; set; }
    public virtual ICollection<LadingItemDetail> LadingItemDetails { get; set; }
    public virtual LadingItemSummary LadingItemSummary { get; set; }
    public virtual ICollection<NewShopping> NewShoppings { get; set; }
  }
}
