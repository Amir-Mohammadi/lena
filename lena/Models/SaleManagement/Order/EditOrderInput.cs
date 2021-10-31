using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.SaleManagement.OrderDocument;
using lena.Models.SaleManagement.OrderItem;
using lena.Models.SaleManagement.PaymentDueDate;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Order
{
  public class EditOrderInput
  {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int OrderTypeId { get; set; }
    public TValue<string> Orderer { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public string DocumentNumber { get; set; }
    public OrderDocumentType DocumentType { get; set; }
    public EditOrderItemInput[] EditOrderItems { get; set; }
    public AddOrderItemInput[] AddOrderItems { get; set; }
    public RemoveOrderItemInput[] RemoveOrderItems { get; set; }
    public EditPaymentDueDateInput[] EditPaymentDueDates { get; set; }
    public AddPaymentDueDateInput[] AddPaymentDueDates { get; set; }
    public DeletePaymentDueDateInput[] DeletePaymentDueDates { get; set; }
    public AddOrderDocumentInput[] AddOrderDocumentInputs { get; set; }
    public EditOrderDocumentInput[] EditOrderDocumentInputs { get; set; }
    public DeleteOrderDocumentInput[] DeleteOrderDocumentInputs { get; set; }
    public double? TotalAmount { get; set; }
  }
}
