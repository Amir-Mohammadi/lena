using System;
using System.Collections.Generic;
using System.Linq;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class LadingResult
  {
    public int Id { get; set; }
    public LadingType Type { get; set; }
    public bool HasLadingChangeRequest { get; set; }
    public bool HasReceiptLicence { get; set; }
    public DateTime ReceiptLicenceDateTime { get; set; }
    public IEnumerable<LadingItemStatus> LadingItemStatuses { get; set; }
    public IQueryable<LadingItemResult> LadingItems { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public int? BankOrderId { get; set; }
    public double? CustomsValue { get; set; }
    public double? ActualWeight { get; set; }
    public int? BankOrderCurrencyId { get; set; }
    public DateTime? BankOrderRegisterDate { get; set; }
    public string BankOrderCurrencyTitle { get; set; }
    public DateTime DateTime { get; set; }
    public string BankOrderNumber { get; set; }
    public string LadingCode { get; set; }
    public string KotazhCode { get; set; }
    public string SataCode { get; set; }
    public int? CustomhouseId { get; set; }
    public string CustomhouseName { get; set; }
    public long? BoxCount { get; set; }
    public DateTime? TransportDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public byte[] RowVersion { get; set; }
    public string CurrentLadingBankOrderLogDescription { get; set; }
    public string CurrentLadingBankOrderStatusCode { get; set; }
    public string CurrentLadingBankOrderStatusName { get; set; }
    public int? CurrentLadingBankOrderStatusId { get; set; }
    public string CurrentBankOrderLogDescription { get; set; }
    public int? CurrentBankOrderStatusId { get; set; }
    public int? CurrentCustomhouseStatusId { get; set; }
    public string CurrentBankOrderStatusCode { get; set; }
    public string CurrentCustomhouseStatusCode { get; set; }
    public string CurrentBankOrderStatusName { get; set; }
    public DateTime? CurrentBankOrderLogDateTime { get; set; }
    public bool CheckCurrentBankOrderLogDateTime { get; set; }
    public string CurrentCustomhouseStatusName { get; set; }
    public DateTime? CurrentCustomhouseLogDateTime { get; set; }
    public bool CheckCurrentLadingCustomhouseLogDateTime { get; set; }
    public string CurrentCustomhouseLogDescription { get; set; }
    public string LatestBaseEntityDocumentDescription { get; set; }
    public string BankName { get; set; }
    public DateTime? BankOrderExpireDate { get; set; }
    public string BankOrderStatus { get; set; }
    public double? GrossWeightSum { get; set; }
    public bool IsLocked { get; set; }
    public int? EmployeeId { get; set; }

    #region TransferLadingCost
    public double? TransferCost { get; set; }
    public double? TransferCostPerUnit => ActualWeight == 0 || TransferCost == 0 ? (double?)null : TransferCost / ActualWeight;
    public int? TransferLadingCostFinancialAccountCurrencyId { get; set; }
    public string TransferLadingCostFinancialAccountCurrencyTitle { get; set; }
    #endregion

    public IEnumerable<string> PlanCodes { get; set; }


    #region DutyLadingCost
    public double? KotazhTransPortSum { get; set; }
    public double? EntranceRightsCostSum { get; set; }
    public string DutyLadingFinancialAccountCurrencyTitle { get; set; }
    public int? DutyLadingFinancialAccountCurrencyId { get; set; }
    #endregion

    #region OtherLadingCost
    public double? OtherCostForLadingAmount { get; set; }
    public int? OtherCostForLadingFinancialAccountCurrencyId { get; set; }
    public string OtherCostForLadingFinancialAccountCurrencyTitle { get; set; }
    #endregion


    #region 
    public double? StuffValues { get; set; }
    public int? PurchaseOrderCurrencyId { get; set; }
    public string PurchaseOrderCurrencyTitle { get; set; }

    public int? BankOrderCurrencySourceId { get; set; }
    public double? BankOrderCurrencySourceFOB { get; set; }
    public double? BankOrderCurrencySourceTransferCost { get; set; }
    #endregion


    public bool NeedToCost { get; set; }
    public string CountryTitle { get; set; }
    public string CityTitle { get; set; }
  }
}
