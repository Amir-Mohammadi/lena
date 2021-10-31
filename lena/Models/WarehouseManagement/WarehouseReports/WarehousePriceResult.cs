using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseReports
{
  public class WarehousePriceResult
  {
    public int? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int? StuffId { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public int? StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public Nullable<int> BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public double TotalAmount { get; set; }
    public double AvailableAmount { get; set; }
    public double BlockedAmount { get; set; }
    public double QualityControlAmount { get; set; }
    public double WasteAmount { get; set; }
    public double SerialBufferAmount { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public string Serial { get; set; }
    public Nullable<StuffSerialStatus> SerialStatus { get; set; }
    public Nullable<System.DateTime> SerialProfileDateTime { get; set; }
    public Nullable<int> SerialProfileCode { get; set; }
    public StuffType StuffType { get; set; }
    public Nullable<int> LastStuffPriceId { get; set; }
    public Nullable<double> LastStuffPrice { get; set; }
    public Nullable<double> TotalAmountPrice { get; set; }
    public Nullable<StuffPriceType> StuffPriceType { get; set; }
    public Nullable<StuffPriceStatus> StuffPriceStatus { get; set; }
    public Nullable<int> CurrencyId { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyTitle { get; set; }
    public string CurrencySign { get; set; }
    public Nullable<byte> CurrencyDecimalDigitCount { get; set; }
    public Nullable<int> ConfirmUserId { get; set; }
    public string ConfirmerFullName { get; set; }
    public Nullable<System.DateTime> ConfirmDate { get; set; }
    public Nullable<System.DateTime> LastStuffPriceDateTime { get; set; }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
