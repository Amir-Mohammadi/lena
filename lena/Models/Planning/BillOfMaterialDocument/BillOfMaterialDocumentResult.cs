using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDocument
{
  public class BillOfMaterialDocumentResult
  {
    public int Id { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public int BillOfMaterialDocumentTypeId { get; set; }
    public string BillOfMaterialDocumentTypeTitle { get; set; }
    public string BilOfMaterialStuffCode { get; set; }
    public string DocumentTitle { get; set; }
    public string DocumentDescription { get; set; }
    public Guid DocumentId { get; set; }
    public string FileName { get; set; }
    public DateTime CreationTime { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
    public Boolean IsDelete { get; set; }
  }
}
