using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePrice
{
  public class StuffBasePriceResult
  {
    public long? Id { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public StuffPriceType Type { get; set; } = StuffPriceType.BasePrice;
    public DateTime? DateTime { get; set; }
    public StuffPriceStatus Status { get; set; }
    public double Price { get; set; }
    public double MainPrice { get; set; }
    public int? CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public byte CurrencyDecimalDigitCount { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
    public StuffBasePriceType StuffBasePriceType { get; set; }
    public int? CustomsId { get; set; }
    public StuffBasePriceCustomsType? CustomsType { get; set; } = StuffBasePriceCustomsType.Percentage;
    public double? CustomsPrice { get; set; }
    public double? CustomsPercent { get; set; }
    public int? CustomsHowToBuyId { get; set; }
    public string CustomsHowToBuyTitle { get; set; }
    public double? CustomsTariff { get; set; }
    public double? CustomsWeight { get; set; }
    public int? TransportId { get; set; }
    public StuffBasePriceTransportType? TransportType { get; set; } = StuffBasePriceTransportType.Percentage;
    public StuffBasePriceTransportComputeType? TransportComputeType { get; set; } = StuffBasePriceTransportComputeType.Weighing;
    public double? TransportPercent { get; set; }
    public int? CustomsCurrencyId { get; set; }
    public string Code { get; set; }
    public int? UserId { get; set; }
    public double? CustomsHowToBuyRatio { get; set; }
    public string ConfirmStuffBasePriceEmployeeFullName { get; set; }
    public DateTime? ConfirmStuffBasePriceDate { get; set; }
    public string Description { get; set; }

  }
}