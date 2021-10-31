using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarehousePriceReportResult : IEntity
  {
    protected internal WarehousePriceReportResult()
    {
    }
    public int Id { get; set; }
    public Nullable<int> WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public Nullable<int> StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public Nullable<int> StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string Serial { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public double AvailableAmount { get; set; }
    public double BlockedAmount { get; set; }
    public double QualityControlAmount { get; set; }
    public double WasteAmount { get; set; }
    public double SerialBufferAmount { get; set; }
    public double TotalAmount { get; set; }
    public Nullable<int> UnitId { get; set; }
    public string UnitName { get; set; }
    public Nullable<DateTime> StuffLastTransactionDateTime { get; set; }
    public Nullable<double> LastStuffPrice { get; set; }
    public Nullable<DateTime> LastStuffPriceDateTime { get; set; }
    public Nullable<double> TotalAmountPrice { get; set; }
    public Nullable<int> CurrencyId { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyTitle { get; set; }
    public string CurrencySign { get; set; }
    public Nullable<byte> CurrencyDecimalDigitCount { get; set; }
    public byte[] RowVersion { get; set; }
  }
}