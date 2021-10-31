using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingItemSummary : IEntity
  {
    protected internal LadingItemSummary()
    {
    }
    public int Id { get; set; }
    public double ReceiptedQty { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public int LadingItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual LadingItem LadingItem { get; set; }
  }
}