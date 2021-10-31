using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialDocumentCost : IEntity
  {
    protected internal FinancialDocumentCost()
    {
      this.CargoCosts = new HashSet<CargoCost>();
      this.LadingCosts = new HashSet<LadingCost>();
      this.PurchaseOrderCosts = new HashSet<PurchaseOrderCost>();
      this.BankOrderCosts = new HashSet<BankOrderCost>();
    }
    public int Id { get; set; }
    public CostType CostType { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<double> CargoWeight { get; set; }
    public Nullable<double> LadingWeight { get; set; }
    public Nullable<double> PurchaseOrderWeight { get; set; }
    public Nullable<double> EntranceRightsCost { get; set; } //هزینه حقوق ورودی
    public Nullable<double> KotazhTransPort { get; set; } //حمل کوتاژ    public virtual ICollection<CargoCost> CargoCosts { get; set; }
    public int FinancialDocumentId { get; internal set; }
    public virtual ICollection<CargoCost> CargoCosts { get; set; }
    //public int FinancialDocumentId { get; set; }
    public virtual FinancialDocument FinancialDocument { get; set; }
    public virtual ICollection<LadingCost> LadingCosts { get; set; }
    public virtual ICollection<PurchaseOrderCost> PurchaseOrderCosts { get; set; }
    public virtual ICollection<BankOrderCost> BankOrderCosts { get; set; }
  }
}