using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CargoItemDetailSummary : IEntity
  {
    protected internal CargoItemDetailSummary()
    {
    }
    public int Id { get; set; }
    public double ReceiptedQty { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public double LadingItemDetailQty { get; set; }
    public int CargoItemDetailId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual CargoItemDetail CargoItemDetail { get; set; }
  }
}