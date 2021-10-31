using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderDocument
{
  public class EditOrderDocumentInput
  {
    public Guid DocumentId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
