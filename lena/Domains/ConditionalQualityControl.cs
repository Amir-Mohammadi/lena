using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ConditionalQualityControl : BaseEntity, IEntity
  {
    protected internal ConditionalQualityControl()
    {
      this.ConditionalQualityControlItems = new HashSet<ConditionalQualityControlItem>();
    }
    public int QualityControlAccepterId { get; set; }
    public int QualityControlConfirmationId { get; set; }
    public Nullable<int> WarrantyExpirationExceptionTypeId { get; set; }
    public Nullable<DateTime> ResponseConditionalConfirmationDate { get; set; }
    public Nullable<int> ResponseConditionalConfirmationUserId { get; set; }
    public ConditionalQualityControlStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual QualityControlAccepter QualityControlAccepter { get; set; }
    public virtual ICollection<ConditionalQualityControlItem> ConditionalQualityControlItems { get; set; }
    public virtual QualityControlConfirmation QualityControlConfirmation { get; set; }
    public virtual WarrantyExpirationExceptionType WarrantyExpirationExceptionType { get; set; }
    public virtual User ResponseConditionalConfirmationlUser { get; set; }
    public virtual ResponseConditionalQualityControl ResponseConditionalQualityControl { get; set; }
  }
}