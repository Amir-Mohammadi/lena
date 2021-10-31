using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderDocument
{
  public class OrderDocumentResult
  {
    public int OrderCode { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }

    public string DocumentFullName;
    public Guid DocumentId { get; set; }
    public string DocumentName { get { return DocumentFullName.ToLower().Replace(DocumentId + "_", "").Replace(DocumentId.ToString(), ""); } }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
