using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TransactionSummary
{
  public class TransactionSummaryResult
  {
    public DateTime EffectDateTime { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public int StuffId { get; set; }
    public string StuffCategoryName { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double Amount { get; set; }
    public double TotalAmount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
  }
}
