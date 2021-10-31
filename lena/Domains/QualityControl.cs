using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControl : BaseEntity, IEntity
  {
    protected internal QualityControl()
    {
      this.QualityControlItems = new HashSet<QualityControlItem>();
      this.GiveBackExitReceiptRequests = new HashSet<GiveBackExitReceiptRequest>();
      this.PaymentSuggestStatusLogs = new HashSet<PaymentSuggestStatusLog>();
    }
    public QualityControlType QualityControlType { get; set; }
    public int StuffId { get; set; }
    public short WarehouseId { get; set; }
    public QualityControlStatus Status { get; set; }
    public byte UnitId { get; set; }
    public double Qty { get; set; }
    public short DepartmentId { get; set; }
    public Nullable<int> EmployeeId { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public Nullable<DateTime> ConfirmationDateTime { get; set; }
    public Nullable<int> ConfirmationUserId { get; set; }
    public Nullable<QualityControlPaymentSuggestStatus> QualityControlPaymentSuggestStatus { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<QualityControlItem> QualityControlItems { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual QualityControlConfirmation QualityControlConfirmation { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Department Department { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual User ConfirmationUser { get; set; }
    public virtual QualityControlSummary QualityControlSummary { get; set; }
    public virtual ICollection<GiveBackExitReceiptRequest> GiveBackExitReceiptRequests { get; set; }
    public virtual ICollection<PaymentSuggestStatusLog> PaymentSuggestStatusLogs { get; set; }
    public virtual PayRequest PayRequest { get; set; }
  }
}