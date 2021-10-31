using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssueItem
{
  public class DeleteWarehouseIssueItemInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
    public int WarehouseIssueId { get; set; }
  }
}
