using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ReturnOfSale
{
  public class ReturnOfSaleResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int? ReceiptId { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int InboundCargoId { get; set; }
    public string InboundCargoCode { get; set; }
    public string ExitReceiptCode { get; set; }
    public double QtyPerBox { get; set; }
    public double BoxNo { get; set; }
    public double Amount { get; set; }
    public string Serial { get; set; }
    public int? MainStuffId { get; set; }
    public string MainStuffCode { get; set; }
    public string MainStuffName { get; set; }
    public string MainStuffNone { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public ReturnOfSaleStatus Status { get; set; }
    public int? SendProductId { get; set; }
    public string SendProductCode { get; set; }
    public int StoreReceiptId { get; set; }
    public string StoreReceiptCode { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }




  }
}
