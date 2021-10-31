using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrderIssue : IEntity
  {
    protected internal BankOrderIssue()
    {
    }
    public int Id { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int FinancialDocumentId { get; set; }
    public string Number { get; set; }
    public int BankOrderIssueTypeId { get; set; }
    public int AllocationId { get; set; }
    public double NetAmountPaid { get; set; } // مبلغ خالص پرداخت شده
    public double ConvertRate { get; set; } // نرخ تبدیل
    public double CurrencyFee { get; set; } // کارمزد ارزی
    public double RialFee { get; set; } // کارمزد ریالی
    public double DailyUSDRate { get; set; } //نرخ روز دلار
    public double FinishedCurrencyRate { get; set; } // نرخ تمام شده ارز
    public double DailyExchangeRateUSD { get; set; }  // قیمت روز دلاری ارز     public byte[] RowVersion { get; set; }    public virtual BankOrderIssueType BankOrderIssueType { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual BankOrderIssueType BankOrderIssueType { get; set; }
    public virtual Allocation Allocation { get; set; }
    public virtual FinancialDocument FinancialDocument { get; set; }
  }
}