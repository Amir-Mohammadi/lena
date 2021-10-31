using Newtonsoft.Json;
using lena.Domains.Enums;
using System;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItem
{
  public class OrderItemResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int OrderId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public byte StuffUnitTypeId { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public double Qty { get; set; }
    public double CanceledQty { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerCode { get; set; }
    public string Orderer { get; set; }
    public int OrderTypeId { get; set; }
    public string OrderTypeName { get; set; }
    public string DocumentNumber { get; set; }
    public OrderDocumentType? DocumentType { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public string OrderDescription { get; set; }
    public double ProducedQty { get; set; }
    public double PlannedQty { get; set; }
    public double BlockedQty { get; set; }
    public double BlockedQtyOtherCustomers { get; set; }
    public double PermissionQty { get; set; }
    public double SendedQty { get; set; }
    public double SentToOtherCustomersQty { get; set; }
    public double NotPostedQty { get; set; }
    public double NotPostedCanceledQty { get; set; }
    public OrderItemStatus Status { get; set; }
    public bool HasChange { get; set; }
    public bool IsArchive { get; set; }
    public bool? OrderItemConfirmationConfirmed { get; set; }
    public bool? OrderItemHasActivated { get; set; }
    public DateTime? OrderItemConfirmationDateTime { get; set; }
    public bool? CheckOrderItemConfirmed { get; set; }
    public DateTime? CheckOrderItemDateTime { get; set; }
    public OrderItemChangeStatus OrderItemChangeStatus { get; set; }
    public int? ProductPackBillOfMaterialStuffId { get; set; }
    public string ProductPackBillOfMaterialStuffCode { get; set; }
    public int? ProductPackBillOfMaterialVersion { get; set; }
    public bool? ProductPackBillOfMaterialIsPublished { get; set; }
    [JsonIgnore]
    public IEnumerable<ContactResult> ContactResults { get; set; }
    public string CustomerContactInfo
    {
      get
      {
        List<string> itemInfos = new List<string>();
        foreach (var item in ContactResults)
        {
          string result = "";
          result += item.Title;
          result += ": ";
          result += item.ContactText;

          itemInfos.Add(result);
        }

        return string.Join("\n", itemInfos);
      }
    }
    public byte[] RowVersion { get; set; }
    public byte[] OrderRowVersion { get; set; }
  }
}
