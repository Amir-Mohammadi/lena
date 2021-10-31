using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionLineRepairUnit : IEntity
  {
    protected internal ProductionLineRepairUnit()
    {
      this.ProductionLines = new HashSet<ProductionLine>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public short WarehouseId { get; set; }
    public DateTime CreationTime { get; set; }
    public int UserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<ProductionLine> ProductionLines { get; set; }
  }
}
