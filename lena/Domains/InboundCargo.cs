using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class InboundCargo : Transport, IEntity
  {
    protected internal InboundCargo()
    {
      this.InboundCargoCooperators = new HashSet<InboundCargoCooperator>();
      this.StoreReceipts = new HashSet<StoreReceipt>();
    }
    public short BoxCount { get; set; }
    public virtual ICollection<InboundCargoCooperator> InboundCargoCooperators { get; set; }
    public virtual ICollection<StoreReceipt> StoreReceipts { get; set; }
  }
}