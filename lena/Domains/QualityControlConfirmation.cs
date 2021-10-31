using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlConfirmation : BaseEntity, IEntity
  {
    protected internal QualityControlConfirmation()
    {
      this.QualityControlConfirmationTests = new HashSet<QualityControlConfirmationTest>();
      this.ConditionalQualityControls = new HashSet<ConditionalQualityControl>();
      this.QualityControlConfirmationItems = new HashSet<QualityControlConfirmationItem>();
    }
    public QualityControlStatus Status { get; set; }
    public Nullable<bool> IsEmergency { get; set; }
    public int QualityControlId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual QualityControl QualityControl { get; set; }
    public virtual ICollection<QualityControlConfirmationTest> QualityControlConfirmationTests { get; set; }
    public virtual ICollection<ConditionalQualityControl> ConditionalQualityControls { get; set; }
    public virtual ICollection<QualityControlConfirmationItem> QualityControlConfirmationItems { get; set; }
  }
}