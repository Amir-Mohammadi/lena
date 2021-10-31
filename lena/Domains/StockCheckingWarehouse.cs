using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StockCheckingWarehouse : IEntity
  {
    protected internal StockCheckingWarehouse()
    {
      this.StockCheckingTags = new HashSet<StockCheckingTag>();
    }
    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StockChecking StockChecking { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual ICollection<StockCheckingTag> StockCheckingTags { get; set; }
  }
}
