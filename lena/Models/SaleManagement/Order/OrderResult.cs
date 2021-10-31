using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Order
{
  public class OrderResult
  {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Orderer { get; set; }
    public string Description { get; set; }
    public int OrderTypeId { get; set; }
    public string OrderTypeName { get; set; }
    public string DocumentNumber { get; set; }
    public OrderDocumentType? DocumentType { get; set; }
    public bool IsDeleted { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
