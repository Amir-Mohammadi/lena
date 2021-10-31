using lena.Domains.Enums;

using lena.Models.Supplies.LadingCustomhouseLog;
using System;
using lena.Models.Supplies.LadingItemDetail;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class EditLadingInput
  {
    public int Id { get; set; }
    public LadingType Type { get; set; }
    public int? LadingBlockerId { get; set; }
    public short? CustomhouseId { get; set; }
    public int? BankOrderId { get; set; }
    public double? CustomsValue { get; set; }
    public byte? CurrencyId { get; set; }
    public double? ActualWeight { get; set; }


    public string LadingCode { get; set; }
    public string KotazhCode { get; set; }
    public string SataCode { get; set; }
    public DateTime? TransportDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string Description { get; set; }

    public byte[] RowVersion { get; set; }

    public AddLadingCustomhouseLogInput[] CustomhouseLogs { get; set; }
    public AddLadingBankOrderLogInput[] LadingBankOrderLogs { get; set; }

    public int[] DeleteCustomhouseLogIds { get; set; }
    public int[] DeleteladingBankOrderLogIds { get; set; }
    public int[] DeleteBankOrderLogIds { get; set; }
    public Nullable<short> CityId { get; set; }
    public AddLadingItemDetailInput[] AddLadingItemDetails { get; set; }
    public EditLadingItemDetailInput[] EditLadingItemDetails { get; set; }
    public DeleteLadingItemDetailInput[] DeleteLadingItemDetails { get; set; }
  }
}
