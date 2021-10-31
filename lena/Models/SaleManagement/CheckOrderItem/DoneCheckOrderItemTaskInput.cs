using System;
using lena.Models.SaleManagement.OrderItemSaleBlock;
using lena.Models.SaleManagement.ProductionRequest;
using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CheckOrderItem
{
  public class AddCheckOrderItemInput
  {
    public int OrderItemConfirmationId { get; set; }
    public byte[] OrderItemConfirmationRowVersion { get; set; }
    public string Description { get; set; }
    public bool Confirmed { get; set; }
    public double? SuggestedQty { get; set; }
    public DateTime? SuggestedDeliveryDate { get; set; }
    public AddOrderItemSaleBlockInput[] AddOrderItemSaleBlockInputs { get; set; }
    public AddProductionRequestInput[] AddProductionRequests { get; set; }
  }
}
