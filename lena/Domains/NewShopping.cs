using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class NewShopping : StoreReceipt, IEntity
  {
    protected internal NewShopping()
    {
      this.NewShoppingDetails = new HashSet<NewShoppingDetail>();
    }
    public double QtyPerBox { get; set; }
    public int BoxNo { get; set; }
    public Nullable<int> LadingItemId { get; set; }
    public virtual ICollection<NewShoppingDetail> NewShoppingDetails { get; set; }
    public virtual LadingItem LadingItem { get; set; }
  }
}
