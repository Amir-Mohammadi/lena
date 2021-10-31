using lena.Models.WarehouseManagement.WarehouseIssue;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.RequestWarehouseIssue
{
  public class AddRequestWarehouseIssueInput : AddWarehouseIssueInput
  {
    public int[] ResponseStuffRequestItemIds { get; set; }
    public bool AllowChangeOrder { get; set; }
  }
}
