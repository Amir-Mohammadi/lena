using System;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemConfirmation
{
  public class OrderItemConfirmationResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeName { get; set; }
    public bool IsDelete { get; set; }
    public bool? Confirmed { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public int OrderItemId { get; set; }
    public string OrderItemCode { get; set; }
    public double Qty { get; set; }
    public double PlannedQty { get; set; }
    public double BlockedQty { get; set; }
    public double ProducedQty { get; set; }
    public double PermissionQty { get; set; }
    public double SendedQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public int OrderTypeId { get; set; }
    public string OrderTypeName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public StuffType StuffType { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string OrderItemDescription { get; set; }
    public string OrderDocumentNumber { get; set; }
    public OrderDocumentType? OrderDocumentType { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
