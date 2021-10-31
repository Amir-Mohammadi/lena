using lena.Models.Accounting.FinancialDocument;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditBankOrderIssueInput : EditFinancialDocumentInput
  {
    public int Id { get; set; }
    public string Number { get; set; }
    public int BankOrderIssueTypeId { get; set; }
    public int BankOrderId { get; set; }
    public int AllocationId { get; set; }
    public double NetAmountPaid { get; set; } // مبلغ خالص پرداخت شده 
    public int CurrencyId { get; set; }
    public double ConvertRate { get; set; } // نرخ تبدیل
    public double CurrencyFee { get; set; } // کارمزد ارزی
    public double RialFee { get; set; } // کارمزد ریالی
    public double DailyUSDRate { get; set; } //نرخ روز دلار
    public double FinishedCurrencyRate { get; set; } // نرخ تمام شده ارز
    public double DailyExchangeRateUSD { get; set; }  // قیمت روز دلاری ارز 
    public DateTime? SettlementDateTime { get; set; } // تاریخ تسویه

    public int FinancialDocumentId { get; set; }
    public byte[] FinancialDocumentRowVersion { get; set; }
    public byte[] RowVersion { get; set; }

  }
}