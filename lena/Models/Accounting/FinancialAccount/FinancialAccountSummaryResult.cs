using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccount
{
  public class FinancialAccountSummaryResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorCode { get; set; }
    public string CooperatorName { get; set; }
    public int? CurrencyId { get; set; }
    public int? CorrectionDocsCount { get; set; }
    public string CurrencyTitle { get; set; }
    public double? AccountDebitTotal { get; set; } // جمع بدهکار مالی
    public double? AccountCreditTotal { get; set; } // جمع بستانکار مالی
    public double? AccountDebitBalance { get; set; } // مانده بدهکار مالی  
    public double? AccountCreditBalance { get; set; } // مانده بستانکار مالی
    public double? OrderDebitTotal { get; set; } // جمع بدهکار سفارش
    public double? OrderCreditTotal { get; set; } // جمع بستانکار سفارش
    public double? OrderDebitBalance { get; set; } // مانده بدهکار سفارش
    public double? OrderCreditBalance { get; set; }// مانده بستانکار سفارش
    public double? PageTotalOrderDebit { get; set; }
    public double? PageTotalOrderCredit { get; set; }
    public double? PageTotalAccountDebit { get; set; }
    public double? PageTotalAccountCredit { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
