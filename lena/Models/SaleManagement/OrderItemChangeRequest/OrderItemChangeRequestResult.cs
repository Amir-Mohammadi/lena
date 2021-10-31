using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemChangeRequest
{
  public class OrderItemChangeRequestResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int OrderItemId { get; set; }
    public string Description { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime DateTime { get; set; }
    public double OrderItemQty { get; set; }
    public int OrderItemUnitId { get; set; }
    public string OrderItemUnitName { get; set; }
    public DateTime OrderItemRequestDate { get; set; }
    public DateTime OrderItemDeliveryDate { get; set; }
    public string OrderItemCode { get; set; }
    public string OrderItemDescription { get; set; }
    public int OrderId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int StuffId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerCode { get; set; }
    public string Orderer { get; set; }
    public OrderItemChangeStatus OrderItemChangeStatus { get; set; }
    public int OrderTypeId { get; set; }
    public string OrderTypeName { get; set; }
    public string EmployeeFullName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public byte[] RowVersion { get; set; }
    public bool IsActive { get; set; }
  }
}
