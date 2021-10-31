using System;
using lena.Domains.Enums;
using lena.Models.Supplies.Ladings;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class CargoItemResult
  {
    public int Id { get; set; }

    public lena.Domains.PurchaseStep PurchaseStepItem { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public int CargoId { get; set; }
    public string CargoCode { get; set; }
    public int? LadingId { get; set; }
    public string LadingCode { get; set; }
    public double? LadingItemQty { get; set; }
    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public DateTime PurchaseOrderDateTime { get; set; }
    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string ProviderCode { get; set; }
    public int HowToBuyId { get; set; }
    public string HowToBuyTitle { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double PurchaseOrderQty { get; set; }
    public int PurchaseOrderUnitId { get; set; }
    public string PurchaseOrderUnitName { get; set; }
    public DateTime PurchaseOrderDeadline { get; set; }
    public string PurchaseOrderDescription { get; set; }
    public double? PurchaseOrderCargoedQty { get; set; }
    public double? PurchaseOrderRemainedQty { get; set; }
    public PurchaseOrderStatus PurchaseOrderStatus { get; set; }
    public string UnitName { get; set; }
    public byte UnitId { get; set; }
    public double Qty { get; set; }
    public double? ReceiptedQty { get; set; }
    public double RemainedQty { get; set; }
    public bool IsDelete { get; set; }
    public string EmployeeFullName { get; set; }
    public int EmployeeId { get; set; }
    public byte[] RowVersion { get; set; }
    public CargoItemStatus Status { get; set; }
    public double QualityControlFailedQty { get; set; }
    public double QualityControlPassedQty { get; set; }
    public IQueryable<CargoItemsWithLadinItem> Ladings { get; set; }
  }
}
