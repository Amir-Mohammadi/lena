using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public class ReturnOfSaleStoreReceiptResult
  {
    public int? ReceiptId { get; set; }
    public ReceiptStatus? ReceiptStatus { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public string EmployeeFullName { get; set; }

    public int StoreReceiptId { get; set; }
    public string StoreReceiptCode { get; set; }
    public int StoreReceiptStuffId { get; set; }
    public string StoreReceiptStuffCode { get; set; }
    public string StoreReceiptStuffName { get; set; }
    public DateTime? StoreReceiptDateTime { get; set; }
    public DateTime? ReceiptDateTime { get; set; }// تاریخ ثبت
    public DateTime? ReceiptReceiptDateTime { get; set; } // تاریخ رسید
    public StoreReceiptType StoreReceiptType { get; set; }
    public double StoreReceiptAmount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public double ReceiptQualityControlPassedQty { get; set; }
    public double ReceiptQualityControlFailedQty { get; set; }
    public double ReceiptQualityControlConsumedQty { get; set; }

    public int? ReturnOfSaleStuffId { get; set; }
    public string ReturnOfSaleStuffCode { get; set; }
    public string ReturnOfSaleStuffName { get; set; }
    public double? ReturnOfSaleQty { get; set; }
    public int? SendProductId { get; set; }
    public string SendProductCode { get; set; }
    public string Serial { get; set; }
    public int ReturnOfSaleId { get; set; }

    public int? ExitReceiptId { get; set; }
    public int? ExitReceiptStuffId { get; set; }
    public string ExitReceiptStuffCode { get; set; }
    public string ExitReceiptStuffName { get; set; }
    public double? ExitReceiptQty { get; set; }
    public DateTime? ExitReceiptDateTime { get; set; }
    public int? ReturnOfSaleMainStuffId { get; set; }
    public string ExitReceiptCode { get; set; }
    public double? ReturnOfSaleQualityControlConsumedQty { get; set; }
    public double? ReturnOfSaleQualityControlFailedQty { get; set; }
    public double? ReturnOfSaleQualityControlPassedQty { get; set; }
  }
}
