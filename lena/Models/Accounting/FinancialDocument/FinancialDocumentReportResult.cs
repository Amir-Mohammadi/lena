using lena.Domains.Enums;
using System;
using Newtonsoft.Json;
using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class FinancialDocumentReportResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int FinancialAccountId { get; set; }
    public string FinancialAccountCode { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public string FinancialAccountDescription { get; set; }

    [JsonIgnore]
    public CostType? CostType { get; set; }

    [JsonIgnore]
    public DiscountType? DiscountType { get; set; }

    public FinancialDocumentType Type { get; set; }
    public FinancialDocumentTypeResult? TypeResult
    {
      get
      {
        switch (Type)
        {
          case FinancialDocumentType.Beginning:
            return FinancialDocumentTypeResult.Beginning;

          case FinancialDocumentType.Correction:
            return FinancialDocumentTypeResult.Correction;

          case FinancialDocumentType.Deposit:
            return FinancialDocumentTypeResult.Deposit;

          case FinancialDocumentType.Transfer:
            return FinancialDocumentTypeResult.Transfer;

          case FinancialDocumentType.Discount:
            {
              if (DiscountType == null)
                return FinancialDocumentTypeResult.Discount;

              switch (DiscountType)
              {
                case Domains.Enums.DiscountType.PurchaseOrderGroup:
                case Domains.Enums.DiscountType.PurchaseOrderItem:
                  return FinancialDocumentTypeResult.PurchaseOrderDiscount;

                default:
                  return FinancialDocumentTypeResult.Discount;
              }
            }

          case FinancialDocumentType.Expense:
            {
              if (CostType == null)
                return FinancialDocumentTypeResult.Expense;

              switch (CostType)
              {
                case Domains.Enums.CostType.PurchaseOrderGroup:
                case Domains.Enums.CostType.PurchaseOrderItem:
                  return FinancialDocumentTypeResult.PurchaseOrderCost;

                case Domains.Enums.CostType.TransferCargo:
                case Domains.Enums.CostType.TransferCargoItems:
                  return FinancialDocumentTypeResult.CargoTransferCost;

                case Domains.Enums.CostType.DutyLading:
                case Domains.Enums.CostType.DutyLadingItems:
                  return FinancialDocumentTypeResult.LadingDutyCost;

                case Domains.Enums.CostType.TransferLading:
                case Domains.Enums.CostType.TransferLadingItems:
                  return FinancialDocumentTypeResult.LadingTransferCost;

                case Domains.Enums.CostType.LadingOtherCosts:
                  return FinancialDocumentTypeResult.OtherLadingCost;

                default:
                  return FinancialDocumentTypeResult.Expense;
              }
            }

          default:
            return null;
        }
      }
    }

    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime DocumentDateTime { get; set; }
    public string Description { get; set; }
    public double CreditAmount { get; set; }
    public double DebitAmount { get; set; }
    public int? FinancialDocumentTransferId { get; set; }
    public int? FinancialDocumentBeginningId { get; set; }
    public int? FinancialDocumentCorrectionId { get; set; }
    public int? ToFinancialAccountId { get; set; }
    public string ToFinancialAccountCode { get; set; }
    public int? ToCurrencyId { get; set; }
    public string ToCurrencyTitle { get; set; }
    public int? ToCooperatorId { get; set; }
    public string ToCooperatorName { get; set; }
    public double? ToCurrencyRate { get; set; }
    public double? ToAmount { get; set; }
    public double? FromAmount
    {
      get
      {
        if (CreditAmount > 0) return CreditAmount;
        return DebitAmount;
      }
    }
    public double? FinancialDocumentConvertRate { get; set; }
    public double? FinancialDocumentConvertAmount { get; set; }
    public double? FinancialDocumentConvertTax { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public int? FinanceId { get; set; }
    public string FinanceCode { get; set; }
    public double? CreditTransaction { get; set; }
    public double? DebitTransaction { get; set; }
    public int? FinancialTransactionBatchId { get; set; }
  }
}
