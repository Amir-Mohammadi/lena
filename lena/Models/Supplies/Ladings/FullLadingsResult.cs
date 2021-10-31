using lena.Domains.Enums;
using lena.Models.Supplies.BankOrderLog;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class FullLadingResult
  {
    public FullLadingResult()
    {

    }

    public int Id { get; set; }
    public LadingType Type { get; set; }
    public int? LadingBlockerId { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public int? BankOrderId { get; set; }
    public double? CustomsValue { get; set; }
    public double? ActualWeight { get; set; }
    public int? BankOrderCurrencyId { get; set; }

    public DateTime DateTime { get; set; }
    public string BankOrderNumber { get; set; }
    public bool HasReceiptLicence { get; set; }
    public DateTime ReceiptLicenceDateTime { get; set; }

    public int? CustomhouseId { get; set; }
    public string CustomhouseName { get; set; }

    public string LadingCode { get; set; }
    public string KotazhCode { get; set; }
    public string SataCode { get; set; }
    public long? BoxCount { get; set; }
    public DateTime? TransportDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? BankOrderCurrencySourceId { get; set; }
    public double? BankOrderCurrencySourceFOB { get; set; }
    public double? BankOrderCurrencySourceTransferCost { get; set; }
    public byte[] RowVersion { get; set; }

    public IQueryable<LadingItemResult> LadingItems { get; set; }
    public IQueryable<BankOrderLogResult> BankOrderLogs { get; set; }
    public IQueryable<LadingCustomhouseLogResult> LadingCustomhouseLogs { get; set; }
    public IQueryable<LadingBankOrderLogResult> LadingBankOrderLogs { get; set; }
    public string BankName { get; set; }

    public int EmployeeId { get; set; }
    public bool NeedToCost { get; set; }
    public byte CountryId { get; set; }
    public short CityId { get; set; }
  }
}
