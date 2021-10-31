using lena.Domains.Enums;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrder
{
  public class BankOrderResult
  {
    public int Id { get; set; }
    public double? SumBankOrderIssueCreditAmount { get; set; }
    public int? BankOrderIssueCurrencyId { get; set; }
    public string BankOrderIssueCurrencyTitle { get; set; }
    public double? SumBankOrderIssueDebitAmount { get; set; }
    public int? ToBankOrderIssueCurrencyId { get; set; }
    public string ToBankOrderIssueCurrencyTitle { get; set; }
    public double? SumBankOrderCurrencySource { get; set; }// کل وزن ناخلص منشاء ارز
    public double? SumBankOrderDetailWeight { get; set; } //کل وزن ناخالص سفارش بانکی
    public double? RemainBankOrderWeight => SumBankOrderDetailWeight - SumBankOrderCurrencySource;
    public bool HasEnactment { get; set; }
    public int? EnactmentId { get; set; }
    //public int? LastBankOrderIssueTypeId { get; set; } //آخرین نوع حواله ثبت شده برای سفارش بانکی مورد نظر
    //public string LastBankOrderIssueTypeName { get; set; }
    public int StuffPriority { get; set; }


    public DateTime? SettlementDateTime { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; set; }
    public string OrderNumber { get; set; }
    public string FolderCode { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public DateTime DateTime { get; set; }
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int UserId { get; set; }
    public string RegisterEmployeeFullName { get; set; }
    public BankOrderStatus Status { get; set; }
    public BankOrderType BankOrderType { get; set; }
    public int BankId { get; set; }
    public string BankName { get; set; }
    public int CustomhouseId { get; set; }
    public string CustomhouseName { get; set; }
    public int CountryId { get; set; }

    public double TransferCost { get; set; }
    public double FOB { get; set; }
    public double TotalAmount { get; set; }

    public double DepositedTransferCost { get; set; }
    public double RemainingTransferCost => TransferCost - DepositedTransferCost;

    public double DepositedFOB { get; set; }
    public double RemainingFOB => FOB - DepositedFOB;

    public double DepositedAmount { get; set; }
    public double RemainingAmount => TotalAmount - DepositedAmount;
    public double RemainingFulfillmentCommitment => DepositedAmount - DepositedFOB - DepositedTransferCost; // مانده رفع تعهد شده

    public string CountryName { get; set; }
    public string Description { get; set; }
    public string CurrentBankOrderStatusTypeName { get; set; }
    public int? CurrentBankOrderStatusTypeId { get; set; }
    public DateTime? CurrentDateTime { get; set; }
    public bool? CheckCurrentDateTime { get; set; }
    public string CurrentEmployeeFullName { get; set; }
    public string CurrentDescription { get; set; }
    public IQueryable<int> BankOrderStatusType { get; set; }
    public string BankOrderContractTypeTitle { get; set; }
    public short BankOrderContractTypeId { get; set; }
    public bool IsDelete { get; set; }
    public int EmployeeId { get; set; }
    public double? CustomsValue { get; set; }
    public IQueryable<string> LadingCodes { get; set; }
    public DateTime? AllocationFinalizationDateTime { get; set; }
    public AllocationStatus? AllocationStatus { get; set; }
    public bool WithoutCurrencyTransfer { get; set; }

  }
}
