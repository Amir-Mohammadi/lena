using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseOrderCost : IEntity
  {
    protected internal PurchaseOrderCost()
    {
    }
    public int Id { get; set; }
    public double Amount { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> PurchaseOrderGroupId { get; set; }
    public int PurchaseOrderId { get; set; }
    public int FinancialDocumentCostId { get; set; }
    public virtual PurchaseOrderGroup PurchaseOrderGroup { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual FinancialDocumentCost FinancialDocumentCost { get; set; }
  }
}