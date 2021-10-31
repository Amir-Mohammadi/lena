using lena.Domains.Enums;
using lena.Models.SaleManagement.PaymentDueDate;
using lena.Models.SaleManagement.OrderDocument;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddOrderInput
  {
    public int CustomerId { get; set; }
    public int OrderTypeId { get; set; }
    public string Orderer { get; set; }
    public string Description { get; set; }
    public string DocumentNumber { get; set; }
    public OrderDocumentType DocumentType { get; set; }
    public AddOrderItemInput[] OrderItems { get; set; }
    public AddPaymentDueDateInput[] PaymentDueDates { get; set; }

    public AddOrderDocumentInput[] AddOrderDocumentInputs { get; set; }
    public double? TotalAmount { get; set; }
  }
}