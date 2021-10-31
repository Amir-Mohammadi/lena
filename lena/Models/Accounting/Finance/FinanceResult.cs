using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.Finance
{
  public class FinanceResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorCode { get; set; }
    public string CooperatorName { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int? FinancialAccountDetailId { get; set; }
    public int? FinancialAccountId { get; set; }
    public string FinancialAccountCode { get; set; }
    public string FinancialAccountDescription { get; set; }
    public string Account { get; set; }
    public string BankTitle { get; set; }
    public string AccountOwner { get; set; }
    public string EmployeeName { get; set; }
    public double TotalRequestAmount { get; set; }
    public double TotalAllocateAmount { get; set; }
    public double TotalTransferAmount { get; set; }
    public double TotalSeparatedTransferAmount { get; set; }
    public string Description { get; set; }
    public FinanceConfirmationStatus Status { get; set; }
    public FinanceTransferStatus FinanceTransferStatus { get; set; }
    public FinanceTransferStatus FinanceTransferInDetailStatus { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
