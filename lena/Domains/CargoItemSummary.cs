using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CargoItemSummary : IEntity
  {
    protected internal CargoItemSummary()
    {
    }
    public int Id { get; set; }
    public double ReceiptedQty { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public int CargoItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public double LadingItemQty { get; set; }
    public virtual CargoItem CargoItem { get; set; }
  }
}