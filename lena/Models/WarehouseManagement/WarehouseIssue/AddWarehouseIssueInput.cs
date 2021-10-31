using lena.Domains.Enums;
using lena.Models.WarehouseManagement.WarehouseTransaction;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssue
{
  public class AddWarehouseIssueInput
  {
    public short FromWarehouseId { get; set; }
    public short? ToWarehouseId { get; set; }
    public AddWarehouseIssueItemInput[] AddWarehouseIssueItems { get; set; }
    public string Description { get; set; }
    public int? ToEmployeeId { get; set; }
    public short? ToDepartmentId { get; set; }
    public TransactionLevel? TransactionLevel { get; set; }

  }
}
