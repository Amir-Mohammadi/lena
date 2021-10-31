using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ConditionalQualityControlItem : BaseEntity, IEntity
  {
    protected internal ConditionalQualityControlItem()
    {
    }
    public int ConditionalQualityControlId { get; set; }
    public int QualityControlConfirmationItemId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ConditionalQualityControl ConditionalQualityControl { get; set; }
    public virtual QualityControlConfirmationItem QualityControlConfirmationItem { get; set; }
    public virtual Unit Unit { get; set; }
  }
}