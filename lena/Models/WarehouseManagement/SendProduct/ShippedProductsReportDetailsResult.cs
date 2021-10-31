using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class ShippedProductsReportDetailsResult
  {
    public string ShippingCode { get; set; }
    public string Barcode { get; set; }
    public string StuffCode { get; set; }
    public string StuffNoun { get; set; }
    public string StuffEnName { get; set; }
    public float Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime SentDate { get; set; }
  }
}
