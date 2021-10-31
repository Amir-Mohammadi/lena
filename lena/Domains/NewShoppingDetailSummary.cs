using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class NewShoppingDetailSummary : IEntity
  {
    protected internal NewShoppingDetailSummary()
    {
    }
    public int Id { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public double QualityControlConsumedQty { get; set; }
    public int NewShoppingDetailId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual NewShoppingDetail NewShoppingDetail { get; set; }
  }
}
