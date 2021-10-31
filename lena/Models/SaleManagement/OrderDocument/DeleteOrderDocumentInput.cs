using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderDocument
{
  public class DeleteOrderDocumentInput
  {
    public Guid DocumentId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
