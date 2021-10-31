using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinanceItem : IEntity
  {
    protected internal FinanceItem()
    {
      this.FinanceItemConfirmations = new HashSet<FinanceItemConfirmation>();
    }
    public int Id { get; set; }
    public Nullable<int> FinanceId { get; set; }
    public Nullable<int> PurchaseOrderId { get; set; }
    public Nullable<int> ExpenseFinancialDocumentId { get; set; }
    public int CooperatorId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public FinanceType FinanceType { get; set; }
    public Nullable<PaymentKind> PaymentKind { get; set; }
    public Nullable<PaymentMethod> AcceptedPaymentMethod { get; set; }
    public string ChequeNumber { get; set; }
    public int UserId { get; set; }
    public double RequestedAmount { get; set; }
    public DateTime RequestedDateTime { get; set; }
    public DateTime RequestedDueDateTime { get; set; }
    public Nullable<DateTime> AcceptedDueDateTime { get; set; }
    public Nullable<double> AllocatedAmount { get; set; }
    public string Description { get; set; }
    public string FinancialDescription { get; set; }
    public Nullable<DateTime> ReceivedDateTime { get; set; }
    public Nullable<DateTime> ReceivedCreatedDateTime { get; set; }
    public Nullable<int> ReceivedUserId { get; set; }
    public int? LatestConfirmationId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Finance Finance { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual FinanceItemConfirmation LatestFinanceItemConfirmation { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual FinancialDocument FinancialDocument { get; set; }
    public virtual User User { get; set; }
    public virtual User ReceivedUser { get; set; }
    public virtual ICollection<FinanceItemConfirmation> FinanceItemConfirmations { get; set; }
    public virtual FinanceItemAllocationSummary FinanceItemAllocationSummary { get; set; }
  }
}