using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PayRequest : IEntity, IHasSaveLog, IHasDescription, IRemovable, IHasFinancialTransaction
  {
    protected internal PayRequest()
    { }
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public double PayedAmount { get; set; }
    public double? DiscountedTotalPrice { get; set; }
    public User User { get; set; }
    public PayRequestStatus Status { get; set; }
    public int QualityControlId { get; internal set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual QualityControl QualityControl { get; set; }
    public int? FinancialTransactionBatchId { get; set; }
    public FinancialTransactionBatch FinancialTransactionBatch { get; set; }
    public Guid? DocumentId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}