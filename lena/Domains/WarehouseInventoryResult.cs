using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarehouseInventoryResult : IEntity
  {
    protected internal WarehouseInventoryResult()
    {
    }
    public int Id { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int StuffId { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public short StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public Nullable<short> BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public Nullable<double> TotalAmount { get; set; }
    public Nullable<double> AvailableAmount { get; set; }
    public Nullable<double> BlockedAmount { get; set; }
    public Nullable<double> QualityControlAmount { get; set; }
    public Nullable<double> WasteAmount { get; set; }
    public Nullable<double> SerialBufferAmount { get; set; }
    public byte UnitId { get; set; }
    public byte DecimalDigitCount { get; set; }
    public string UnitName { get; set; }
    public string Serial { get; set; }
    public Nullable<StuffSerialStatus> SerialStatus { get; set; }
    public Nullable<DateTime> SerialProfileDateTime { get; set; }
    public StuffType StuffType { get; set; }
    public string StockPlaceCodes { get; set; }
    public string StockPlaceTitles { get; set; }
    public string QualityControlDescription { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> SerialProfileCode { get; set; }
    public DateTime? WarehouseEnterTime { get; set; }
    public int? IssueConfirmerUserId { get; set; }
    public string IssueConfirmerUserFullName { get; set; }
    public int? IssueUserId { get; set; }
    public string IssueUserFullName { get; set; }
    public double? UnitRialPrice { get; set; }
  }
}