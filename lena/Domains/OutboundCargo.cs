using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OutboundCargo : Transport, IEntity
  {
    protected internal OutboundCargo()
    {
      this.ExitReceipts = new HashSet<ExitReceipt>();
    }
    public virtual ICollection<ExitReceipt> ExitReceipts { get; set; }
  }
}
