using System;
using lena.Domains.Enums;
namespace lena.Models.Accounting.RialRatesList
{
  public class RialRateResult
  {
    public int RialRateId { get; set; }
    public int FinancialTransactionId { get; set; }
    public double Rate { get; set; }
    public int? RecorderId { get; set; }
    public string RecorderFullName { get; set; }
    public double Amount { get; set; }
    public int? OriginCurrencyId { get; set; }
    public string OriginCurrency { get; set; }
    public DateTime TransactionDate { get; set; }
    public int? SupplierId { get; set; }
    public string SupplierFullName { get; set; }
  }
}