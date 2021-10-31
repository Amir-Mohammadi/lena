using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingCost : IEntity
  {
    protected internal LadingCost()
    {
    }
    public int Id { get; set; }
    public int FinancialDocumentCostId { get; set; }
    public double Amount { get; set; }
    public Nullable<int> LadingId { get; set; }
    public int LadingItemId { get; set; }
    public bool IsTemp { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual FinancialDocumentCost FinancialDocumentCost { get; set; }
    public virtual Lading Lading { get; set; }
    public virtual LadingItem LadingItem { get; set; }
  }
}
