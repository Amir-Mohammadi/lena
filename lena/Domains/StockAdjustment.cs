using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StockAdjustment : BaseEntity, IEntity
  {
    protected internal StockAdjustment()
    {
    }
    public int StockCheckingTagId { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StockCheckingTag StockCheckingTag { get; set; }
    public virtual Unit Unit { get; set; }
  }
}