using System;
using lena.Models.Supplies.LadingCustomhouseLog;
using lena.Models.Supplies.LadingItemDetail;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class AddLadingInput
  {
    public LadingType Type { get; set; }
    public int? LadingBlockerId { get; set; }
    public int? BankOrderId { get; set; }
    public double? CustomsValue { get; set; }
    public double? ActualWeight { get; set; }
    public byte? CurrencyId { get; set; }
    public short? CustomhouseId { get; set; }
    public string LadingCode { get; set; }
    public string KotazhCode { get; set; }
    public string SataCode { get; set; }
    public short CityId { get; set; }
    public string Description { get; set; }
    public DateTime? TransportDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public bool NeedToCost { get; set; }
    public AddLadingItemDetailInput[] LadingItemDetails { get; set; }
    public AddLadingCustomhouseLogInput[] CustomhouseLogs { get; set; }
    public AddLadingBankOrderLogInput[] LadingBankOrderLogs { get; set; }
  }
}
