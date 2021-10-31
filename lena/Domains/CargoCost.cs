using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CargoCost : IEntity
  {
    protected internal CargoCost()
    {
    }
    public int Id { get; set; }
    public int FinancialDocumentCostId { get; set; }
    public double Amount { get; set; }
    public Nullable<int> CargoId { get; set; }
    public int CargoItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual FinancialDocumentCost FinancialDocumentCost { get; set; }
    public virtual Cargo Cargo { get; set; }
    public virtual CargoItem CargoItem { get; set; }
  }
}