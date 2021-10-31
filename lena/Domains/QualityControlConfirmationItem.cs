using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlConfirmationItem : BaseEntity, IEntity
  {
    protected internal QualityControlConfirmationItem()
    {
      this.ConditionalQualityControlItems = new HashSet<ConditionalQualityControlItem>();
    }
    public double RemainedQty { get; set; }
    public double TestQty { get; set; }
    public double ConsumeQty { get; set; }
    public byte UnitId { get; set; }
    public int QualityControlConfirmationId { get; set; }
    public int QualityControlItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual QualityControlItem QualityControlItem { get; set; }
    public virtual QualityControlConfirmation QualityControlConfirmation { get; set; }
    public virtual ICollection<ConditionalQualityControlItem> ConditionalQualityControlItems { get; set; }
  }
}