using System.Linq;
using lena.Domains.Enums;
using lena.Models.SaleManagement.PaymentDueDate;
using lena.Models.SaleManagement.OrderDocument;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Order
{
  public class FullOrderResult
  {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int OrderTypeId { get; set; }
    public string Orderer { get; set; }
    public string Description { get; set; }
    public string DocumentNumber { get; set; }
    public OrderDocumentType? DocumentType { get; set; }
    public bool IsDeleted { get; set; }
    public byte[] RowVersion { get; set; }
    public double? TotalAmount { get; set; }
    public IQueryable<FullOrderItemResult> OrderItems { get; set; }
    public IQueryable<PaymentDueDateResult> PaymentDueDates { get; set; }

    public IQueryable<OrderDocumentResult> OrderDocuments { get; set; }
  }
}
