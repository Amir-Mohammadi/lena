using System;
using System.Linq;
using lena.Models.SaleManagement.OrderItemSaleBlock;
using lena.Models.SaleManagement.ProductionRequest;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CheckOrderItem
{
  public class FullCheckOrderItemResult
  {

    public int Id { get; set; }
    public string Description { get; set; }
    public string OrderItemCode { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string OrderTypeName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public double Qty { get; set; }
    public string UnitName { get; set; }
    public byte UnitId { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public IQueryable<FullOrderItemSaleBlockResult> OrderItemSaleBlocks { get; set; }
    public IQueryable<FullProductionRequestResult> ProductionRequests { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
