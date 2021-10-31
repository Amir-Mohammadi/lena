using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialTransaction
{
  public class FinancialTransactionResult
  {
    public int? Id { get; set; }
    public int FinancialTransactionBatchId { get; set; }
    public int? BaseEntityId { get; set; }
    public double Amount { get; set; }
    public double RunningTotal { get; set; }
    public int CurrencyDecimalDigitCount { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? EffectDateTime { get; set; }
    public int FinancialAccountId { get; set; }
    public string FinancialAccountCode { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeFullName { get; set; }
    public string ProviderName { get; set; }
    public int FinancialTransactionTypeId { get; set; }
    public string Description { get; set; }
    public FinancialTransactionLevel FinancialTransactionTypeLevel { get; set; }
    public TransactionTypeFactor FinancialTransactionTypeFactor { get; set; }
    public byte[] RowVersion { get; set; }
    public int? PayRequestId { get; set; }
    public string QualityControlCode { get; set; }
    public int? PurchaseOrderGroupId { get; set; }
    public string PurchaseOrderGroupCode { get; set; }
    public int? PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public DateTime? PurchaseOrderDateTime { get; set; }
    public int? CargoId { get; set; }
    public string CargoCode { get; set; }
    public int? CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public DateTime? CargoDateTime { get; set; }
    public int? StuffId { get; set; }
    public bool? IsPermanent { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double? Qty { get; set; }
    public double? ReceiptedQty { get; set; }
    public double? AcceptedQty { get; set; }
    public double? RejectedQty { get; set; }
    public string UnitName { get; set; }
    public double? Price { get; set; }
    public string PlanCode { get; set; }
    public int? FinancialDocumentId { get; set; }
    public int? FinancialDocumentCorrectionId { get; set; }
    public bool? HasFinancialDocument { get; set; }
    public byte[] FinancialDocumentCorrectionRowVersion { get; set; }
    public FinancialDocumentType? FinancialDocumentType { get; set; }
    [JsonIgnore]
    public DiscountType? DiscountType { get; set; }
    [JsonIgnore]
    public CostType? CostType { get; set; }
    public FinancialDocumentTypeResult? FinancialDocumentTypeResultView { get; set; }
    public IsCorrectionFinancialTransaction IsCorrectionFinancialTransaction { get; set; }
    public FinancialDocumentTypeResult? FinancialDocumentTypeResult
    {
      get
      {
        if (DetailFinancialTransactions != null) return null;
        if (FinancialTransactionTypeId == StaticData.StaticFinancialTransactionTypes.SaleOfWaste.Id)
          return Domains.Enums.FinancialDocumentTypeResult.SaleOfWaste;
        if (FinancialTransactionTypeId == StaticData.StaticFinancialTransactionTypes.Giveback.Id)
          return Domains.Enums.FinancialDocumentTypeResult.Giveback;
        if (FinancialTransactionTypeId == StaticData.StaticFinancialTransactionTypes.ImportToPurchaseOrder.Id)
          return Domains.Enums.FinancialDocumentTypeResult.SubmitOrder;
        if (FinancialTransactionTypeId == StaticData.StaticFinancialTransactionTypes.ExportFromPurchase.Id)
          return Domains.Enums.FinancialDocumentTypeResult.DeliveryOrder;
        if (FinancialTransactionTypeId == StaticData.StaticFinancialTransactionTypes.ImportToCargo.Id)
          return Domains.Enums.FinancialDocumentTypeResult.FinancialFactor;
        if (FinancialTransactionTypeId == StaticData.StaticFinancialTransactionTypes.GivebackExitReceipt.Id)
          return Domains.Enums.FinancialDocumentTypeResult.GivebackExitReceipt;
        if (FinancialTransactionTypeId == StaticData.StaticFinancialTransactionTypes.QualityControlRejected.Id)
          return Domains.Enums.FinancialDocumentTypeResult.QualityControlRejected;
        switch (FinancialDocumentType)
        {
          case Domains.Enums.FinancialDocumentType.Beginning:
            return Domains.Enums.FinancialDocumentTypeResult.Beginning;
          case Domains.Enums.FinancialDocumentType.Correction:
            return Domains.Enums.FinancialDocumentTypeResult.Correction;
          case Domains.Enums.FinancialDocumentType.Deposit:
            return Domains.Enums.FinancialDocumentTypeResult.Deposit;
          case Domains.Enums.FinancialDocumentType.Transfer:
            return Domains.Enums.FinancialDocumentTypeResult.Transfer;
          case Domains.Enums.FinancialDocumentType.Discount:
            {
              if (DiscountType == null)
                return Domains.Enums.FinancialDocumentTypeResult.Discount;
              switch (DiscountType)
              {
                case Domains.Enums.DiscountType.PurchaseOrderGroup:
                case Domains.Enums.DiscountType.PurchaseOrderItem:
                  return Domains.Enums.FinancialDocumentTypeResult.PurchaseOrderDiscount;
                default:
                  return Domains.Enums.FinancialDocumentTypeResult.Discount;
              }
            }
          case Domains.Enums.FinancialDocumentType.Expense:
            {
              if (CostType == null)
                return Domains.Enums.FinancialDocumentTypeResult.Expense;
              switch (CostType)
              {
                case Domains.Enums.CostType.PurchaseOrderGroup:
                case Domains.Enums.CostType.PurchaseOrderItem:
                  return Domains.Enums.FinancialDocumentTypeResult.PurchaseOrderCost;
                case Domains.Enums.CostType.TransferCargo:
                case Domains.Enums.CostType.TransferCargoItems:
                  return Domains.Enums.FinancialDocumentTypeResult.CargoTransferCost;
                case Domains.Enums.CostType.DutyLading:
                case Domains.Enums.CostType.DutyLadingItems:
                  return Domains.Enums.FinancialDocumentTypeResult.LadingDutyCost;
                case Domains.Enums.CostType.TransferLading:
                case Domains.Enums.CostType.TransferLadingItems:
                  return Domains.Enums.FinancialDocumentTypeResult.LadingTransferCost;
                case Domains.Enums.CostType.LadingOtherCosts:
                  return Domains.Enums.FinancialDocumentTypeResult.OtherLadingCost;
                default:
                  return Domains.Enums.FinancialDocumentTypeResult.Expense;
              }
            }
        }
        if (PurchaseOrderGroupId != null)
          return Domains.Enums.FinancialDocumentTypeResult.SubmitOrder;
        if (CargoId != null)
          return Domains.Enums.FinancialDocumentTypeResult.DeliveryOrder;
        return null;
      }
    }
    public bool? IsCorrectionActive { get; set; }
    public int? ReferenceFinancialTransactionId { get; set; }
    public string FinancialAccountDescription { get; set; }
    public double OrderDebit { get; set; } // بدهکار سفارش
    public double OrderCredit { get; set; } // بستانکار سفارش
    public double OrderRunningTotalDebit { get; set; } // مانده بدهکار سفارش
    public double OrderRunningTotalCredit { get; set; } // مانده بستانکار سفارش
    public double AccountDebit { get; set; } // بدهکار مالی
    public double AccountDebitRial { get; set; } // بدهکار مالی به ریال
    public double AccountCredit { get; set; } // بستانکار مالی
    public double AccountCreditRial { get; set; } // بستانکار مالی به ریال
    public double? CurrencyRate { get; set; } // نرخ ارز به ریال
    public IEnumerable<int?> ReferenceRialRateFinancialTransactionIds { get; set; }
    public string ReferenceRialRateFinancialTransactionIdsString =>
        ReferenceRialRateFinancialTransactionIds == null
        ? ""
        : string.Join(", ", ReferenceRialRateFinancialTransactionIds.Distinct());
    public bool? IsRialRateValid { get; set; }
    public bool? IsRialRateUsed { get; set; }
    public bool? HasCurrencyRate
    {
      get
      {
        if (AccountDebit == 0 && AccountCredit == 0)
        {
          return null;
        }
        return CurrencyRate != null && CurrencyRate > 0;
      }
    }
    public double AccountRunningTotalDebit { get; set; } // مانده بدهکار مالی
    public double AccountRunningTotalDebitRial { get; set; } // مانده بدهکار مالی به ریال
    public double AccountRunningTotalCredit { get; set; } // مانده بستانکار مالی
    public double AccountRunningTotalCreditRial { get; set; } // مانده بستانکار مالی به ریال
    public IQueryable<FinancialTransactionResult> DetailFinancialTransactions { get; set; }
  }
}