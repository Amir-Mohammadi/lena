using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseOrderDiscount : IEntity
  {
    protected internal PurchaseOrderDiscount()
    {
    }
    public int Id { get; set; }
    public double Amount { get; set; }
    public byte[] RowVersion { get; set; }
    public int FinancialDocumentDiscountId { get; set; }
    public Nullable<int> PurchaseOrderGroupId { get; set; }
    public int PurchaseOrderId { get; set; }
    public virtual FinancialDocumentDiscount FinancialDocumentDiscount { get; set; }
    public virtual PurchaseOrderGroup PurchaseOrderGroup { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
  }
}