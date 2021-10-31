using System;
using lena.Domains.Enums;
using lena.Models.Accounting.FinancialDocument;

using lena.Domains.Enums;
namespace lena.Models
{
  public class BankOrderIssueResult
  {
    public int Id { get; set; }
    public bool IsDelete { get; set; }
    public string BankOrderNumber { get; set; }
    public int FinancialDocumentId { get; set; }
    public byte[] FinancialDocumentRowVersion { get; set; }
    public FinancialDocumentType FinancialDocumentType { get; set; }
    public FinancialDocumentResult FinancialDocumentResult { get; set; }

    public string Number { get; set; }
    public int BankOrderIssueTypeId { get; set; }
    public int AllocationId { get; set; }
    public int BankOrderId { get; set; }
    public double NetAmountPaid { get; set; } // مبلغ خالص پرداخت شده
    public double ConvertRate { get; set; } // نرخ تبدیل
    public double CurrencyFee { get; set; } // کارمزد ارزی
    public double RialFee { get; set; } // کارمزد ریالی

    public double DailyUSDRate { get; set; } //نرخ روز دلار
    public double FinishedCurrencyRate { get; set; } // نرخ تمام شده ارز
    public double DailyExchangeRateUSD { get; set; }  // قیمت روز دلاری ارز 
    public DateTime? SettlementDateTime { get; set; } // تاریخ تسویه

    public int FinancialAccountId { get; set; }
    public string FinancialAccountCode { get; set; }
    public double FinancialDocumentCreditAmount { get; set; } // حساب بستانکار یا حساب مبداء
    public int CurrencyId { get; set; } // ارز حساب مبداء
    public string CurrencyTitle { get; set; }

    public int ToFinancialAccountId { get; set; }
    public string ToFinancialAccountCode { get; set; }
    public double FinancialDocumentDebitAmount { get; set; } // حساب بدهکار یا حساب مقصد
    public int ToCurrencyId { get; set; } // ارز حساب مقصد
    public string ToCurrencyTitle { get; set; }

    public FinancialDocumentType FinancialAccountType { get; set; }
    public string FinancialDocumentDescription { get; set; }
    public Guid? FinancialDocumentDocumentId { get; set; }
    public DateTime FinancialDocumentDocumentDateTime { get; set; }
    public byte[] FinancialDocumentDocumentRowVersion { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }


    public int? FinancialDocumentTransferId { get; set; }
    public byte[] FinancialDocumentTransferRowVersion { get; set; }
    public FinancialDocumentBankOrderResult FinancialDocumentBankOrder { get; set; }

    public byte[] RowVersion { get; set; }

  }
}