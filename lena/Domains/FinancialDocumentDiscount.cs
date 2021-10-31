using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialDocumentDiscount : IEntity
  {
    protected internal FinancialDocumentDiscount()
    {
      this.PurchaseOrderDiscounts = new HashSet<PurchaseOrderDiscount>();
    }
    public int Id { get; set; }
    public DiscountType DiscountType { get; set; }
    public int FinancialDocumentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual FinancialDocument FinancialDocument { get; set; }
    public virtual ICollection<PurchaseOrderDiscount> PurchaseOrderDiscounts { get; set; }
  }
}
