using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StockPlace : IEntity
  {
    protected internal StockPlace()
    {
      this.StuffStockPlaces = new HashSet<StuffStockPlace>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public short WarehouseId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StuffStockPlace> StuffStockPlaces { get; set; }
    public virtual Warehouse Warehouse { get; set; }
  }
}